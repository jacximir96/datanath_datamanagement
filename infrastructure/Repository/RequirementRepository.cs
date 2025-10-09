using domain.Entities;
using domain.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Net;


namespace infrastructure.Repository
{
    public class RequirementRepository : IRepository
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
                using (CosmosClient cosmos = (CosmosClient)_connection.CreateConnection("cosmosdb").GetObjectDataBase()) 
                {
                    var container = cosmos.GetDatabase(_configuration.GetSection("cosmosdb").Value).GetContainer(_configuration.GetSection("requirementcontainer").Value);
                    var res = await container.CreateItemAsync(template, new PartitionKey(template.id));
                    resDomain = new ResponseDomain();
                    resDomain.template = res.Resource;
                    resDomain.StatusCode = HttpStatusCode.OK;
                    resDomain.Message = ResourceInfra.MessageRequestCreated;
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

        public async Task<Requirement> GetRequirement(string idrequirement, Connection connection)
        {
            List<Requirement> requirements = new List<Requirement>();
            try
            {
                using (CosmosClient cosmos = (CosmosClient)_connection.CreateConnection(connection.adapter).GetObjectDataBase())
                {
                    var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
    .WithParameter("@id", idrequirement);
                    var container = cosmos.GetDatabase(_configuration.GetSection("cosmosdb").Value).GetContainer(_configuration.GetSection("requirementcontainer").Value);
                    using (var result = container.GetItemQueryIterator<Requirement>(query))
                    {

                        while (result.HasMoreResults)
                        {
                            var _response = await result.ReadNextAsync();
                            requirements.AddRange(_response.ToList());
                        }
                    }
                }

            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            return requirements.FirstOrDefault();
        }
    }
}
