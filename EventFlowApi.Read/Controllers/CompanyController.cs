﻿using EventFlow;
using EventFlow.Queries;
using EventFlowApi.Core.Aggregates.Entities;
using EventFlowApi.Core.Aggregates.Queries.CompanyQueries;
using EventFlowApi.Read.Dto.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowApi.Read.Controllers
{
    /// <summary>Company api, collection of all related operations</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : BaseController
    {
        /// <summary>employee api cons.</summary>
        /// <param name="commandBus"></param>
        /// <param name="queryProcessor"></param>
        public CompanyController(ICommandBus commandBus, IQueryProcessor queryProcessor) : base(commandBus, queryProcessor)
        {
        }

        /// <summary>
        ///  Company information retrieval.
        /// </summary>
        /// <param name="employeeId">The string company Id</param>
        /// <returns>return company</returns>
        [HttpGet]
        public async Task<CompanyResponse> Get(string companyId)
        {
            var readModel = await QueryProcessor.ProcessAsync(
                       new CompanyGetQuery(new CompanyId(companyId)), CancellationToken.None)
                       .ConfigureAwait(false);

            var response = new CompanyResponse { TenantId = readModel.TenantId, Id = readModel.Id.GetGuid().ToString(), Name = readModel.Name, Address = readModel.Address, CreatedDate = readModel.CreatedDate, ModifiedDate = readModel.ModifiedDate, IsDeleted = readModel.IsDeleted };
            return response;
        }
        [HttpGet("address")]
        public async Task<int> GetByAddress(string address, Guid companyId)
        {
            var count = 0;
            if (!string.IsNullOrEmpty(address))
                count = await QueryProcessor.ProcessAsync(
                       new CompanyGetByAddressQuery((address), CompanyId.With(companyId)), CancellationToken.None)
                       .ConfigureAwait(false);
            return count;
        }

        [HttpGet("search")]
        public async Task<IList<CompanyResponse>> GetCompanyByName(string name)
        {
            var readModel = await QueryProcessor.ProcessAsync(new CompanySearchByNameQuery(name), CancellationToken.None)
                .ConfigureAwait(false);

            var response = new List<CompanyResponse>();
            foreach (var item in readModel.Records)
            {
                response.Select(x => new CompanyResponse
                {
                    Name = item.Name,
                    Address = item.Address,
                }).ToList();
            }
            return response;
        }
    }
}