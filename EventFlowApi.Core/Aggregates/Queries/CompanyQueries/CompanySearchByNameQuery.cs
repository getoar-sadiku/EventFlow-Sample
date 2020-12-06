using EventFlow.Queries;
using EventFlowApi.Common.Infrastructure;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Queries.CompanyQueries
{
    public class CompanySearchByNameQuery : IQuery<IPagedResponse<Company>>
    {
        public string Name { get; }

        public CompanySearchByNameQuery(string name)
        {
            Name = name;
        }
    }
}
