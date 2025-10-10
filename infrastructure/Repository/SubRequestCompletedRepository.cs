using domain.Entities;
using infrastructure;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace domain.Interfaces
{
    public class SubRequestCompletedRepository: ISubRequestCompletedRepository, IResponseDomain
    {
        private readonly IConnectionFactory _connection;
        private readonly IConfiguration _configuration;

        public SubRequestCompletedRepository(IConnectionFactory connection, IConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }

        public async Task<ResponseDomain> Update(SubRequestCompleted subReq, Connection connection)
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
                    _requirement.status = subReq.status;
                    _requirement.progress = subReq.progress;
                    _requirement.completedAt = subReq.completedAt;
                    _requirement.references = subReq.references;

                    await container.ReplaceItemAsync(
                                    item: _requirement,
                                    id: _requirement.id,
                    partitionKey: new PartitionKey(subReq.id));
                    res = GetResponse(ResourceInfra.RecordUpdatedOk,true);
                    return res; 
                }
            }
            catch (Exception e)
            {
                res = GetResponse(e.Message,false);
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
