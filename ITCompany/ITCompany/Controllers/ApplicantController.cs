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

        public ApplicantController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicantDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NewApplication([FromBody] ApplicantDto applicant)
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
    }
}
