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
    public class SubRequestCompletedService : ISubRequestCompleted, IResponseDomain
    {

        private readonly ISubRequestCompletedRepository _subReqRepository;
        private readonly IConfiguration _configuration;
        public SubRequestCompletedService(ISubRequestCompletedRepository subReqRepository, IConfiguration configuration)
        {
            _subReqRepository = subReqRepository;
            _configuration = configuration;
        }

        public async Task<ResponseDomain> ReceiveSubRequestCompleted(SubRequestCompleted subReq)
        {
            ResponseDomain response = null;
            try
            {
                Connection connection = new Connection();
                connection.server = _configuration.GetSection("endpoint").Value;
                connection.password = _configuration.GetSection("key").Value;
                await _subReqRepository.Update(subReq, connection);
                response = GetResponse(ResourceApp.RecordUpdatedOk,true);
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
