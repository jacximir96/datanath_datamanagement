using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<ResponseDomain> CreateTemplate(T template);

    }
}
