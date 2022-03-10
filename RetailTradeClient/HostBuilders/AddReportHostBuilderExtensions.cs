using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTradeClient.Report;
using RetailTradeClient.State.Reports;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using System;

namespace RetailTradeClient.HostBuilders
{
    public static class AddReportHostBuilderExtensions
    {
        public static IHostBuilder AddReports(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {                
                _ = services.AddSingleton(CreateXReport);
                _ = services.AddSingleton<IReportService, ReportService>();
            });
        }

        private static XReport CreateXReport(IServiceProvider serviceProvider)
        {
            return new XReport(serviceProvider.GetRequiredService<IUserStore>(),
                serviceProvider.GetRequiredService<IShiftStore>());
        }
    }
}
