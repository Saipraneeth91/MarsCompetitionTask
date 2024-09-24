using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Competition.Models
{
    public class CertificationData
    {
        public List<CertificationInfo> CertificationRecords { get; set; }
    }
    public class CertificationInfo
    {
        public string Certificate { get; set; }
        public string From { get; set; }
        public string Year { get; set; }
        
    }
}
