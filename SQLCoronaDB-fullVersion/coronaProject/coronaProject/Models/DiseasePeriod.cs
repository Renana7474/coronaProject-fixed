using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coronaProject.Models
{
    public class DiseasePeriod
    {
        public int id { get; set; }
        public int member_id { get; set; }
        public string detected_date { get; set; }
        public string recovery_date { get; set; }
    }
}
