using System;

namespace EventFlowApi.Read.Dto.Responses
{
    public class CompanyResponse
    {
        public string TenantId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? IsDeleted { get; set; }
    }
}
