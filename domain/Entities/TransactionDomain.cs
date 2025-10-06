using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class TransactionDomain
    {
        public long Id { get; set; }
        public DateTime TransactionDate {get;set;}
        public decimal Amounth { get; set; }
    
    }
}
