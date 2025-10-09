using domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Interfaces
{
    public class SubRequestCompletedRepository: ISubRequestCompletedRepository
    {
        private readonly IConnectionFactory _connection;
        private readonly IConfiguration _configuration;

        public SubRequestCompletedRepository(IConnectionFactory connection, IConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }

        public async Task Update(SubRequestCompleted subReq, Connection connection)
        {
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
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
        }
    }
}
