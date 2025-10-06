using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace domain.Entities
{
    public class Department
    {
        public string name { get; set; }
        public List<Store> stores { get; set; }
    }
}
