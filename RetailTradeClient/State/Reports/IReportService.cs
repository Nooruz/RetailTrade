﻿using RetailTrade.Domain.Models;
using RetailTradeClient.Report;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Reports
{
    public interface IReportService
    {
        Task<XReport> CreateXReport();
        Task<ProductSaleReport> CreateProductSaleReport(Receipt receipt);
    }
}
