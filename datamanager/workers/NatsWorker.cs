
using application.Interfaces;
using domain.Entities;
using NATS.Client.Core;

namespace datamanager.workers
{
    public class NatsWorker : BackgroundService
    {

        
        private readonly IConfiguration _config;
        private readonly INatsConnection _nats;
        private readonly ISubRequestCompleted _subRequestCompleted;

        public NatsWorker(ISubRequestCompleted subRequestCompleted, IConfiguration config)
        {         
            _config = config;
            var opts = new NatsOpts { Url = _config.GetSection("natsurl").Value };
            _nats = new NatsConnection(opts);
            _subRequestCompleted = subRequestCompleted;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var msg in _nats.SubscribeAsync<SubRequestCompleted>("subject.>", cancellationToken: stoppingToken))
            {
                var subReq = msg.Data;
                if (msg.Subject == "subject.SUBREQUEST_COMPLETED")
                {
                    await _subRequestCompleted.ReceiveSubRequestCompleted(subReq);
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
