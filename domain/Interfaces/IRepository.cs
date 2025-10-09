using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Repository
{
    public interface IRepository
    {
        Task<ResponseDomain> CreateTemplate(Template template);
        Task<Requirement> GetRequirement(string idrequirement, Connection connection);       
    }
}
