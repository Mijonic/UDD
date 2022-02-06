using AutoMapper;
using BitMiracle.Docotic.Pdf;
using ITCompany.Dtos;
using ITCompany.ElasticsearchModels;
using ITCompany.Interfaces;
using ITCompany.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Services
{
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly IApplicantService _applicantService;
        private readonly IMapper _mapper;
        private readonly IPDFService _pdfService;

        public ServiceWrapper(IApplicantService applicantService, IMapper mapper, IPDFService pdfService)
        {
            _applicantService = applicantService;
            _mapper = mapper;
            _pdfService = pdfService;
        }

        public async Task<ApplicantDto> InsertApplicant(ApplicantDto applicant)
        {
            //add validation
            applicant.Id = Guid.NewGuid();

            Applicant newApplicant = await _applicantService.SaveApplicantDbAsync(_mapper.Map<Applicant>(applicant));

            ApplicantESModel newApplicantES = _mapper.Map<ApplicantESModel>(applicant);

          
            string cvPath = await _pdfService.AttachFileToApplicant(applicant.CvFile, newApplicant.Id, true);
            string coverLetterPath = await _pdfService.AttachFileToApplicant(applicant.CoverLetterFile, newApplicant.Id, false);

            using (var cvPdf = new PdfDocument(cvPath))
            {
                string documentText = cvPdf.GetText();
                newApplicantES.CvContent = documentText;
            }

            using (var coverLetterPdf = new PdfDocument(coverLetterPath))
            {
                string documentTextCoverLetter = coverLetterPdf.GetText();
                newApplicantES.CoverLetterContent = documentTextCoverLetter;
            }

            await _applicantService.SaveApplicantESAsync(newApplicantES);
            

            return _mapper.Map<ApplicantDto>(newApplicant);

        }

    }
}
