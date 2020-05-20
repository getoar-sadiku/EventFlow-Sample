using EventFlow.Aggregates;
using EventFlow.EventStores;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Events
{
    [EventVersion("CompanyDeleted", 1)]
    public class CompanyDeletedEvent : AggregateEvent<CompanyAggregate, CompanyId>
    {
        public Company CompanyRecord { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyDeletedEvent"/> class.
        /// </summary>
        /// <param name="companyRecord">The Company record.</param>
        public CompanyDeletedEvent(Company companyRecord)
        {
            CompanyRecord = companyRecord;
        }
    }
}
