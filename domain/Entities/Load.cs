using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class Load
    {
        public string code { get; set; }
        public bool withCache { get; set; }
        public bool withResponse { get; set; }
        public string syncId { get; set; }
        public Configuration configuration { get; set; }
        public DataInput? dataInput { get; set; }
        public List<Entities2>? entities { get; set; }

    }
}
