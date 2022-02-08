using ITCompany.Dtos;
using ITCompany.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Controllers
{
    [Route("api/applicants")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IPDFService _pdfService;
        private readonly IApplicantService _applicantService;

        public ApplicantController(IServiceWrapper serviceWrapper, IPDFService pdfService, IApplicantService applicationService)
        {
            _serviceWrapper = serviceWrapper;
            _pdfService = pdfService;
            _applicantService = applicationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicantDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NewApplication([FromForm] ApplicantDto applicant)
        {
            try
            {
                ApplicantDto insertedApplicant = await _serviceWrapper.InsertApplicant(applicant);

                return CreatedAtAction(nameof(NewApplication), new { id = insertedApplicant.Id }, insertedApplicant);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


       
        [HttpGet("search/name/{name}/surname/{surname}")]
        public async Task<IActionResult> SearchByNameAndSurname(string name, string surname)
        {
            try
            {
                List<SearchResultDto> searchResults = await _applicantService.SearchApplicantsByNameAndSurname(name, surname);

                return Ok(searchResults);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("search/education/{education}")]
        public async Task<IActionResult> SearchByEducation(string education)
        {
            try
            {
                List<SearchResultDto> searchResults = await _applicantService.SearchApplicantsByEducation(education);

                return Ok(searchResults);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }


        [HttpGet("search/content/{content}")]
        public async Task<IActionResult> SearchByContent(string content)
        {
            try
            {
                List<SearchResultDto> searchResults = await _applicantService.SearchApplicantsByCvContent(content);

                return Ok(searchResults);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("search/all-fields/{text}")]
        public async Task<IActionResult> SearchApplicantsByAllFields(string text)
        {
            try
            {
                List<SearchResultDto> searchResults = await _applicantService.SearchApplicantsByAllFields(text);

                return Ok(searchResults);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("search/city/{city}/radius/{radius}")]
        public async Task<IActionResult> SearchApplicantsByCity(string city, double radius)
        {
            try
            {
                List<SearchResultDto> searchResults = await _serviceWrapper.SearchApplicantsByCity(city, radius);

                return Ok(searchResults);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }



        //[HttpPost("/attachments/{applicantId}")]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> AttachFileAsync(IFormFile file, Guid applicantId)
        //{
        //    try
        //    {
        //        await _pdfService.AttachFileToApplicant(file, applicantId, true);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }

        //}
    }
}
