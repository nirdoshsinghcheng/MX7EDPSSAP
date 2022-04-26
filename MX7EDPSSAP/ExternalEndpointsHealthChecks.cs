using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace MX7EDPSSAP
{
    public class ExternalEndpointsHealthChecks : IHealthCheck
    {
        private readonly ServiceSettings settings;

        public ExternalEndpointsHealthChecks(IOptions<ServiceSettings> options)
        {
            settings = options.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                Ping ping = new();
                var reply = await ping.SendPingAsync(settings.OpenWeatherHost);

                if (reply.Status != IPStatus.Success)
                {
                    return HealthCheckResult.Unhealthy();
                }
                //if (reply.Address.ScopeId != 5)
                //{
                //    return HealthCheckResult.Unhealthy();
                //}
                return HealthCheckResult.Healthy();
            }
            catch (System.Net.Sockets.SocketException)
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
