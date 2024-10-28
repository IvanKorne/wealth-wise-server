using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Helpers
{
    public class StockQuery
    {
        public string? Symbol {get;set;} = null;
        public string? CompanyName {get;set;} = null;
        public string? SortBy {get;set;} = null;
        public bool IsDescending {get;set;} = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}