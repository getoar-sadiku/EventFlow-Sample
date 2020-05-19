using EventFlow.Commands;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Commands
{
    public class CompanyAddCommand : Command<CompanyAggregate, CompanyId>
    {
        public Company CompanyRecord { get; }

        public CompanyAddCommand(CompanyId aggregateId, Company companyRecord)
            : base(aggregateId)
        {
            CompanyRecord = companyRecord;
        }
    }
}
