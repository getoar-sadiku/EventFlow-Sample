using EventFlow.Aggregates;
using EventFlow.EventStores;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Events
{
    [EventVersion("CompanyDeleted", 1)]
    public class CompanyDeletedEvent : AggregateEvent<CompanyAggregate, CompanyId>
    {
        public Company CompanyRecord { get; }

        public CompanyDeletedEvent(Company companyRecord)
        {
            CompanyRecord = companyRecord;
        }
    }
}
