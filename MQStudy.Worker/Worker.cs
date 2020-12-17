using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MQStudy.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const int minuteBuff = 10;
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now:yyyyMMdd HH:mm:ss}, " +
                    $"next log time {DateTimeOffset.Now.AddMinutes(minuteBuff):yyyyMMdd HH:mm:ss}");
                await Task.Delay(TimeSpan.FromMinutes(minuteBuff), stoppingToken);
            }
        }
    }
}
