using ITCompany.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.Models
{
    public class Applicant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public EducationLevel Education { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public Guid CoverLetterId { get; set; }
        public Guid CvId { get; set; }
    }
}
