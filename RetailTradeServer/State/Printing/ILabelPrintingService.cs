using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RetailTradeServer.State.Printing
{
    public interface ILabelPrintingService
    {
        public ObservableCollection<LabelPrinting> LabelPrintings { get; set; }
        void Add(LabelPrinting labelPrinting);
        void Delete(LabelPrinting labelPrinting);
        void Clear();
    }
}
