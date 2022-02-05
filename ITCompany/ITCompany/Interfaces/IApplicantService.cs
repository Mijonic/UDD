using ITCompany.ElasticsearchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Interfaces
{
    public interface IApplicantService
    {
        Task SaveSingleAsync(ApplicantESModel applicant);
      
    }
}
