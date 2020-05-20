using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.AspNetCore.Middlewares;
using EventFlow.Autofac.Extensions;
using EventFlow.Configuration;
using EventFlow.Elasticsearch.Extensions;
using EventFlow.Extensions;
using EventFlowApi.Core.Aggregates.Entities;
using EventFlowApi.Core.Aggregates.Locator;
using EventFlowApi.Core.Aggregates.Queries;
using EventFlowApi.ElasticSearch.QueryHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using EventFlowApi.ElasticSearch.ReadModels;
using Swashbuckle.AspNetCore.Swagger;
using EventFlowApi.Common.Infrastructure;

namespace EventFlowApi.Read
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IEventFlowOptions Options { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = "Eventflow Demo -Read API", Version = "v1" });
                x.OperationFilter<SwaggerAuthorizationHeaderParameterOperationFilter>();
                x.DescribeAllEnumsAsStrings();
            });

            string elasticSearchUrl = Environment.GetEnvironmentVariable("ELASTICSEARCHURL");
            ContainerBuilder containerBuilder = new ContainerBuilder();
            Uri node = new Uri(elasticSearchUrl);
            ConnectionSettings settings = new ConnectionSettings(node);

            settings.DisableDirectStreaming();

            ElasticClient elasticClient = new ElasticClient(settings);

            Options = EventFlowOptions.New
              .UseAutofacContainerBuilder(containerBuilder)
              .AddDefaults(typeof(Company).Assembly)
              .ConfigureElasticsearch(() => elasticClient)
              .RegisterServices(sr => sr.Register<IScopedContext, ScopedContext>(Lifetime.Scoped))
              .RegisterServices(sr => sr.RegisterType(typeof(CompanyLocator)))
              .UseElasticsearchReadModel<CompanyReadModel, CompanyLocator>()
              .AddQueryHandlers(typeof(ESCompanyGetQueryHandler))
              .Configure(c => c.IsAsynchronousSubscribersEnabled = true)
              .AddAspNetCoreMetadataProviders();

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Read API Version 1");

            });

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseHttpsRedirection();
            app.UseMiddleware<CommandPublishMiddleware>();
            app.UseMvcWithDefaultRoute();
        }
    }
}
