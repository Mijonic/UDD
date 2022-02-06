using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Dtos
{
    public class ApplicantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Education { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public IFormFile CvFile { get; set; }
        public IFormFile CoverLetterFile { get; set; }
        public Guid CoverLetterId { get; set; }
        public Guid CvId { get; set; }
    }
}
