using RetailTradeClient.Report;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Reports
{
    public interface IReportService
    {
        Task<XReport> CreateXReport();
    }
}
