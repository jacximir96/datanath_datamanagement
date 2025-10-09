using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class Template
    {
        public string? id { get; set; }
        public string? Client { get; set; }
        public List<Origin>? origins { get; set; }
        public string? serviceDesk { get; set; }
        public Store? stores { get; set; }
        public string? entity { get; set; }
        public string? operation { get; set; }
        public List<Entity>? entities { get; set; }
        public string? transformationKey { get; set; }
        public Target? target { get; set; }
        public string? status { get; set; }
        public Recap? progress { get; set; }
        public DateTime? completedAt { get; set; }
        public List<string>? references { get; set; }
        

    }
}
