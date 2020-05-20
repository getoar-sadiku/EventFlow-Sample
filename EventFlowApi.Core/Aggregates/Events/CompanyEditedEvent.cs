using EventFlow.Aggregates;
using EventFlow.EventStores;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Events
{
    [EventVersion("CompanyEdited", 1)]
    public class CompanyEditedEvent : AggregateEvent<CompanyAggregate, CompanyId>
    {
        public Company CompanyRecord { get; set; }

        public CompanyEditedEvent(Company companyRecord)
        {
            CompanyRecord = companyRecord;
        }
    }
}
