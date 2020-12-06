using EventFlow.Queries;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Queries.CompanyQueries
{
    public class CompanyGetByAddressQuery : IQuery<int>
    {
        public string Address { get; }
        public CompanyId Id { get; }
        public CompanyGetByAddressQuery(string address, CompanyId id)
        {
            Address = address;
            Id = id;
        }
    }
}
