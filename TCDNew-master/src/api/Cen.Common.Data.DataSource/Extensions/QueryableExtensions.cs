using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.DataSource.Infrastructure;
using Cen.Common.Data.DataSource.Infrastructure.Implementation.Expressions.Filtering;
using Cen.Common.Data.DataSource.Infrastructure.Implementation.Sorting;

namespace Cen.Common.Data.DataSource.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using DataSource.Infrastructure;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides extension methods to process DataSourceRequest.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies paging, sorting, filtering and grouping using the information from the DataSourceRequest object.
        /// If the collection is already paged, the method returns an empty result.
        /// </summary>
        /// <param name="enumerable">An instance of <see cref="IEnumerable" />.</param>
        /// <param name="request">An instance of <see cref="DataSourceRequest" />.</param>
        /// <returns>
        /// A <see cref="DataSourceResult" /> object, which contains the processed data after
        /// paging, sorting, filtering and grouping are applied.
        /// </returns>
        public static DataSourceResult ToDataSourceResult(this IEnumerable enumerable, DataSourceRequest request)
        {
            return enumerable.AsQueryable().CreateDataSourceResult<object, object>(request, null);
        }

        /// <summary>
        /// Applies paging, sorting, filtering and grouping using the information from the DataSourceRequest object.
        /// If the collection is already paged, the method returns an empty result.
        /// </summary>
        /// <param name="enumerable">An instance of <see cref="IEnumerable" />.</param>
        /// <param name="request">An instance of <see cref="DataSourceRequest" />.</param>
        /// <returns>
        /// A Task of <see cref="DataSourceResult" /> object, which contains the processed data
        /// after paging, sorting, filtering and grouping are applied.
        /// It can be called with the "await" keyword for asynchronous operation.
        /// </returns>
        public static Task<DataSourceResult> ToDataSourceResultAsync(this IEnumerable enumerable, DataSourceRequest request)
        {
            return CreateDataSourceResultAsync(() => QueryableExtensions.ToDataSourceResult(enumerable, request));
        }

        /// <summary>
        /// Applies paging, sorting, filtering and grouping using the information from the DataSourceRequest object.
        /// If the collection is already paged, the method returns an empty result.
        /// </summary>
        /// <param name="queryable">An instance of <see cref="IQueryable" />.</param>
        /// <param name="request">An instance of <see cref="DataSourceRequest" />.</param>
        /// <returns>
        /// A <see cref="DataSourceResult" /> object, which contains the processed data after paging, sorting, filtering and grouping are applied.
        /// </returns>
        public static DataSourceResult ToDataSourceResult(this IQueryable queryable, DataSourceRequest request)
        {
            return queryable.CreateDataSourceResult<object, object>(request, null);
        }

        /// <summary>
        /// Applies paging, sorting, filtering and grouping using the information from the DataSourceRequest object.
        /// If the collection is already paged, the method returns an empty result.
        /// </summary>
        /// <param name="queryable">An instance of <see cref="IQueryable" />.</param>
        /// <param name="request">An instance of <see cref="DataSourceRequest" />.</param>
        /// <returns>
        /// A Task of <see cref="DataSourceResult" /> object, which contains the processed data
        /// after paging, sorting, filtering and grouping are applied.
        /// It can be called with the "await" keyword for asynchronous operation.
        /// </returns>
        public static Task<DataSourceResult> ToDataSourceResultAsync(this IQueryable queryable, DataSourceRequest request)
        {
            return CreateDataSourceResultAsync(() => QueryableExtensions.ToDataSourceResult(queryable, request));
        }

        public static DataSourceResult ToDataSourceResult<TModel, TResult>(this IEnumerable<TModel> enumerable, DataSourceRequest request, Func<TModel, TResult> selector)
        {
            return enumerable.AsQueryable().CreateDataSourceResult(request, selector);
        }

        public static Task<DataSourceResult> ToDataSourceResultAsync<TModel, TResult>(
            this IEnumerable<TModel> enumerable, DataSourceRequest request, Func<TModel, TResult> selector)
        {
            return CreateDataSourceResultAsync(() => QueryableExtensions.ToDataSourceResult(enumerable, request, selector));
        }

        public static DataSourceResult ToDataSourceResult<TModel, TResult>(this IQueryable<TModel> enumerable, DataSourceRequest request, Func<TModel, TResult> selector)
        {
            return enumerable.CreateDataSourceResult(request, selector);
        }

        public static Task<DataSourceResult> ToDataSourceResultAsync<TModel, TResult>
            (this IQueryable<TModel> queryable, DataSourceRequest request, Func<TModel, TResult> selector)
        {
            return CreateDataSourceResultAsync(() => QueryableExtensions.ToDataSourceResult(queryable, request, selector));
        }

        private static DataSourceResult CreateDataSourceResult<TModel, TResult>(this IQueryable queryable, DataSourceRequest request, Func<TModel, TResult> selector)
        {
            var result = new DataSourceResult();

            var data = queryable;

            var filters = new List<IFilterDescriptor>();

            if (request.Filters != null)
            {
                filters.AddRange(request.Filters);
            }

            if (filters.Any())
            {
                data = data.Where(filters);
            }

            var sort = new List<SortDescriptor>();

            if (request.Sorts != null)
            {
                sort.AddRange(request.Sorts);
            }

            var temporarySortDescriptors = new List<SortDescriptor>();

            result.Total = data.Count();

            if (!sort.Any() && queryable.Provider.IsEntityFrameworkProvider())
            {
                // The Entity Framework provider demands OrderBy before calling Skip.
                SortDescriptor sortDescriptor = new SortDescriptor
                {
                    Member = queryable.ElementType.FirstSortableProperty()
                };
                sort.Add(sortDescriptor);
                temporarySortDescriptors.Add(sortDescriptor);
            }

            if (sort.Any())
            {
                data = data.Sort(sort);
            }

            var notPagedData = data;

            data = data.Page(request.Page - 1, request.PageSize);

            result.Items = data.Execute(selector);

            temporarySortDescriptors.Each(sortDescriptor => sort.Remove(sortDescriptor));

            return result;
        }

        private static Task<DataSourceResult> CreateDataSourceResultAsync(Func<DataSourceResult> expression)
        {
            return Task.Run(expression);
        }

        private static IQueryable CallQueryableMethod(this IQueryable source, string methodName, LambdaExpression selector)
        {
            IQueryable query = source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { source.ElementType, selector.Body.Type },
                    source.Expression,
                    Expression.Quote(selector)));

            return query;
        }

        /// <summary>
        /// Sorts the elements of a sequence using the specified sort descriptors.
        /// </summary>
        /// <param name="source">A sequence of values to sort.</param>
        /// <param name="sortDescriptors">The sort descriptors used for sorting.</param>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are sorted according to a <paramref name="sortDescriptors"/>.
        /// </returns>
        public static IQueryable Sort(this IQueryable source, IEnumerable<SortDescriptor> sortDescriptors)
        {
            var builder = new SortDescriptorCollectionExpressionBuilder(source, sortDescriptors);
            return builder.Sort();
        }

        /// <summary>
        /// Pages through the elements of a sequence until the specified
        /// <paramref name="pageIndex"/> using <paramref name="pageSize"/>.
        /// </summary>
        /// <param name="source">A sequence of values to page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are at the specified <paramref name="pageIndex"/>.
        /// </returns>
        public static IQueryable Page(this IQueryable source, int pageIndex, int pageSize)
        {
            IQueryable query = source;

            query = query.Skip(pageIndex * pageSize);

            if (pageSize > 0)
            {
                query = query.Take(pageSize);
            }

            return query;
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are the result of invoking a
        /// projection selector on each element of <paramref name="source" />.
        /// </returns>
        /// <param name="source"> A sequence of values to project. </param>
        /// <param name="selector"> A projection function to apply to each element. </param>
        public static IQueryable Select(this IQueryable source, LambdaExpression selector)
        {
            return source.CallQueryableMethod("Select", selector);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <param name="source"> An <see cref="IQueryable" /> whose elements to group.</param>
        /// <param name="keySelector"> A function to extract the key for each element.</param>
        /// <returns>
        /// An <see cref="IQueryable"/> with <see cref="IGrouping{TKey,TElement}"/> items,
        /// whose elements contains a sequence of objects and a key.
        /// </returns>
        public static IQueryable GroupBy(this IQueryable source, LambdaExpression keySelector)
        {
            return source.CallQueryableMethod("GroupBy", keySelector);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are sorted according to a key.
        /// </returns>
        /// <param name="source">
        /// A sequence of values to order.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from an element.
        /// </param>
        public static IQueryable OrderBy(this IQueryable source, LambdaExpression keySelector)
        {
            return source.CallQueryableMethod("OrderBy", keySelector);
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are sorted in descending order according to a key.
        /// </returns>
        /// <param name="source">
        /// A sequence of values to order.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from an element.
        /// </param>
        public static IQueryable OrderByDescending(this IQueryable source, LambdaExpression keySelector)
        {
            return source.CallQueryableMethod("OrderByDescending", keySelector);
        }

        /// <summary>
        /// Calls <see cref="OrderBy(System.Linq.IQueryable,System.Linq.Expressions.LambdaExpression)"/>
        /// or <see cref="OrderByDescending"/> depending on the <paramref name="sortDirection"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are sorted according to a key.
        /// </returns>
        public static IQueryable OrderBy(this IQueryable source, LambdaExpression keySelector, ListSortDirection? sortDirection)
        {
            if (sortDirection.HasValue)
            {
                if (sortDirection.Value == ListSortDirection.Ascending)
                {
                    return source.OrderBy(keySelector);
                }

                return source.OrderByDescending(keySelector);
            }

            return source;
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains elements from the input sequence
        /// that satisfy the condition specified by <paramref name="predicate" />.
        /// </returns>
        /// <param name="source"> An <see cref="IQueryable" /> to filter.</param>
        /// <param name="predicate"> A function to test each element for a condition.</param>
        public static IQueryable Where(this IQueryable source, Expression predicate)
        {
            return source.Provider.CreateQuery(
               Expression.Call(
                   typeof(Queryable),
                   "Where",
                   new[] { source.ElementType },
                   source.Expression,
                   Expression.Quote(predicate)));
        }

        /// <summary>
        /// Filters a sequence of values based on a collection of <see cref="IFilterDescriptor"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="filterDescriptors">The filter descriptors.</param>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains elements from the input sequence
        /// that satisfy the conditions specified by each filter descriptor in <paramref name="filterDescriptors" />.
        /// </returns>
        public static IQueryable Where(this IQueryable source, IEnumerable<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                var parameterExpression = Expression.Parameter(source.ElementType, "item");

                var expressionBuilder = new FilterDescriptorCollectionExpressionBuilder(parameterExpression, filterDescriptors);
                expressionBuilder.Options.LiftMemberAccessToNull = source.Provider.IsLinqToObjectsProvider();
                var predicate = expressionBuilder.CreateFilterExpression();
                return source.Where(predicate);
            }

            return source;
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains the specified number
        /// of elements from the start of <paramref name="source" />.
        /// </returns>
        /// <param name="source"> The sequence to return elements from.</param>
        /// <param name="count"> The number of elements to return. </param>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is null. </exception>
        public static IQueryable Take(this IQueryable source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Take",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant(count)));
        }

        /// <summary>
        /// Bypasses a specified number of elements in a sequence
        /// and then returns the remaining elements.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains elements that occur
        /// after the specified index in the input sequence.
        /// </returns>
        /// <param name="source">
        /// An <see cref="IQueryable" /> to return elements from.
        /// </param>
        /// <param name="count">
        /// The number of elements to skip before returning the remaining elements.
        /// </param>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null.</exception>
        public static IQueryable Skip(this IQueryable source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Skip",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant(count)));
        }

        /// <summary> Returns the number of elements in a sequence.</summary>
        /// <returns> The number of elements in the input sequence.</returns>
        /// <param name="source">
        /// The <see cref="IQueryable" /> that contains the elements to be counted.
        /// </param>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null.</exception>
        public static int Count(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.Execute<int>(
                Expression.Call(
                    typeof(Queryable), "Count",
                    new Type[] { source.ElementType }, source.Expression));
        }

        /// <summary> Returns the element at a specified index in a sequence.</summary>
        /// <returns> The element at the specified position in <paramref name="source" />.</returns>
        /// <param name="source"> An <see cref="IQueryable" /> to return an element from.</param>
        /// <param name="index"> The zero-based index of the element to retrieve.</param>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero.</exception>
        public static object ElementAt(this IQueryable source, int index)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (index < 0) throw new ArgumentOutOfRangeException("index");

            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable),
                    "ElementAt",
                    new Type[] { source.ElementType },
                    source.Expression,
                    Expression.Constant(index)));
        }

        /// <summary>
        /// Produces the set union of two sequences by using the default equality comparer.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains the elements from both input sequences, excluding duplicates.
        /// </returns>
        /// <param name="source">
        /// An <see cref="IQueryable" /> whose distinct elements form the first set for the union.
        /// </param>
        /// <param name="second">
        /// An <see cref="IQueryable" /> whose distinct elements form the first set for the union.
        /// </param>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null.</exception>
        public static IQueryable Union(this IQueryable source, IQueryable second)
        {
            IQueryable query = source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "Union",
                    new[] { source.ElementType },
                    source.Expression,
                    second.Expression));

            return query;
        }

        private static IEnumerable Execute<TModel, TResult>(this IQueryable source, Func<TModel, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");

            var type = source.ElementType;


            if (selector != null)
            {
                var list = new List<TResult>();

                foreach (TModel item in source)
                {
                    list.Add(selector(item));
                }

                return list;
            }
            else
            {
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

                foreach (var item in source)
                {
                    list.Add(item);
                }

                return list;
            }
        }
    }
}
