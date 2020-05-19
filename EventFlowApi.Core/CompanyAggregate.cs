using EventFlow.Aggregates;
using EventFlow.Exceptions;
using EventFlow.Snapshots;
using EventFlow.Snapshots.Strategies;
using EventFlowApi.Core.Aggregates.Entities;
using EventFlowApi.Core.Aggregates.Events;
using EventFlowApi.Core.Aggregates.Queries;
using EventFlowApi.Core.Aggregates.Snapshots;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowApi.Core
{
    [AggregateName("Company")]
    public class CompanyAggregate : SnapshotAggregateRoot<CompanyAggregate, CompanyId, CompanySnapshot>, IEmit<CompanyDomainErrorAfterFirstEvent>
    {
        private readonly IScopedContext _scopedContext;
        public const int SnapshotEveryVersion = 10;
        private readonly Dictionary<CompanyId, Company> _records = new Dictionary<CompanyId, Company>();
        public Dictionary<CompanyId, Company> Records => _records;

        public bool DomainErrorAfterFirstReceived { get; private set; }
        public IReadOnlyCollection<CompanySnapshotVersion> SnapshotVersions { get; private set; } = new CompanySnapshotVersion[] { };

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyAggregate"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public CompanyAggregate(CompanyId id, IScopedContext scopedContext) : base(id, SnapshotEveryFewVersionsStrategy.With(SnapshotEveryVersion))
        {
            _scopedContext = scopedContext;

            Register<CompanyAddedEvent>(e => _records.Add(id, e.CompanyRecord));
            Register<CompanyAddedEvent>(e => _records[id] = e.CompanyRecord);
            Register<CompanyDeletedEvent>(e => _records.Remove(id));
            Register<CompanySagaStartRequestedEvent>(e => {/* do nothing */});
            Register<CompanySagaCompleteRequestedEvent>(e => {/* do nothing */});
        }

        public void DomainErrorAfterFirst()
        {
            if (DomainErrorAfterFirstReceived)
            {
                throw DomainError.With("DomainErrorAfterFirst already received!");
            }

            Emit(new CompanyDomainErrorAfterFirstEvent());
        }

        public void AddRecord(Company record)
        {
            if (_records.Any(m => m.Value.Id == record.Id))
            {
                throw DomainError.With($"Company '{Id}' already has a record with ID '{record.Id}'");
            }

            Emit(new CompanyAddedEvent(record));
        }

        public void DeleteRecord(Company record)
        {
            Emit(new CompanyDeletedEvent(record));
        }
        public void EditRecord(Company record)
        {
            Emit(new CompanyEditedEvent(record));
        }

        public void RequestSagaStart()
        {
            Emit(new CompanySagaStartRequestedEvent());
        }

        public void RequestSagaComplete()
        {
            Emit(new CompanySagaCompleteRequestedEvent());
        }
        public void Apply(CompanyDomainErrorAfterFirstEvent aggregateEvent)
        {
            DomainErrorAfterFirstReceived = true;
        }

        protected override Task<CompanySnapshot> CreateSnapshotAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new CompanySnapshot(Records.Values,
                    Enumerable.Empty<CompanySnapshotVersion>()));
        }

        protected override Task LoadSnapshotAsync(CompanySnapshot snapshot, ISnapshotMetadata metadata, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        //protected override Task LoadSnapshotAsync(CompanySnapshot snapshot, ISnapshotMetadata metadata, CancellationToken cancellationToken)
        //{
        //    _records..AddRange(snapshot.CompanyAdded);
        //    SnapshotVersions = snapshot.PreviousVersions;

        //    return Task.FromResult(0);
        //}
    }
}
