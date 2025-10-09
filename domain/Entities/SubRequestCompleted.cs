using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class SubRequestCompleted
    {
        public string id { get; set; }
        public string status { get; set; }
        public DateTime completedAt { get; set; }
        public List<string> references { get; set; }     
        public Recap progress { get; set; }
    }
}
