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

        public ApplicantController(IServiceWrapper serviceWrapper, IPDFService pdfService)
        {
            _serviceWrapper = serviceWrapper;
            _pdfService = pdfService;
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

                await _pdfService.AttachFileToApplicant(applicant.CvFile, insertedApplicant.Id, true);
                await _pdfService.AttachFileToApplicant(applicant.CoverLetterFile, insertedApplicant.Id, false);

                return CreatedAtAction(nameof(NewApplication), new { id = insertedApplicant.Id }, insertedApplicant);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
