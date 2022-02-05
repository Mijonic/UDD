using ITCompany.Dtos;
using ITCompany.ElasticsearchModels;
using ITCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Interfaces
{
    public interface IApplicantService
    {
        Task SaveApplicantESAsync(ApplicantESModel applicant);
        Task<Applicant> SaveApplicantDbAsync(Applicant applicant);

    }
}
