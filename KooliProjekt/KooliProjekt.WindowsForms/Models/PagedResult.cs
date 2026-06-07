using System.Collections.Generic;

namespace KooliProjekt.WindowsForms.Models
{
    public class PagedResult<T>
    {
        public List<T> Results { get; set; } = new();

        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}