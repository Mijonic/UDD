using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCompany.ElasticsearchModels
{
    public class ApplicantESModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Education { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string CoverLetterId { get; set; }
        public string CvId { get; set; }
        public string CoverLetterContent { get; set; }
        public string CvContent { get; set; }
    }
}
