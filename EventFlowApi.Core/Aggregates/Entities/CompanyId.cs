using EventFlow.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventFlowApi.Core.Aggregates.Entities
{
    public class CompanyId : Identity<CompanyId>
    {
        public CompanyId(string value) : base(value)
        {
        }
    }
}
