using ITCompany.ElasticsearchModels;
using ITCompany.Interfaces;
using ITCompany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        IApplicantService service;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IApplicantService service)
        {
            _logger = logger;
            this.service = service;
        }


        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await service.SaveSingleAsync(new ApplicantESModel()
            {
                City = "Novi Sad",
                CoverLetterId = Guid.NewGuid(),
                CvId = Guid.NewGuid(),
                Description = "neki opis",
                Education = Enums.EducationLevel.DoctoralStudies.ToString(),
                Id = Guid.NewGuid(),
                Name = "nikola",
                Surname = "mijonic",
                CoverLetterContent = "ovo je propratno pismo neko kao",
                CvContent = "ovo je cv koji sadrzi svasta nesto"

            });
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
