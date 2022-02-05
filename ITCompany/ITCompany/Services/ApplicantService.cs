using ITCompany.Dtos;
using ITCompany.ElasticsearchModels;
using ITCompany.Infrastructure;
using ITCompany.Interfaces;
using ITCompany.Models;
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
        private readonly ITCompanyDbContext _context;

        public ApplicantService(IElasticClient client, ITCompanyDbContext context)
        {
            _client = client;
            _context = context;
        }

        public async Task<Applicant>  SaveApplicantDbAsync(Applicant applicant)
        {
            _context.Add(applicant);
            await _context.SaveChangesAsync();

            return applicant;
        }

        public async Task SaveApplicantESAsync(ApplicantESModel applicant)
        {
            await _client.IndexDocumentAsync(applicant);
        }


    }
}
