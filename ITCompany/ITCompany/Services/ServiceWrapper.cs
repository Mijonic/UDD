using AutoMapper;
using BitMiracle.Docotic.Pdf;
using ITCompany.Dtos;
using ITCompany.ElasticsearchModels;
using ITCompany.Interfaces;
using ITCompany.Models;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ITCompany.Services
{
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly IApplicantService _applicantService;
        private readonly IMapper _mapper;
        private readonly IPDFService _pdfService;
        private HttpClient httpClient;

        private const string _openstreetmap = "https://nominatim.openstreetmap.org/";

        public ServiceWrapper(IApplicantService applicantService, IMapper mapper, IPDFService pdfService)
        {
            _applicantService = applicantService;
            _mapper = mapper;
            _pdfService = pdfService;
            httpClient = GetHttpClient();
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

            newApplicantES.GeoLocation = await GetGeoLocation(applicant.Street, applicant.City, applicant.Country);
            await _applicantService.SaveApplicantESAsync(newApplicantES);
            

            return _mapper.Map<ApplicantDto>(newApplicant);

        }

        private HttpClient GetHttpClient()
        {

            var http = new HttpClient
            {
                BaseAddress = new Uri(_openstreetmap),
                Timeout = TimeSpan.FromSeconds(30),
            };

            return http;
        }

        private async Task<GeoLocation> GetGeoLocation(string street, string city, string country)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"search?q={city}&limit=1&format=json&city={city}&street={street}&country={country}");
            request.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
     
            var locationCoordinates = JsonConvert.DeserializeObject<List<LocationCoordinates>>(content);
            double lat = double.Parse(locationCoordinates?[0].lat ?? locationCoordinates?[1].lat);
            double lon = double.Parse(locationCoordinates?[0].lon ?? locationCoordinates?[1].lon);

            return new GeoLocation(lat, lon);
        }

        private async Task<GeoLocation> GetGeoLocationForCity(string city)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"search?q={city}&limit=1&format=json&city={city}");
            request.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            var locationCoordinates = JsonConvert.DeserializeObject<List<LocationCoordinates>>(content);
            double lat = double.Parse(locationCoordinates?[0].lat ?? locationCoordinates?[1].lat);
            double lon = double.Parse(locationCoordinates?[0].lon ?? locationCoordinates?[1].lon);

            return new GeoLocation(lat, lon);
        }

        public async Task<List<SearchResultDto>> SearchApplicantsByCity(string city, double radius)
        {
            GeoLocation geoLocation = await GetGeoLocationForCity(city);
            return  await _applicantService.SearchApplicantsByCity(geoLocation.Latitude, geoLocation.Longitude, radius);
        }
    }
}
