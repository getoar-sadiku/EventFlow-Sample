using EventFlow.Commands;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Commands
{
    public class CompanyEditCommand : Command<CompanyAggregate, CompanyId>
    {
        public Company CompanyRecord { get; }

        public CompanyEditCommand(CompanyId aggregateId, Company companyRecord)
            : base(aggregateId)
        {
            CompanyRecord = companyRecord;
        }
    }
}
