using AutoMapper;
using ITCompany.Dtos;
using ITCompany.ElasticsearchModels;
using ITCompany.Interfaces;
using ITCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Services
{
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly IApplicantService _applicantService;
        private readonly IMapper _mapper;

        public ServiceWrapper(IApplicantService applicantService, IMapper mapper)
        {
            _applicantService = applicantService;
            _mapper = mapper;
        }

        public async Task<ApplicantDto> InsertApplicant(ApplicantDto applicant)
        {
            //add validation
            applicant.Id = Guid.NewGuid();

            ApplicantESModel newApplicantES = _mapper.Map<ApplicantESModel>(applicant);

            await _applicantService.SaveApplicantESAsync(newApplicantES);
            Applicant newApplicant = await _applicantService.SaveApplicantDbAsync(_mapper.Map<Applicant>(applicant));

            return _mapper.Map<ApplicantDto>(newApplicant);

        }

    }
}
