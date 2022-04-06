using RetailTrade.Domain.Models;
using System;
using System.Collections.ObjectModel;

namespace RetailTradeServer.State.Printing
{
    public class LabelPrintingService : ILabelPrintingService
    {
        #region Private Members

        private ObservableCollection<LabelPrinting> _labelPrintings = new();

        #endregion

        public ObservableCollection<LabelPrinting> LabelPrintings
        {
            get => _labelPrintings;
            set => _labelPrintings = value;
        }

        public void Add(LabelPrinting labelPrinting)
        {
            try
            {
                LabelPrintings.Add(labelPrinting);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public void Clear()
        {
            try
            {
                LabelPrintings.Clear();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public void Delete(LabelPrinting labelPrinting)
        {
            try
            {
                LabelPrintings.Remove(labelPrinting);
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}
