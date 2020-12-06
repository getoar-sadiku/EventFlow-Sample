using EventFlow.Elasticsearch.ReadStores;
using EventFlow.Elasticsearch.ValueObjects;
using EventFlow.Queries;
using EventFlowApi.Core.Aggregates.Entities;
using EventFlowApi.Core.Aggregates.Queries.CompanyQueries;
using EventFlowApi.ElasticSearch.ReadModels;
using Nest;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowApi.ElasticSearch.QueryHandler
{
    public class ESCompanyGetQueryHandler : IQueryHandler<CompanyGetQuery, Company>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IReadModelDescriptionProvider _readModelDescriptionProvider;

        public ESCompanyGetQueryHandler(IElasticClient elasticClient, IReadModelDescriptionProvider readModelDescriptionProvider)
        {
            _elasticClient = elasticClient;
            _readModelDescriptionProvider = readModelDescriptionProvider;
        }

        public async Task<Company> ExecuteQueryAsync(CompanyGetQuery query, CancellationToken cancellationToken)
        {
            ReadModelDescription readModelDescription = _readModelDescriptionProvider.GetReadModelDescription<CompanyReadModel>();
            string indexName = readModelDescription.IndexName.Value;

            await _elasticClient.FlushAsync(indexName,
                    d => d.RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound)), cancellationToken)
                    .ConfigureAwait(false);

            await _elasticClient.RefreshAsync(indexName,
                    d => d.RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound)), cancellationToken)
                    .ConfigureAwait(false);

            IGetResponse<CompanyReadModel> response = await _elasticClient.GetAsync<CompanyReadModel>(
                query.CompanyId.Value.ToString(),
                d => d.RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound))
                .Index(indexName), cancellationToken)
                .ConfigureAwait(false);

            return response.Source.ToCompany();
        }
    }
}
