using SalePageServer.Report;
using System.Threading.Tasks;

namespace RetailTradeServer.State.Reports
{
    public interface IReportService
    {
        Task<LabelReport> CreateLabelReport();
    }
}
