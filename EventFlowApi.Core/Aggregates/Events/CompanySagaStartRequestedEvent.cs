using EventFlow.Aggregates;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Events
{
    public class CompanySagaStartRequestedEvent : AggregateEvent<CompanyAggregate, CompanyId>
    {
    }
}
