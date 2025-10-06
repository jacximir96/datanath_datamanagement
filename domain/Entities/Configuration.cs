using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class Configuration
    {
        public Connection connection { get; set; }
        public List<Entity> entities { get; set; }
    }
}
