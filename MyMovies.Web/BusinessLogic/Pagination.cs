using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMovies.Web.Models;

namespace MyMovies.Web.BusinessLogic
{
    public class Pagination
    {
        private const int ITEMS_PER_PAGE = 50;

        public IEnumerable<T> TakePaginationModel<T>(IEnumerable<T> list, PaginationModel pagination) where T : class
        {
            list = (pagination.CurrentPage > 0 ? list.Skip((pagination.CurrentPage - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage) : list);
            return list;
        }

        public PaginationModel GetPaginationModel(int page, int itemCount, int itemsPerPage = 0, string pageName = "")
        {
            if (itemsPerPage == 0)
                itemsPerPage = ITEMS_PER_PAGE;

            return new PaginationModel
            {
                PageName = pageName,
                CurrentPage = page == 0 ? 1 : page,
                TotalPages = Convert.ToInt32(Math.Ceiling((decimal)itemCount / itemsPerPage)),
                TotalItems = itemCount,
                DefaultItemsPerPage = itemsPerPage,
                ItemsPerPage = itemsPerPage,
            };
        }
    }
}