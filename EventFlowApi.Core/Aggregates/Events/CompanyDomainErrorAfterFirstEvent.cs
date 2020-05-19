using EventFlow.Aggregates;
using EventFlow.EventStores;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Events
{
    [EventVersion("CompanyDomainErrorAfterFirst", 1)]
    public class CompanyDomainErrorAfterFirstEvent : AggregateEvent<CompanyAggregate, CompanyId>
    {
    }
}
