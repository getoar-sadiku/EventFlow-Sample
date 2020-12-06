using EventFlow.Elasticsearch.ReadStores;
using EventFlow.Elasticsearch.ValueObjects;
using EventFlow.Queries;
using EventFlowApi.Common.Infrastructure;
using EventFlowApi.Core.Aggregates.Entities;
using EventFlowApi.Core.Aggregates.Queries.CompanyQueries;
using EventFlowApi.ElasticSearch.ReadModels;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowApi.ElasticSearch.QueryHandler
{
    public class ESCompanySearchByNameQueryHandler : IQueryHandler<CompanySearchByNameQuery, IPagedResponse<Company>>
    {
        private readonly IReadModelDescriptionProvider _readModelDescriptionProvider;
        private readonly IElasticClient _elasticClient;

        public ESCompanySearchByNameQueryHandler(IElasticClient elasticClient, IReadModelDescriptionProvider readModelDescriptionProvider)
        {
            _readModelDescriptionProvider = readModelDescriptionProvider;
            _elasticClient = elasticClient;
        }
        public async Task<IPagedResponse<Company>> ExecuteQueryAsync(CompanySearchByNameQuery query, CancellationToken cancellationToken)
        {
            ReadModelDescription readModelDescription = _readModelDescriptionProvider.GetReadModelDescription<CompanyReadModel>();
            string indexName = readModelDescription.IndexName.Value;

            await _elasticClient.FlushAsync(indexName,
                    d => d.RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound)), cancellationToken)
                    .ConfigureAwait(false);

            await _elasticClient.RefreshAsync(indexName,
                    d => d.RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound)), cancellationToken)
                    .ConfigureAwait(false);

            var querySearch = !string.IsNullOrEmpty(query.Name)
                ? new WildcardQuery()
                {
                    Field = Infer.Field<CompanyReadModel>(f => f.Name),
                    Value = query.Name.ToLower() + "*",
                }
                : null;
            var searchRequest = new SearchRequest(indexName)
            {
                Query = new BoolQuery()
                {
                    Must = new QueryContainer[]
                    {
                        querySearch
                    },
                    //MustNot = new QueryContainer[]
                    //{
                    //    new ExistsQuery()
                    //    {
                    //        Field = Infer.Field<CompanyReadModel>(f => f.IsDeleted)
                    //    }
                    //}
                },
                From = 1,
                Size = 10,
            };

            var companyRecords = await _elasticClient.SearchAsync<CompanyReadModel>(searchRequest, cancellationToken);
            IPagedResponse<Company> response = new PagedResponse<Company>()
            {
                Records = (companyRecords?.Documents?.Count > 0) ? companyRecords.Documents.Select(collection => collection.ToCompany()).ToList() : new List<Company>(),
                TotalCount = (companyRecords?.Documents?.Count > 0) ? companyRecords.Total : 0
            };

            return response;
        }
    }
}
