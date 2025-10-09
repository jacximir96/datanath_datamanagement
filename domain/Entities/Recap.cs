using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class Recap
    {
        public int totalSubRequests { get; set; }
        public int completed { get; set; }
        public int failed { get; set; }
    }
}
