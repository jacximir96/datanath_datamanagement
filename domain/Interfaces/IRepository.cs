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
        Task<ResponseDomain> Update(SubRequest subReq, Connection connection, string status);
    }
}
