using domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace infrastructure.Factory
{
    public class SqlServerDataBase : IConnectioDataBase
    {
        private readonly IConfiguration _configuration;

        public SqlServerDataBase(IConfiguration configuration) 
        { 
            _configuration = configuration; 
        }

        public object GetObjectDataBase()
        {           
            return null;
        }
    }
}
