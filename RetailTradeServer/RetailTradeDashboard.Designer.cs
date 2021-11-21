
using RetailTrade.Domain.Models;

namespace RetailTradeServer
{
    partial class RetailTradeDashboard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.DashboardCommon.Measure measure1 = new DevExpress.DashboardCommon.Measure();
            DevExpress.DashboardCommon.Card card1 = new DevExpress.DashboardCommon.Card();
            DevExpress.DashboardCommon.CardCenteredLayoutTemplate cardCenteredLayoutTemplate1 = new DevExpress.DashboardCommon.CardCenteredLayoutTemplate();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetailTradeDashboard));
            DevExpress.DashboardCommon.Measure measure2 = new DevExpress.DashboardCommon.Measure();
            DevExpress.DashboardCommon.Card card2 = new DevExpress.DashboardCommon.Card();
            DevExpress.DashboardCommon.CardCenteredLayoutTemplate cardCenteredLayoutTemplate2 = new DevExpress.DashboardCommon.CardCenteredLayoutTemplate();
            DevExpress.DashboardCommon.Measure measure3 = new DevExpress.DashboardCommon.Measure();
            DevExpress.DashboardCommon.Card card3 = new DevExpress.DashboardCommon.Card();
            DevExpress.DashboardCommon.CardCenteredLayoutTemplate cardCenteredLayoutTemplate3 = new DevExpress.DashboardCommon.CardCenteredLayoutTemplate();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup1 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem1 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            this.ReceiptCardDashboardItem = new DevExpress.DashboardCommon.CardDashboardItem();
            this.ReceiptDataSource = new DevExpress.DashboardCommon.DashboardObjectDataSource();
            ((System.ComponentModel.ISupportInitialize)(this.ReceiptCardDashboardItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(measure1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(measure2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(measure3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReceiptDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ReceiptCardDashboardItem
            // 
            measure1.DataMember = "Sum";
            resources.ApplyResources(measure1, "measure1");
            measure1.NumericFormat.CustomFormatString = "";
            measure1.NumericFormat.FormatType = DevExpress.DashboardCommon.DataItemNumericFormatType.Number;
            measure1.NumericFormat.IncludeGroupSeparator = true;
            measure1.NumericFormat.Unit = DevExpress.DashboardCommon.DataItemNumericUnit.Ones;
            cardCenteredLayoutTemplate1.BottomSubValue1.DimensionIndex = 0;
            cardCenteredLayoutTemplate1.BottomSubValue1.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.AbsoluteVariation;
            cardCenteredLayoutTemplate1.BottomSubValue1.Visible = true;
            cardCenteredLayoutTemplate1.BottomSubValue2.DimensionIndex = 0;
            cardCenteredLayoutTemplate1.BottomSubValue2.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.PercentVariation;
            cardCenteredLayoutTemplate1.BottomSubValue2.Visible = true;
            cardCenteredLayoutTemplate1.BottomValue.DimensionIndex = 0;
            cardCenteredLayoutTemplate1.BottomValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.ActualValue;
            cardCenteredLayoutTemplate1.BottomValue.Visible = true;
            cardCenteredLayoutTemplate1.DeltaIndicator.Visible = true;
            cardCenteredLayoutTemplate1.MainValue.DimensionIndex = 0;
            cardCenteredLayoutTemplate1.MainValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.Title;
            cardCenteredLayoutTemplate1.MainValue.Visible = true;
            cardCenteredLayoutTemplate1.MaxWidth = 270;
            cardCenteredLayoutTemplate1.MinWidth = 270;
            cardCenteredLayoutTemplate1.Sparkline.Visible = true;
            cardCenteredLayoutTemplate1.SubValue.DimensionIndex = 0;
            cardCenteredLayoutTemplate1.SubValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.Subtitle;
            cardCenteredLayoutTemplate1.SubValue.Visible = true;
            card1.LayoutTemplate = cardCenteredLayoutTemplate1;
            resources.ApplyResources(card1, "card1");
            card1.AddDataItem("ActualValue", measure1);
            measure2.DataMember = "PaidInCash";
            resources.ApplyResources(measure2, "measure2");
            measure2.NumericFormat.CustomFormatString = "";
            measure2.NumericFormat.FormatType = DevExpress.DashboardCommon.DataItemNumericFormatType.Number;
            measure2.NumericFormat.IncludeGroupSeparator = true;
            measure2.NumericFormat.Unit = DevExpress.DashboardCommon.DataItemNumericUnit.Ones;
            cardCenteredLayoutTemplate2.BottomSubValue1.DimensionIndex = 0;
            cardCenteredLayoutTemplate2.BottomSubValue1.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.AbsoluteVariation;
            cardCenteredLayoutTemplate2.BottomSubValue1.Visible = true;
            cardCenteredLayoutTemplate2.BottomSubValue2.DimensionIndex = 0;
            cardCenteredLayoutTemplate2.BottomSubValue2.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.PercentVariation;
            cardCenteredLayoutTemplate2.BottomSubValue2.Visible = true;
            cardCenteredLayoutTemplate2.BottomValue.DimensionIndex = 0;
            cardCenteredLayoutTemplate2.BottomValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.ActualValue;
            cardCenteredLayoutTemplate2.BottomValue.Visible = true;
            cardCenteredLayoutTemplate2.DeltaIndicator.Visible = true;
            cardCenteredLayoutTemplate2.MainValue.DimensionIndex = 0;
            cardCenteredLayoutTemplate2.MainValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.Title;
            cardCenteredLayoutTemplate2.MainValue.Visible = true;
            cardCenteredLayoutTemplate2.MaxWidth = 270;
            cardCenteredLayoutTemplate2.MinWidth = 270;
            cardCenteredLayoutTemplate2.Sparkline.Visible = true;
            cardCenteredLayoutTemplate2.SubValue.DimensionIndex = 0;
            cardCenteredLayoutTemplate2.SubValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.Subtitle;
            cardCenteredLayoutTemplate2.SubValue.Visible = true;
            card2.LayoutTemplate = cardCenteredLayoutTemplate2;
            resources.ApplyResources(card2, "card2");
            card2.AddDataItem("ActualValue", measure2);
            measure3.DataMember = "PaidInCashless";
            resources.ApplyResources(measure3, "measure3");
            measure3.NumericFormat.CustomFormatString = "";
            measure3.NumericFormat.FormatType = DevExpress.DashboardCommon.DataItemNumericFormatType.Number;
            measure3.NumericFormat.IncludeGroupSeparator = true;
            measure3.NumericFormat.Unit = DevExpress.DashboardCommon.DataItemNumericUnit.Ones;
            cardCenteredLayoutTemplate3.BottomSubValue1.DimensionIndex = 0;
            cardCenteredLayoutTemplate3.BottomSubValue1.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.AbsoluteVariation;
            cardCenteredLayoutTemplate3.BottomSubValue1.Visible = true;
            cardCenteredLayoutTemplate3.BottomSubValue2.DimensionIndex = 0;
            cardCenteredLayoutTemplate3.BottomSubValue2.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.PercentVariation;
            cardCenteredLayoutTemplate3.BottomSubValue2.Visible = true;
            cardCenteredLayoutTemplate3.BottomValue.DimensionIndex = 0;
            cardCenteredLayoutTemplate3.BottomValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.ActualValue;
            cardCenteredLayoutTemplate3.BottomValue.Visible = true;
            cardCenteredLayoutTemplate3.DeltaIndicator.Visible = true;
            cardCenteredLayoutTemplate3.MainValue.DimensionIndex = 0;
            cardCenteredLayoutTemplate3.MainValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.Title;
            cardCenteredLayoutTemplate3.MainValue.Visible = true;
            cardCenteredLayoutTemplate3.MaxWidth = 270;
            cardCenteredLayoutTemplate3.MinWidth = 270;
            cardCenteredLayoutTemplate3.Sparkline.Visible = true;
            cardCenteredLayoutTemplate3.SubValue.DimensionIndex = 0;
            cardCenteredLayoutTemplate3.SubValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.Subtitle;
            cardCenteredLayoutTemplate3.SubValue.Visible = true;
            card3.LayoutTemplate = cardCenteredLayoutTemplate3;
            resources.ApplyResources(card3, "card3");
            card3.AddDataItem("ActualValue", measure3);
            this.ReceiptCardDashboardItem.Cards.AddRange(new DevExpress.DashboardCommon.Card[] {
            card1,
            card2,
            card3});
            this.ReceiptCardDashboardItem.ComponentName = "ReceiptCardDashboardItem";
            this.ReceiptCardDashboardItem.DataItemRepository.Clear();
            this.ReceiptCardDashboardItem.DataItemRepository.Add(measure2, "DataItem1");
            this.ReceiptCardDashboardItem.DataItemRepository.Add(measure1, "DataItem0");
            this.ReceiptCardDashboardItem.DataItemRepository.Add(measure3, "DataItem2");
            this.ReceiptCardDashboardItem.DataSource = this.ReceiptDataSource;
            this.ReceiptCardDashboardItem.InteractivityOptions.IgnoreMasterFilters = false;
            resources.ApplyResources(this.ReceiptCardDashboardItem, "ReceiptCardDashboardItem");
            this.ReceiptCardDashboardItem.ShowCaption = false;
            // 
            // ReceiptDataSource
            // 
            this.ReceiptDataSource.ComponentName = "ReceiptDataSource";
            this.ReceiptDataSource.Name = "Чек";
            // 
            // RetailTradeDashboard
            // 
            this.DataSources.AddRange(new DevExpress.DashboardCommon.IDashboardDataSource[] {
            this.ReceiptDataSource});
            this.Items.AddRange(new DevExpress.DashboardCommon.DashboardItem[] {
            this.ReceiptCardDashboardItem});
            this.LayoutOptions.Height.Value = 200;
            dashboardLayoutItem1.DashboardItem = this.ReceiptCardDashboardItem;
            dashboardLayoutItem1.Weight = 50.100603621730386D;
            dashboardLayoutGroup1.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutItem1});
            dashboardLayoutGroup1.DashboardItem = null;
            dashboardLayoutGroup1.Weight = 100D;
            this.LayoutRoot = dashboardLayoutGroup1;
            this.Title.ShowMasterFilterState = false;
            this.Title.Text = resources.GetString("RetailTradeDashboard.Title.Text");
            ((System.ComponentModel.ISupportInitialize)(measure1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(measure2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(measure3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReceiptCardDashboardItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReceiptDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        public DevExpress.DashboardCommon.DashboardObjectDataSource ReceiptDataSource;
        public DevExpress.DashboardCommon.CardDashboardItem ReceiptCardDashboardItem;
    }
}
