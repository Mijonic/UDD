﻿using ITCompany.Dtos;
using ITCompany.ElasticsearchModels;
using ITCompany.Enums;
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
        Task<List<SearchResultDto>> SearchApplicantsByNameAndSurname(string name, string surname);
        Task<List<SearchResultDto>> SearchApplicantsByEducation(string education);
        Task<List<SearchResultDto>> SearchApplicantsByCvContent(string content);
        Task<List<SearchResultDto>> SearchApplicantsByAllFields(string text);
        Task<List<SearchResultDto>> SearchApplicantsByCity(double lat, double lon, double radius);
        Task<List<SearchResultDto>> SearchApplicantsWithOperator(string name, Operator operator1, string surname, Operator operator2, string education, Operator operator3, string cvContent);


    }
}
