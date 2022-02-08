using AutoMapper;
using ITCompany.Dtos;
using ITCompany.ElasticsearchModels;
using ITCompany.Infrastructure;
using ITCompany.Interfaces;
using ITCompany.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IElasticClient _client;
        private readonly ITCompanyDbContext _context;
        private readonly IMapper _mapper;

        public ApplicantService(IElasticClient client, ITCompanyDbContext context, IMapper mapper)
        {
            _client = client;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Applicant>  SaveApplicantDbAsync(Applicant applicant)
        {
            _context.Add(applicant);
            await _context.SaveChangesAsync();

            return applicant;
        }

        public async Task SaveApplicantESAsync(ApplicantESModel applicant)
        {
            await _client.IndexDocumentAsync(applicant);
        }

        public async Task<List<SearchResultDto>> SearchApplicantsByNameAndSurname(string name, string surname)
        {
            var searchResponse =  await _client.SearchAsync<ApplicantESModel>(s => s
                            .Query(q => q
                                .Bool(b => b
                                    .Must(mu => mu
                                        .Match(m => m
                                            .Field(f => f.Name)
                                            .Query(name)
                                        ), mu => mu
                                        .Match(m => m
                                            .Field(f => f.Surname)
                                            .Query(surname)
                                        )
                                    )
                                    
                                )
                            )
                        );

           return MapResults(searchResponse.Documents.ToList());
        }


        public async Task<List<SearchResultDto>> SearchApplicantsByEducation(string education)
        {

            var searchResponse = await  _client.SearchAsync<ApplicantESModel>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Education)
                        .Query(education)
                    )
                )
            );

            return MapResults(searchResponse.Documents.ToList());
        }

        public async Task<List<SearchResultDto>> SearchApplicantsByCvContent(string content)
        {

            var searchResponse = await _client.SearchAsync<ApplicantESModel>(s => s
               .Query(q => q
                   .Match(m => m
                       .Field(f => f.CvContent)
                       .Query(content)
                   )
               ).Highlight(h => h
                    .PreTags("<mark>")
                    .PostTags("</mark>")
                    .Fields(fs => fs
                        .Field(f => f.CvContent)
                    ))
            );

            return MapResults(searchResponse.Documents.ToList());
        }

        public async Task<List<SearchResultDto>> SearchApplicantsByAllFields(string text)
        {

            var searchResponse = await _client.SearchAsync<ApplicantESModel>(s => s
               .Query(q => q
                   .MultiMatch(m => m
                        .Query(text)
                        .Type(TextQueryType.Phrase)
                        .Fields(x => x
                            .Field(f => f.Name)
                            .Field(f => f.Surname)
                            .Field(f => f.City)
                            .Field(f => f.CoverLetterContent)
                            .Field(f => f.CvContent)
                            .Field(f => f.Description)
                            .Field(f => f.Education))
                           
                    
                   )
               )
            );

            return MapResults(searchResponse.Documents.ToList());
        }

        public async Task<List<SearchResultDto>> SearchApplicantsByCity(double lat, double lon, double radius)
        {
            var searchResponse = await _client.SearchAsync<ApplicantESModel>(s => s
                .Query(q => q
                        .GeoDistance(g => g
                            .Boost(1.1)
                            .Name("named_query")
                            .Field(p => p.GeoLocation)
                            .DistanceType(GeoDistanceType.Arc)
                            .Location(lat, lon)
                            .Distance(radius, DistanceUnit.Kilometers)
                            .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                        )
                        ));



            return MapResults(searchResponse.Documents.ToList());

        }


        private List<SearchResultDto> MapResults(List<ApplicantESModel> results)
        {
            List<SearchResultDto> resultsDto = new List<SearchResultDto>();

            foreach (var res in results)
            {
                var resultDto = _mapper.Map<SearchResultDto>(res);
                resultsDto.Add(resultDto);
            }

            return resultsDto;
        }

    }

   
}
