using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlowApi.Core;
using EventFlowApi.Core.Aggregates.Entities;
using EventFlowApi.Core.Aggregates.Events;
using Nest;

namespace EventFlowApi.ElasticSearch.ReadModels
{
    public class CompanyReadModel : IReadModel, IAmReadModelFor<CompanyAggregate, CompanyId, CompanyAddedEvent>
    {
        [Keyword(Index = true)]
        public string TenantId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<CompanyAggregate, CompanyId, CompanyAddedEvent> domainEvent)
        {
            TenantId = domainEvent.Metadata["tenant_Id"];
            Id = domainEvent.AggregateEvent.CompanyRecord.Id.Value;
            Name = domainEvent.AggregateEvent.CompanyRecord.Name;
            Address = domainEvent.AggregateEvent.CompanyRecord.Address;
        }

        public Company ToCompany()
        {
            return new Company(CompanyId.With(Id), Name, Address, TenantId);
        }
    }
}
