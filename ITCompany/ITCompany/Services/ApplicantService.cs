using ITCompany.ElasticsearchModels;
using ITCompany.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IElasticClient _client;

        public ApplicantService(IElasticClient client)
        {
            _client = client;
        }

        public async Task SaveSingleAsync(ApplicantESModel applicant)
        {
            await _client.IndexDocumentAsync(applicant);
        }
    }
}
