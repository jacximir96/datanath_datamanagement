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
    public class SubRequestRepository: ISubRequestRepository
    {
        private readonly IConnectionFactory _connection;
        private readonly IConfiguration _configuration;

        public SubRequestRepository(IConnectionFactory connection, IConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }


        public async Task<List<SubRequest>> GetSubRequests(string parentSubRequest, Connection connection)
        {
            List<SubRequest> subrequest = new List<SubRequest>();
            try
            {
                using (CosmosClient cosmos = (CosmosClient)_connection.CreateConnection(connection.adapter).GetObjectDataBase())
                {
                    var container = cosmos.GetDatabase(_configuration.GetSection("cosmosdb").Value).GetContainer(_configuration.GetSection("subrequestcontainer").Value);
                    var queryDefinition = new QueryDefinition("SELECT * FROM c where c.parentRequest=@parentRequest").WithParameter("@parentRequest", parentSubRequest);
                    using (var query = container.GetItemQueryIterator<SubRequest>(queryDefinition))
                    {

                        while (query.HasMoreResults)
                        {
                            var _response = await query.ReadNextAsync();
                            subrequest.AddRange(_response.ToList());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.StackTrace.ToString();
            }
            return subrequest;
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
