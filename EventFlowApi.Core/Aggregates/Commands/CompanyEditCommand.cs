using EventFlow.Commands;
using EventFlowApi.Core.Aggregates.Entities;

namespace EventFlowApi.Core.Aggregates.Commands
{
    public class CompanyEditCommand : Command<CompanyAggregate, CompanyId>
    {
        public Company CompanyRecord { get; }
        //public string Name { get; }
        //public string Address { get; }

        public CompanyEditCommand(CompanyId aggregateId, Company companyRecord/*, string name, string address*/)
            : base(aggregateId)
        {
            CompanyRecord = companyRecord;
            //Name = name;
            //Address = address;
        }
    }
}
