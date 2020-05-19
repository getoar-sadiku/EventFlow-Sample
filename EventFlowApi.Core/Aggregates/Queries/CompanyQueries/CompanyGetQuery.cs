using EventFlow.Queries;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Queries.CompanyQueries
{
    public class CompanyGetQuery : IQuery<Company>
    {
        public CompanyId CompanyId { get; }

        public CompanyGetQuery(CompanyId companyId)
        {
            CompanyId = companyId;
        }
    }
}
