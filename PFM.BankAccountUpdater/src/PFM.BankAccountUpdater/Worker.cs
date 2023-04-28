namespace PFM.BankAccountUpdater
{
    public class Worker : BackgroundService
    {
        private readonly Serilog.ILogger _logger;

        public Worker(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.Information("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}