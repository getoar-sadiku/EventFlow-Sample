using EventFlow.Commands;
using EventFlowApi.Core.Aggregates.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowApi.Core.Aggregates.Commands
{
    public class CompanyEditCommandHandler : CommandHandler<CompanyAggregate, CompanyId, CompanyEditCommand>
    {
        public override Task ExecuteAsync(CompanyAggregate aggregate, CompanyEditCommand command, CancellationToken cancellationToken)
        {
            aggregate.EditRecord(command.CompanyRecord);

            return Task.FromResult(0);
        }
    }
}
