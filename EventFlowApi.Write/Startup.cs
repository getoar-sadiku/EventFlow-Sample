using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.AspNetCore.Middlewares;
using EventFlow.Autofac.Extensions;
using EventFlow.Configuration;
using EventFlow.Elasticsearch.Extensions;
using EventFlow.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using EventFlowApi.Core.Aggregates.Entities;
using EventFlowApi.Core.Aggregates.Locator;
using EventFlowApi.Core.Aggregates.Queries;
using EventFlowApi.ElasticSearch.ReadModels;
using Nest;
using EventFlowApi.Common.Infrastructure;
using EventFlowApi.ElasticSearch.QueryHandler;

namespace EventFlowApi.Write
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = "Eventflow Demo - Write API", Version = "v1" });
                x.OperationFilter<SwaggerAuthorizationHeaderParameterOperationFilter>();
                x.DescribeAllEnumsAsStrings();
            });

            ContainerBuilder containerBuilder = new ContainerBuilder();
            //string rabbitMqConnection = Environment.GetEnvironmentVariable("RABBITMQCONNECTION");
            string elasticSearchUrl = Environment.GetEnvironmentVariable("ELASTICSEARCHURL");
            Uri node = new Uri(elasticSearchUrl);
            ConnectionSettings settings = new ConnectionSettings(node);

            settings.DisableDirectStreaming();

            ElasticClient elasticClient = new ElasticClient(settings);
            EventFlowOptions.New
                .UseAutofacContainerBuilder(containerBuilder)
                .AddDefaults(typeof(Company).Assembly)
                .ConfigureElasticsearch(() => elasticClient)
                //.ConfigureEventStore()
                //.PublishToRabbitMq(
                //    RabbitMqConfiguration.With(new Uri(rabbitMqConnection),
                //        true, 5, "eventflow"))
                .RegisterServices(sr => sr.Register<IScopedContext, ScopedContext>(Lifetime.Scoped))
                .RegisterServices(sr => sr.RegisterType(typeof(CompanyLocator)))
                .RegisterServices(sr => sr.Register<IScopedContext, ScopedContext>(Lifetime.Scoped))
                .RegisterServices(sr => sr.RegisterType(typeof(CompanyLocator)))
                .UseElasticsearchReadModel<CompanyReadModel, CompanyLocator>()
                .AddQueryHandlers(typeof(ESCompanyGetQueryHandler))

                .AddAspNetCoreMetadataProviders();

            containerBuilder.Populate(services);

            return new AutofacServiceProvider(containerBuilder.Build());
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
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "API Version 1");

            });

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseHttpsRedirection();
            app.UseMiddleware<CommandPublishMiddleware>();
            app.UseMvcWithDefaultRoute();
        }
    }
}
