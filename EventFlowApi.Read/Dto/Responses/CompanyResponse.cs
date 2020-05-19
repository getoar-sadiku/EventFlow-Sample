namespace EventFlowApi.Read.Dto.Responses
{
    public class CompanyResponse
    {
        public string TenantId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        //public IEnumerable<TransactionResponse> Transactions { get; set; }
    }
}
