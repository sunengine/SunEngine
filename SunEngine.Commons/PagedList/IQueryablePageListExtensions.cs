using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;

namespace SunEngine.Commons.PagedList
{
    public static class IQueryablePageListExtensions
    {

        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 0)
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
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
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }
    }
}
