using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShaRide.Application.Pagination
{
    public class PagedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int ItemsInPage { get; set; }
        public int TotalItems { get; set; }
        public List<T> Items { get; set; } = new List<T>();
        public PagedList(List<T> items, int count, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            ItemsInPage = items.Count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;
            Items.AddRange(items);
        }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public int FirstItem
        {
            get
            {
                if (ItemsInPage == 0)
                    return 0;
                return (PageSize * (CurrentPage - 1)) + 1;
            }
        }

        public int LastItem
        {
            get
            {
                if (ItemsInPage == 0)
                    return 0;
                return (PageSize * (CurrentPage - 1)) + ItemsInPage;
            }
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageIndex, pageSize);
        }

        public static PagedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
