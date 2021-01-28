using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutoReport
{
    public class ReportRunner
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ReportRunner> _logger;
        private readonly Reporter _reporter;

        public ReportRunner(Reporter reporter, IConfiguration config, ILogger<ReportRunner> logger)
        {
            _reporter = reporter;
            _config = config;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken token)
        {
            _logger.LogInformation("Report runner has started.");
            while (true)
            {
                token.ThrowIfCancellationRequested();
                var config = _config.Get<AppConfig>();
                if (config.ReportAt == DateTime.Now.Hour)
                {
                    _logger.LogInformation("Report begin.");
                    foreach (var (_, info) in config.Reports) await _reporter.ReportAsync(info, token);
                    _logger.LogInformation($"Report finished, total report {config.Reports.Count}.");
                }


                //Sleep an hour
                var nextHour = DateTime.Now.AddHours(1);
                var nextWake = nextHour - new TimeSpan(0, nextHour.Minute, nextHour.Second);
                await Task.Delay(nextWake - DateTime.Now, token);
            }
        }
    }
}