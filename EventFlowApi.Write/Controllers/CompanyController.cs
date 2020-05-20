using EventFlow;
using EventFlow.Queries;
using EventFlowApi.Core.Aggregates.Commands;
using EventFlowApi.Core.Aggregates.Entities;
using EventFlowApi.Core.Aggregates.Queries.CompanyQueries;
using EventFlowApi.Write.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowApi.Write.Controllers
{
    /// <summary>Company api, collection of all related operations</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : BaseController
    {
        /// <summary>Company api cons.</summary>
        /// <param name="commandBus"></param>
        /// <param name="queryProcessor"></param>
        public CompanyController(ICommandBus commandBus, IQueryProcessor queryProcessor) : base(commandBus, queryProcessor)
        {
        }

        /// <summary>
        /// Add a new company event.
        /// </summary>
        /// <param name="request">create employee request</param>
        /// <returns>employeeid</returns>
        [HttpPost]
        public async Task<CompanyId> Post(CompanyRequest request)
        {
            var id = Guid.NewGuid().ToString();
            var companyId = new CompanyId("company-" + id);
            var companyRecord = new Company(companyId, request.Name, request.Address, "", DateTime.Now, null, null);
            var companyCommand = new CompanyAddCommand(companyId, companyRecord);

            await CommandBus.PublishAsync(companyCommand, CancellationToken.None).ConfigureAwait(false);

            return companyId;

        }
        /// <summary>
        /// Delete Company Async
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> Delete(string companyId)
        {
            var company = await QueryProcessor.ProcessAsync(
                new CompanyGetQuery(new CompanyId(companyId)), CancellationToken.None)
                .ConfigureAwait(false);
            if (company == null) return false;

            var cmd = new CompanyDeleteCommand(company.Id, company);
            await CommandBus.PublishAsync(cmd, CancellationToken.None).ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// Update Company Async
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> Put(string companyId, CompanyRequest request)
        {
            var oldCompany = await QueryProcessor.ProcessAsync(
                new CompanyGetQuery(new CompanyId(companyId)), CancellationToken.None)
                .ConfigureAwait(false);
            var newCompanyRecord = new Company(oldCompany.Id, request.Name, request.Address, "", oldCompany.CreatedDate, oldCompany.ModifiedDate, oldCompany.IsDeleted);
            var cmd = new CompanyEditCommand(new CompanyId(companyId), newCompanyRecord);
            await CommandBus.PublishAsync(cmd, CancellationToken.None).ConfigureAwait(false);

            return true;
        }
    }
}