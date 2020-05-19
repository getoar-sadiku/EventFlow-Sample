using EventFlow.Commands;
using EventFlowApi.Core.Aggregates.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowApi.Core.Aggregates.Commands
{
    public class CompanyDeleteCommandHandler : CommandHandler<CompanyAggregate, CompanyId, CompanyDeleteCommand>
    {
        public override Task ExecuteAsync(CompanyAggregate aggregate, CompanyDeleteCommand command, CancellationToken cancellationToken)
        {
            aggregate.DeleteRecord(command.CompanyRecord);
            return Task.FromResult(0);
            }
    }
}
