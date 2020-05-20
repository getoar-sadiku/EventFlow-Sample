using EventFlow.Entities;
using System;

namespace EventFlowApi.Core.Aggregates.Entities
{
    public class Company : Entity<CompanyId>
    {
        public string TenantId { get; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? IsDeleted { get; set; }
        public Company(CompanyId id, string name, string address, string tenantId, DateTime createdDate, DateTime? modifiedDate, DateTime? isDeleted) : base(id)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));

            Name = name;
            Address = address;
            TenantId = tenantId;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
            IsDeleted = isDeleted;
        }
    }
}
