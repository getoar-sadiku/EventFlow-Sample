using EventFlow.Entities;
using System;

namespace EventFlowApi.Core.Aggregates.Entities
{
    public class Company : Entity<CompanyId>
    {
        public string TenantId { get; }
        public string Name { get; }
        public string Address { get; }
        public Company(CompanyId id, string name, string address, string tenantId) : base(id)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));

            Name = name;
            Address = address;
            TenantId = tenantId;
        }
    }
}
