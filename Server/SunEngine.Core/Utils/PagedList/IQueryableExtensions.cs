using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;

namespace SunEngine.Core.Utils.PagedList
{
	public static class IQueryableExtensions
	{
		public static Task<IPagedList<TResult>> GetPagedListAsync<TResult, TEntity>(this IQueryable<TEntity> query,
			Expression<Func<TEntity, TResult>> selector,
			Expression<Func<TEntity, bool>> predicate = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			int pageIndex = 0,
			int pageSize = 20) where TEntity : class
		{
			IQueryable<TEntity> query1 = query;


			if (predicate != null)
			{
				query1 = query1.Where(predicate);
			}

			if (orderBy != null)
			{
				return orderBy(query1).Select(selector).ToPagedListAsync(pageIndex, pageSize, 1);
			}

			return query1.Select(selector).ToPagedListAsync(pageIndex, pageSize, 1);
		}

		public static Task<IPagedList<TResult>> GetPagedListMaxAsync<TResult, TEntity>(this IQueryable<TEntity> query,
			Expression<Func<TEntity, TResult>> selector,
			Expression<Func<TEntity, bool>> predicate = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			int pageIndex = 0,
			int pageSize = 20,
			int maxPages = 0)
			where TResult : class
		{
			IQueryable<TEntity> query1 = query;


			if (predicate != null)
			{
				query1 = query1.Where(predicate);
			}

			if (orderBy != null)
			{
				query1 = orderBy(query1);
			}

			if (maxPages != 0)
			{
				query1 = query1.Take(maxPages * pageSize);
			}


			return query1.Select(selector).ToPagedListAsync(pageIndex, pageSize, 1);
		}


		public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex,
			int pageSize, int indexFrom = 0)
		{
			if (indexFrom > pageIndex)
			{
				throw new ArgumentException(
					$"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
			}

			var count = await source.CountAsync();
			var items = await source.Skip((pageIndex - indexFrom) * pageSize)
				.Take(pageSize).ToListAsync();

			var pagedList = new PagedList<T>()
			{
				PageIndex = pageIndex,
				PageSize = pageSize,
				IndexFrom = indexFrom,
				TotalCount = count,
				Items = items,
				TotalPages = (int) Math.Ceiling(count / (double) pageSize)
			};

			return pagedList;
		}
	}

	/*public static class TableExtensions
	{
	    public static Task<IPagedList<TResult>> GetPagedListAsync<TResult, TEntity>(this ITable<TEntity> table, Expression<Func<TEntity, TResult>> selector,
	        Expression<Func<TEntity, bool>> predicate = null,
	        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
	        Expression<Func<TEntity,object>> loadWith = null,
	        int pageIndex = 0,
	        int pageSize = 20)  where TEntity : class
	    {
	        IQueryable<TEntity> query;
	        
	        if (loadWith != null)
	        {
	            query = table.LoadWith(loadWith);
	        }
	        else
	        {
	            query = table;
	        }

	        if (predicate != null)
	        {
	            query = query.Where(predicate);
	        }

	        if (orderBy != null)
	        {
	            return orderBy(query).Select(selector).ToPagedListAsync(pageIndex, pageSize, 1);
	        }
	        else
	        {
	            return query.Select(selector).ToPagedListAsync(pageIndex, pageSize, 1);
	        }
	    }
	    
	    public static Task<IPagedList<TResult>> GetPagedListMaxAsync<TResult, TEntity>(this ITable<TEntity> table, 
	        Expression<Func<TEntity, TResult>> selector,
	        Expression<Func<TEntity, bool>> predicate = null,
	        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
	        Expression<Func<TEntity,object>> loadWith = null,
	        int pageIndex = 0,
	        int pageSize = 20,
	        int maxPages = 0)
	        where TResult : class
	    {
	        
	        IQueryable<TEntity> query;
	        if (loadWith != null)
	        {
	            query = table.LoadWith(loadWith);
	        }
	        else
	        {
	            query = table;
	        }

	        if (predicate != null)
	        {
	            query = query.Where(predicate);
	        }

	        if (orderBy != null)
	        {
	            query = orderBy(query);
	        }

	        if (maxPages != 0)
	        {
	            query = query.Take(maxPages * pageSize);
	        }
	        
	        
	        return query.Select(selector).ToPagedListAsync(pageIndex, pageSize, 1);
	    }
	}*/
}