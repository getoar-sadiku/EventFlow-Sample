using EventFlow.Aggregates;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Events
{
    public class CompanyAddedEvent : AggregateEvent<CompanyAggregate, CompanyId>
    {
        public Company CompanyRecord { get; }

        public CompanyAddedEvent(Company companyRecord)
        {
            CompanyRecord = companyRecord;
        }
    }
}
