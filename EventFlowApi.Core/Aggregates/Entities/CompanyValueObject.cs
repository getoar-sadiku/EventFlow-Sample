using EventFlow.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventFlowApi.Core.Aggregates.Entities
{
    public class CompanyValueObject : ValueObject
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public CompanyValueObject()
        {

        }

        public void Update(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
