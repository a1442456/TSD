using System;
using System.Threading.Tasks;
using Quartz;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Cen.Wms.Host.Sync
{
    public class SimpleInjectorJobDecorator: IJob
    {
        private readonly Container _container;
        private readonly Func<IJob> _decorateeFactory;

        public SimpleInjectorJobDecorator(Container container, Func<IJob> decorateeFactory)
        {
            _container = container;
            _decorateeFactory = decorateeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await using var scope = AsyncScopedLifestyle.BeginScope(_container);
            
            var job = _decorateeFactory();
            await job.Execute(context);
            (job as IDisposable)?.Dispose();
        }
    }
}