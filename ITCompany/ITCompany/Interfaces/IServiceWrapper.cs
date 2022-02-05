
using ITCompany.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Interfaces
{
    public interface IServiceWrapper
    {
        Task<ApplicantDto> InsertApplicant(ApplicantDto applicant);
    }
}
