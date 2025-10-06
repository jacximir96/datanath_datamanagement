using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class Extract
    {
        public string code { get; set; }
        public string syncId { get; set; }
        public bool withResponse { get; set; }
        public bool withCache { get; set; }
        public Configuration configuration { get; set; }

    }
}
