using EventFlow.Elasticsearch.ReadStores;
using EventFlow.Elasticsearch.ValueObjects;
using EventFlow.Queries;
using EventFlowApi.Core.Aggregates.Queries.CompanyQueries;
using EventFlowApi.ElasticSearch.ReadModels;
using Nest;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowApi.ElasticSearch.QueryHandler
{
    public class EsCompanyGetByAddressQueryHandler : IQueryHandler<CompanyGetByAddressQuery, int>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IReadModelDescriptionProvider _readModelDescriptionProvider;

        public EsCompanyGetByAddressQueryHandler(IElasticClient elasticClient, IReadModelDescriptionProvider readModelDescriptionProvider)
        {
            _elasticClient = elasticClient;
            _readModelDescriptionProvider = readModelDescriptionProvider;
        }
        public async Task<int> ExecuteQueryAsync(CompanyGetByAddressQuery query, CancellationToken cancellationToken)
        {
            ReadModelDescription readModelDescription = _readModelDescriptionProvider.GetReadModelDescription<CompanyReadModel>();
            string indexName = readModelDescription.IndexName.Value;

            await _elasticClient.FlushAsync(indexName,
                    d => d.RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound)), cancellationToken)
                .ConfigureAwait(false);

            await _elasticClient.RefreshAsync(indexName,
                    d => d.RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound)), cancellationToken)
                .ConfigureAwait(false);

            ISearchResponse<CompanyReadModel> searchResponse;
            if (query.Id != null)
            {
                searchResponse = await _elasticClient.SearchAsync<CompanyReadModel>(
                       d => d
                           .RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound))
                           .Index(indexName)
                           .Query(q => q.Match(m => m.Field(f => f.Address).Query(query.Address).MinimumShouldMatch("100%"))
                                       && !q.Match(m => m.Field(f => f.Id).Query(query.Id.Value.ToString()))), cancellationToken);
            }
            else
            {
                searchResponse = await _elasticClient.SearchAsync<CompanyReadModel>(
              d => d
                  .RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound))
                  .Index(indexName)
                  .Query(q => q.Match(m => m.Field(f => f.Address).Query(query.Address).MinimumShouldMatch("100%"))), cancellationToken);

            }
            var count = searchResponse?.Documents?.Count;

            return count ?? 0;
        }
    }
}
