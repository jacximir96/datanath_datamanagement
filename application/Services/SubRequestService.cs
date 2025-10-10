using application.Interfaces;
using domain.Entities;
using domain.Interfaces;
using infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace application.Services
{
    public class SubRequestService : ISubRequest
    {

        private readonly IRepository _repository;
        private readonly ISubRequestRepository _subReqRepository;
        private readonly IConfiguration _configuration;

        public SubRequestService(IRepository repository, ISubRequestRepository subReqRepository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
            _subReqRepository = subReqRepository;   
        }

        public async Task<ResponseDomain> ReceiveSubRequestCompleted(SubRequest subReq)
        {
            ResponseDomain response = null;
            var countAll = 0;
            var countFailed = 0;
            try
            {
                Connection connection = new Connection();
                connection.server = _configuration.GetSection("endpoint").Value;
                connection.password = _configuration.GetSection("key").Value;
                List<SubRequest> subrequests = await _subReqRepository.GetSubRequests(subReq.requestParent, connection);
                countAll = subrequests.Count;
                foreach (var subrequest in subrequests) 
                {
                    if (subrequest.status== "FAILED")
                    { 
                        countFailed=countFailed+1;
                    }
                }

                if (countFailed > 0 && countFailed != countAll)
                {
                    await _repository.Update(subReq, connection, _configuration.GetSection("statusParentPartialFailed").Value);
                }
                else 
                {
                    if (countFailed == countAll) 
                    {
                      await _repository.Update(subReq, connection, _configuration.GetSection("statusParentFailed").Value);
                    }

                    if (countFailed == 0) { }
                        await _repository.Update(subReq, connection, _configuration.GetSection("statusParentCompleted").Value);
                }
                    
                response = GetResponse(ResourceApp.RecordUpdatedOk, true);
                return response;    
            }
            catch (Exception e) 
            {
                response=GetResponse(ResourceApp.RecordUpdatedOk,false);
                return response;    
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
