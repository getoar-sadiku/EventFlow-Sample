using EventFlow.Commands;
using EventFlowApi.Core.Aggregates.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowApi.Core.Aggregates.Commands
{
    public class CompanyAddCommandHandler : CommandHandler<CompanyAggregate, CompanyId, CompanyAddCommand>
    {
        public override Task ExecuteAsync(CompanyAggregate aggregate, CompanyAddCommand command, CancellationToken cancellationToken)
        {
            aggregate.AddRecord(command.CompanyRecord);

            return Task.FromResult(0);
        }
    }
}
