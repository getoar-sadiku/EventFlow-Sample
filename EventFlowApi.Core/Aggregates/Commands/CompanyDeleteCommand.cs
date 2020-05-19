using EventFlow.Commands;
using EventFlowApi.Core.Aggregates.Entities;
using JetBrains.Annotations;

namespace EventFlowApi.Core.Aggregates.Commands
{
    public class CompanyDeleteCommand : Command<CompanyAggregate, CompanyId>
    {
        public Company CompanyRecord { get; }

        public CompanyDeleteCommand([NotNull]CompanyId aggregateId, Company companyRecord)
            : base(aggregateId)
        {
            CompanyRecord = companyRecord;
        }
    }
}
