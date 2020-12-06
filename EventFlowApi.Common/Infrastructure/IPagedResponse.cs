using System.Collections.Generic;

namespace EventFlowApi.Common.Infrastructure
{
    public interface IPagedResponse<T> where T : class
    {
        IList<T> Records { get; set; }
        long TotalCount { get; set; }
    }
}
