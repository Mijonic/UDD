using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Interfaces
{
    public interface IPDFService
    {
        Task<string> AttachFileToApplicant(IFormFile formFile, Guid applicantId, bool isCV);
    }
}
