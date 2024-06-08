using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitirmeEntity.Models
{
    public class Hospitals:BaseEntity
    {
        public string HospitalName { get; set; }

        public string HospitalAddress { get; set; }
    }
}
