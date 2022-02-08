using ITCompany.ElasticsearchModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Extensions
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .EnableDebugMode()
                .DefaultIndex(defaultIndex);

            AddDefaultMappings(settings);


            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {

            settings.DefaultMappingFor<ApplicantESModel>(m => m.IdProperty(x => x.Id));
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
           index => index.Map<ApplicantESModel>(x => 
                x.Properties(ps => ps
                
                    .Text(s => s
                        .Name(n => n.Name)
                        .Analyzer("serbian"))
                    .Text(s => s
                        .Name(n => n.Surname)
                        .Analyzer("serbian"))
                    .Text(s => s
                        .Name(n => n.Education)
                        .Analyzer("serbian"))
                    .Text(s => s
                        .Name(n => n.Education)
                        .Analyzer("serbian"))
                    .Text(s => s
                        .Name(n => n.City)
                        .Analyzer("serbian"))
                    .Text(s => s
                        .Name(n => n.Description)
                        .Analyzer("serbian"))
                     .Text(s => s
                        .Name(n => n.CoverLetterId))
                     .Text(s => s
                        .Name(n => n.CvId))
                     .Text(s => s
                        .Name(n => n.CoverLetterContent)
                        .Analyzer("serbian"))
                    .Text(s => s
                        .Name(n => n.CvContent)
                        .Analyzer("serbian"))
                    .GeoPoint(t => t
                           .Name(n => n.GeoLocation)))


           ));
        }
    }
}
