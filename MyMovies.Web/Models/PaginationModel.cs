
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMovies.Web.Models
{
    public class PaginationModel
    {
        protected const int DisplayPageRange = 2;

        //Pagination Related Properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string PageName { get; set; }
        public int ItemsPerPage { get; set; }
        public int DefaultItemsPerPage { get; set; }
        public string PagingText { get; set; }

        //Filters
        public string Search { get; set; }
        public string Category { get; set; }
        public string Star { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }

        public virtual int StartDisplayPage
        {
            get
            {
                var start = CurrentPage - DisplayPageRange;
                if (start < 1) start = 1;

                return start;
            }
        }

        public virtual int EndDisplayPage
        {
            get
            {
                var end = StartDisplayPage + (DisplayPageRange * 2);
                if (end > TotalPages) end = TotalPages;

                return end;
            }
        }


        public virtual IEnumerable<int> DisplayPages
        {
            get
            {
                return Enumerable.Range(StartDisplayPage, EndDisplayPage - StartDisplayPage + 1);
            }
        }

        public string ListTitle
        {
            get
            {
                    if (!String.IsNullOrEmpty(Search)) return String.Format("Search: {0}", Search);
                    if (!String.IsNullOrEmpty(Category)) return String.Format("Category: {0}", Category);
                    if (!String.IsNullOrEmpty(Star)) return String.Format("Actor: {0}", Star);
                    if (!String.IsNullOrEmpty(Director)) return String.Format("Director: {0}", Director);
                    if (!String.IsNullOrEmpty(Writer)) return String.Format("Writer: {0}", Writer);

                return "";
            }
        }

        public string PageLink
        {
            get
            {
                var link = "?Page={0}";
                if (!String.IsNullOrEmpty(Category)) link += String.Format("&Category={0}", Category);
                if (!String.IsNullOrEmpty(Search)) link += String.Format("&Search={0}", Search);
                if (!String.IsNullOrEmpty(Star)) link += String.Format("&Star={0}", Star);
                if (!String.IsNullOrEmpty(Director)) link += String.Format("&Director={0}", Director);
                if (!String.IsNullOrEmpty(Writer)) link += String.Format("&Writer={0}", Writer);

                return link;
            }
        }
    }
}