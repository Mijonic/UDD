
using ITCompany.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Interfaces
{
    public interface IServiceWrapper
    {
        Task<ApplicantDto> InsertApplicant(ApplicantDto applicant);
        Task<List<SearchResultDto>> SearchApplicantsByCity(string city, double radius);
    }
}
