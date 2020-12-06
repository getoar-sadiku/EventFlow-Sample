using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlowApi.Core;
using EventFlowApi.Core.Aggregates.Entities;
using EventFlowApi.Core.Aggregates.Events;
using Nest;
using System;

namespace EventFlowApi.ElasticSearch.ReadModels
{
    public class CompanyReadModel : IReadModel,
        IAmReadModelFor<CompanyAggregate, CompanyId, CompanyAddedEvent>,
        IAmReadModelFor<CompanyAggregate, CompanyId, CompanyEditedEvent>,
        IAmReadModelFor<CompanyAggregate, CompanyId, CompanyDeletedEvent>
    {
        //[Keyword(Index = true)]
        public string TenantId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? IsDeleted { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<CompanyAggregate, CompanyId, CompanyAddedEvent> domainEvent)
        {
            TenantId = domainEvent.Metadata["tenant_Id"];
            Id = domainEvent.AggregateEvent.CompanyRecord.Id.GetGuid();
            Name = domainEvent.AggregateEvent.CompanyRecord.Name;
            Address = domainEvent.AggregateEvent.CompanyRecord.Address;
            CreatedDate = DateTime.Now;
        }

        public void Apply(IReadModelContext context, IDomainEvent<CompanyAggregate, CompanyId, CompanyEditedEvent> domainEvent)
        {
            TenantId = domainEvent.Metadata["tenant_Id"];
            Id = domainEvent.AggregateEvent.CompanyRecord.Id.GetGuid();
            Name = domainEvent.AggregateEvent.CompanyRecord.Name;
            Address = domainEvent.AggregateEvent.CompanyRecord.Address;
            ModifiedDate = DateTime.Now;
        }

        public void Apply(IReadModelContext context, IDomainEvent<CompanyAggregate, CompanyId, CompanyDeletedEvent> domainEvent)
        {
            TenantId = domainEvent.Metadata["tenant_Id"];
            Id = domainEvent.AggregateEvent.CompanyRecord.Id.GetGuid();
            Name = domainEvent.AggregateEvent.CompanyRecord.Name;
            Address = domainEvent.AggregateEvent.CompanyRecord.Address;
            IsDeleted = DateTime.Now;
        }

        public Company ToCompany()
        {
            return new Company(CompanyId.With(Id), Name, Address, TenantId, CreatedDate, ModifiedDate, IsDeleted);
        }
    }
}
