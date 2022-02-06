using AutoMapper;
using ITCompany.Dtos;
using ITCompany.ElasticsearchModels;
using ITCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Applicant, ApplicantDto>()
               .ForMember(mem => mem.Education, op => op.MapFrom(o => o.Education))
               .ReverseMap();

            CreateMap<ApplicantESModel, ApplicantDto>().ReverseMap();

            CreateMap<ApplicantESModel, SearchResultDto>().ReverseMap();

        }
    }
}
