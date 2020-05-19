using EventFlow.Snapshots;
using EventFlowApi.Core.Aggregates.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EventFlowApi.Core.Aggregates.Snapshots
{
    [SnapshotVersion("company", 3)]
    public class CompanySnapshot : ISnapshot
    {
        public IReadOnlyCollection<Company> CompanyAdded { get; }
        public IReadOnlyCollection<CompanySnapshotVersion> PreviousVersions { get; }
        public CompanySnapshot(IEnumerable<Company> companyAdded, IEnumerable<CompanySnapshotVersion> previousVersions)
        {
            CompanyAdded = (companyAdded ?? Enumerable.Empty<Company>()).ToList();
            PreviousVersions = (previousVersions ?? Enumerable.Empty<CompanySnapshotVersion>()).ToList();
        }
    }
}
