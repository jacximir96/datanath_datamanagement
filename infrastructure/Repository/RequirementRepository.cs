using domain.Entities;
using domain.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Net;


namespace infrastructure.Repository
{
    public class RequirementRepository : IRepository, IResponseDomain
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
              return resDomain;              
            }
            catch (Exception e)
            {
                resDomain = GetResponse(e.Message,false);
                return resDomain;
            }

        }

        public async Task<ResponseDomain> Update(SubRequest subReq, Connection connection, string status)
        {
            ResponseDomain res = null;
            try
            {

                using (CosmosClient cosmos = (CosmosClient)_connection.CreateConnection(connection.adapter).GetObjectDataBase())
                {
                    var container = cosmos.GetDatabase(_configuration.GetSection("cosmosdb").Value).GetContainer(_configuration.GetSection("requirementcontainer").Value);

                    var response = await container.ReadItemAsync<Requirement>(
                            subReq.id,
                            new PartitionKey(subReq.id)
                         );

                    Requirement _requirement = response.Resource;
                    _requirement.status = status;
                    _requirement.progress = subReq.progress;
                    _requirement.completedAt = subReq.completedAt;
                    _requirement.references = subReq.references;

                    await container.ReplaceItemAsync(
                                    item: _requirement,
                                    id: _requirement.id,
                    partitionKey: new PartitionKey(subReq.id));
                    res = GetResponse(ResourceInfra.RecordUpdatedOk, true);
                    return res;
                }
            }
            catch (Exception e)
            {
                res = GetResponse(e.Message, false);
                return res;
            }
        }


        public ResponseDomain GetResponse(string message, bool isSuccess)
        {
            ResponseDomain response = new ResponseDomain();
            if (isSuccess)
            {
                response.Message = message;
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Message = message;
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }
    }
}
