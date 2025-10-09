using application.Interfaces;
using domain.Entities;
using infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using System.Net;


namespace application.Services
{
    public class RequirementService : IRequirement
    {
        private readonly IRepository _repository;
        private readonly IConfiguration _config;
        
        public RequirementService(IRepository repository, IConfiguration config) 
        { 
            _repository = repository;
            _config = config;
        }   

        public async Task<ResponseDomain> CreateTemplate(Template template)
        {
            ResponseDomain response = null;
            try
            {
                template.id = Guid.NewGuid().ToString();
                template.status = _config.GetSection("estado").Value;
                response=await _repository.CreateTemplate(template);

            }
            catch (Exception e) 
            {
                e.Message.ToString();
            }
            return response;
       
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
