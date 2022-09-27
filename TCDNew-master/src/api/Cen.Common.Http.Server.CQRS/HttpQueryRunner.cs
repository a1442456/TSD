using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SimpleInjector;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Errors;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Cen.Common.Http.Server.CQRS
{
    public class HttpQueryRunner
    {
        private readonly Container _container;
        private readonly ILogger _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HttpQueryRunner(Container container)
        {
            _container = container;
            _logger = container.GetInstance<ILogger>();
            _jsonSerializerOptions = container.GetInstance<JsonSerializerOptions>();
        }

        public async Task Run<TReq, TResp, TProcessor>(HttpContext context) 
            where TProcessor: class, IQueryProcessor<TReq, TResp>
        {
            try
            {
                await using var scope = new Scope(_container);

                using var requestStreamReader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
                var requestString = await requestStreamReader.ReadToEndAsync();

                // TODO: introduce request/response logging

                TReq request = default;
                if (requestString.Length > 0)
                    request = JsonSerializer.Deserialize<TReq>(requestString, _jsonSerializerOptions);

                IServiceProvider serviceProvider = _container;
                var validator = (IValidator<TReq>)serviceProvider.GetService(typeof(IValidator<TReq>));
                if (validator != null)
                {
                    var requestValidationResult = await validator.ValidateAsync(request);
                    if (!requestValidationResult.IsValid)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return;
                    }
                }

                var processor = scope.GetInstance<TProcessor>();
                var accessValidationResult = ValidateAccess(context, processor);
                if (accessValidationResult)
                {
                    var userIdProvider = new UserIdProvider(context);
                    var response = await processor.Run(userIdProvider, request);

                    // TODO: introduce request/response logging

                    var responseString = JsonSerializer.Serialize(response, _jsonSerializerOptions);
                    var responseBytes = Encoding.UTF8.GetBytes(responseString);
                    await ResponseWrite(context, responseBytes);

                    return;
                }

                var errorResponse = RpcResponse<object>.WithError(null, CommonErrors.AccessDenied);
                var errorResponseBytes = JsonSerializer.SerializeToUtf8Bytes(errorResponse, _jsonSerializerOptions);
                await ResponseWrite(context, errorResponseBytes);
            }
            catch (Exception exception)
            {
                var errorResponse = RpcResponse<object>.WithError(null, CommonErrors.Exception(exception));
                var errorResponseBytes = JsonSerializer.SerializeToUtf8Bytes(errorResponse, _jsonSerializerOptions);
                await ResponseWrite(context, errorResponseBytes);

                _logger.Error(exception, "");
            }
        }

        // TODO: make separate entity
        private bool ValidateAccess<TReq, TResp>(HttpContext context, IQueryProcessor<TReq, TResp> processor)
        {
            // TODO: return should be more specific than true/false
            // if processor is not secure
            if (!(processor is IQueryProcessorSecure queryProcessorSecure))
                return true;

            // if processor is secure, and user is not authenticated
            if (!(context.User?.Identity?.IsAuthenticated ?? false))
                return false;

            // if processor is secure, user is authenticated and authorisation is not required
            if (!queryProcessorSecure.Authorize)
                return true;
            
            // if processor is secure, user is authenticated and authorisation required, but no claims pointed
            // it's misconfiguration
            if (queryProcessorSecure.Authorize && (queryProcessorSecure.Claims == null || queryProcessorSecure.Claims.Length == 0))
                return false;
            
            // if processor is secure, user is authenticated and authorisation is passed
            if (queryProcessorSecure.Claims.Aggregate(
                true,
                (hasClaims, claim) =>
                    hasClaims && (context.User?.HasClaim(c => c.Type == claim.Type && c.Value == claim.Value) ?? false)
            ))
                return true;

            // else
            return false;
        }

        private async Task ResponseWrite(HttpContext context, byte[] responseBytes)
        {
            context.Response.Headers.Add("Accept-Ranges", "bytes");
            context.Response.Headers.Add("Cache-Control", "no-cache, must-revalidate");
            context.Response.Headers.Add("Content-Type", "application/json;charset=utf-8");
            context.Response.Headers.Add("Content-Length", responseBytes.Length.ToString());
            await context.Response.Body.WriteAsync(responseBytes);
        }
    }
}