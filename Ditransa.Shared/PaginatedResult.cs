using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ditransa.Shared
{
    public class PaginatedResult<T> : Result<T>
    {
        public PaginatedResult(List<T> data)
        {
            Data = data;
        }

        public PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int pageNumber = 1, int pageSize = 10)
        {
            Data = data;
            CurrentPage = pageNumber;
            Succeeded = succeeded;
            Messages = messages;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }

        public new List<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public string Description {
            get {
                if(TotalCount == 0)
                    return $"0 items";
                var from = ((CurrentPage - 1) * PageSize ) + 1;
                var to = CurrentPage == TotalPages ? TotalCount: CurrentPage * PageSize;
                return $"{from} to {to} of {TotalCount} items";
            }
        }
        public List<string> PagesList
        {
            get { 
                var list = new List<string>();
                if (TotalPages <= 5 || (TotalPages> 5 && CurrentPage <= 4)){
                    for (int i = 1; i <= 5 && i <= TotalPages; i++)
                    {
                        list.Add(i.ToString());
                    }
                    if (TotalPages > 5)
                    {
                        list.Add("...");
                        list.Add(TotalPages.ToString());
                    }
                }
                else if(CurrentPage >= 4 )
                {
                    
                    list.Add("1");
                    if ((CurrentPage - 2) > 1)
                    {
                        if ((CurrentPage - 2) > 2 )
                        {
                            list.Add("...");
                        }
                        for (int i = CurrentPage - 2; i <= CurrentPage + 2 && i <= TotalPages; i++) {
                            list.Add(i.ToString());
                        }

                        if ((CurrentPage + 2) < TotalPages) {
                            if ((CurrentPage + 3) < TotalPages)
                            {
                                list.Add("...");
                            }
                            list.Add(TotalPages.ToString());
                        }
                    }
                    
                }
                return list;
            }
        }

        public static PaginatedResult<T> Create(List<T> data, int count, int pageNumber, int pageSize)
        {
            return new PaginatedResult<T>(true, data, null, count, pageNumber, pageSize);
        }
    }
}
