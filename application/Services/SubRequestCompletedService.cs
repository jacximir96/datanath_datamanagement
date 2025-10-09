using application.Interfaces;
using domain.Entities;
using infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Services
{
    public class SubRequestCompletedService : ISubRequestCompleted
    {
        

        public SubRequestCompletedService()
        {
            
        }

        public async Task ReceiveSubRequestCompleted(SubRequestCompleted subReq)
        {
            try
            {
                //await _repository.Update(subReq, null);
            }
            catch (Exception e) 
            {
                e.Message.ToString();
            }
        }
    }
}
