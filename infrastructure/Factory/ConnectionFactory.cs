using domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Factory
{
    public class ConnectionFactory: IConnectionFactory
    {
        private readonly IConfiguration _configuration; 
        public ConnectionFactory(IConfiguration configuration) 
        { 
            _configuration = configuration;
        }  
        
        public IConnectioDataBase CreateConnection(string dataBase) 
        {
            IConnectioDataBase connectioDataBase = null;    

            switch (dataBase) 
            {
                case "sqlserver":

                    connectioDataBase=new SqlServerDataBase(_configuration);
                    
                    break;

                case "cosmosdb":

                    connectioDataBase=new CosmosDataBase(_configuration);

                    break;
            }

            return connectioDataBase;

        }
    }
}
