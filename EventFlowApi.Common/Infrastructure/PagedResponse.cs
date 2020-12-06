using System.Collections.Generic;

namespace EventFlowApi.Common.Infrastructure
{
    public class PagedResponse<T> : IPagedResponse<T> where T : class
    {
        public IList<T> Records { get; set; } = new List<T>();
        public long TotalCount { get; set; }
    }
}
