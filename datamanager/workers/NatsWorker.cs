
using application.Interfaces;
using domain.Entities;
using NATS.Client.Core;

namespace datamanager.workers
{
    public class NatsWorker : BackgroundService
    {

        
        private readonly IConfiguration _config;
        private readonly INatsConnection _nats;
        private readonly ISubRequest _subRequest;

        public NatsWorker(ISubRequest subRequest, IConfiguration config)
        {         
             _config = config;
            _subRequest = subRequest;
            var opts = new NatsOpts { Url = _config.GetSection("natsurl").Value };
            _nats = new NatsConnection(opts);
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {

                    await foreach (var msg in _nats.SubscribeAsync<SubRequest>("subject.>", cancellationToken: stoppingToken))
                    {
                        var subReq = msg.Data;
                        if (msg.Subject == "subject.SUBREQUEST_COMPLETED")
                        {
                            await _subRequest.ReceiveSubRequestCompleted(subReq);
                            await Task.Delay(5000, stoppingToken);
                        }
                    }
                
            }
            catch (Exception e)
            { 
                Console.WriteLine(e.StackTrace);
            }
            
        }
    }
}
