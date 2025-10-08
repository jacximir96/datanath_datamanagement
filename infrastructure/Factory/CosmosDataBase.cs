using domain.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Factory
{
    public class CosmosDataBase : IConnectioDataBase
    {
        private readonly IConfiguration _configuration;
       

        public CosmosDataBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public object GetObjectDataBase()
        {
          
            CosmosClient client = new CosmosClient(_configuration.GetSection("endpoint").Value, _configuration.GetSection("key").Value, new CosmosClientOptions
            {

                HttpClientFactory = () => new HttpClient(new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }),
                ConnectionMode = ConnectionMode.Direct,
                RequestTimeout=TimeSpan.FromSeconds(60)

            });           

            return client;
        }
    }
}
