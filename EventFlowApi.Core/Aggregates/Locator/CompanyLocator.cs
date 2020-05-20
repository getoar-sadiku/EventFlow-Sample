using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlowApi.Core.Aggregates.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventFlowApi.Core.Aggregates.Locator
{
    public class CompanyLocator : IReadModelLocator
    {
        public IEnumerable<string> GetReadModelIds(IDomainEvent domainEvent)
        {
            IAggregateEvent aggregateEvent = domainEvent.GetAggregateEvent();

            switch (aggregateEvent)
            {
                case CompanyAddedEvent companyRecordAddedEvent:
                    yield return companyRecordAddedEvent.CompanyRecord.Id.Value;
                    break;
                case CompanyEditedEvent companyEditedEvent:
                    yield return companyEditedEvent.CompanyRecord.Id.Value;
                    break;
                case CompanyDeletedEvent companyDeletedEvent:
                    yield return companyDeletedEvent.CompanyRecord.Id.Value;
                    break;
            }
        }
    }
}
