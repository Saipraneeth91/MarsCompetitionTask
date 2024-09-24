using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Competition.Models
{

        public class EducationData  
        {
        public List<EducationInfo> EducationRecords { get; set; }
    }

        public class EducationInfo
        {
            public string University { get; set; }
            public string Country { get; set; }
            public string Title { get; set; }
            public string Degree { get; set; }
            public string GraduationYear { get; set; }
        }
    }



