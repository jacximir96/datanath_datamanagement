using application.Interfaces;
using domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace datamanager.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RequirementController : ControllerBase
    {
        private readonly IRequirement _requirement;  
        public RequirementController(IRequirement requirement) 
        { 
            _requirement = requirement;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequirement(Template template) 
        {
            try
            {
               
                var res=await _requirement.CreateTemplate(template);
                return Ok(res);    
            }
            catch (Exception e)
            { 
                return BadRequest();   
            }
        }
    
    }
}
