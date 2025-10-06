using domain.Entities;
using domain.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Net;


namespace infrastructure.Repository
{
    public class RequirementRepository : IRepository<Template>
    {

        private readonly IConnectionFactory _connection;
        private readonly IConfiguration _configuration;
      
        public RequirementRepository(IConnectionFactory connection, IConfiguration configuration)
        {                    
            _connection = connection;
            _configuration = configuration;
        }

        public async Task<ResponseDomain> CreateTemplate(Template template)
        {
            ResponseDomain resDomain = null;
            try
            {             
                using (CosmosClient cosmos = (CosmosClient)_connection.CreateConnectio("cosmosdb").GetObjectDataBase()) 
                {
                    var container = cosmos.GetDatabase(_configuration.GetSection("cosmosdb").Value).GetContainer(_configuration.GetSection("requirementcontainer").Value);
                    var res = await container.CreateItemAsync(template, new PartitionKey(template.id));
                    resDomain = new ResponseDomain();
                    resDomain.template = res.Resource;
                    resDomain.StatusCode = HttpStatusCode.OK;
                    resDomain.Message = _configuration.GetSection("message_create_request").Value;
                    resDomain.RequestId = template.id;
                    resDomain.Code = 200;
                    resDomain.Error = false;

                }
                                
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            return resDomain;
        }


    }
}
