using AutoMapper;
using ITCompany.Infrastructure;
using ITCompany.Interfaces;
using ITCompany.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Services
{
    public class PDFService : IPDFService
    {
        private readonly ITCompanyDbContext _context;

        public PDFService(ITCompanyDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task AttachFileToApplicant(IFormFile formFile, Guid applicantId, bool isCV)
        {
            Applicant applicant = _context.Applicants.Find(applicantId);

            if (applicant == null)
                throw new Exception($"applicant with id {applicantId} does not exist.");

            string folderName = isCV ? "CV" : "CoverLetter";
            string filePath = Path.Combine(@$"Attachments/{folderName}/{applicant.Name}_{applicant.Surname}-{applicant.Id}/", formFile.FileName);
           
            new FileInfo(filePath).Directory?.Create();
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);

            }

            string id = $"{applicant.Name}_{applicant.Surname}-{applicant.Id}/{formFile.FileName}"; ;
            if (isCV)
                applicant.CvId = id;
            else
                applicant.CoverLetterId = id;

            await _context.SaveChangesAsync();

        }
    }
}
