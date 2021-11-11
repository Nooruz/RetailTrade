using DevExpress.DashboardCommon;
using System.ComponentModel;

namespace RetailTradeServer
{
    partial class RetailTradeDashboard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            DevExpress.DashboardCommon.CardStretchedLayoutTemplate cardStretchedLayoutTemplate1 = new DevExpress.DashboardCommon.CardStretchedLayoutTemplate();
            DevExpress.DashboardCommon.Dimension dimension1 = new DevExpress.DashboardCommon.Dimension();
            DevExpress.DashboardCommon.Dimension dimension2 = new DevExpress.DashboardCommon.Dimension();
            DevExpress.DashboardCommon.Measure measure2 = new DevExpress.DashboardCommon.Measure();
            DevExpress.DashboardCommon.Measure measure3 = new DevExpress.DashboardCommon.Measure();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup1 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup2 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup3 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem1 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem2 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup4 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup5 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup6 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup7 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            this.SalesByCashiers = new DevExpress.DashboardCommon.CardDashboardItem();
            this.Receipts = new DevExpress.DashboardCommon.DashboardObjectDataSource();
            this.PieSalesByCashiers = new DevExpress.DashboardCommon.PieDashboardItem();
            this.dashboardItemGroup1 = new DevExpress.DashboardCommon.DashboardItemGroup();
            this.dashboardItemGroup2 = new DevExpress.DashboardCommon.DashboardItemGroup();
            this.dashboardItemGroup3 = new DevExpress.DashboardCommon.DashboardItemGroup();
            this.dashboardItemGroup4 = new DevExpress.DashboardCommon.DashboardItemGroup();
            ((System.ComponentModel.ISupportInitialize)(this.SalesByCashiers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(measure1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dimension1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Receipts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PieSalesByCashiers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dimension2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(measure2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(measure3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardItemGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardItemGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardItemGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardItemGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // SalesByCashiers
            // 
            measure1.DataMember = "Sum";
            measure1.NumericFormat.CurrencyCultureName = "ky-KG";
            measure1.NumericFormat.CustomFormatString = "";
            measure1.NumericFormat.FormatType = DevExpress.DashboardCommon.DataItemNumericFormatType.Currency;
            measure1.NumericFormat.IncludeGroupSeparator = true;
            measure1.NumericFormat.Unit = DevExpress.DashboardCommon.DataItemNumericUnit.Ones;
            cardStretchedLayoutTemplate1.BottomValue1.DimensionIndex = 0;
            cardStretchedLayoutTemplate1.BottomValue1.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.PercentVariation;
            cardStretchedLayoutTemplate1.BottomValue1.Visible = true;
            cardStretchedLayoutTemplate1.BottomValue2.DimensionIndex = 0;
            cardStretchedLayoutTemplate1.BottomValue2.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.AbsoluteVariation;
            cardStretchedLayoutTemplate1.BottomValue2.Visible = true;
            cardStretchedLayoutTemplate1.DeltaIndicator.Visible = true;
            cardStretchedLayoutTemplate1.MainValue.DimensionIndex = 0;
            cardStretchedLayoutTemplate1.MainValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.Title;
            cardStretchedLayoutTemplate1.MainValue.Visible = true;
            cardStretchedLayoutTemplate1.Sparkline.Visible = true;
            cardStretchedLayoutTemplate1.SubValue.DimensionIndex = 0;
            cardStretchedLayoutTemplate1.SubValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.Subtitle;
            cardStretchedLayoutTemplate1.SubValue.Visible = true;
            cardStretchedLayoutTemplate1.TopValue.DimensionIndex = 0;
            cardStretchedLayoutTemplate1.TopValue.ValueType = DevExpress.DashboardCommon.CardRowDataElementType.ActualValue;
            cardStretchedLayoutTemplate1.TopValue.Visible = true;
            card1.LayoutTemplate = cardStretchedLayoutTemplate1;
            card1.Name = "Сумма";
            card1.AddDataItem("ActualValue", measure1);
            this.SalesByCashiers.Cards.AddRange(new DevExpress.DashboardCommon.Card[] {
            card1});
            this.SalesByCashiers.ComponentName = "SalesByCashiers";
            dimension1.DataMember = "Shift.User.FullName";
            this.SalesByCashiers.DataItemRepository.Clear();
            this.SalesByCashiers.DataItemRepository.Add(measure1, "DataItem1");
            this.SalesByCashiers.DataItemRepository.Add(dimension1, "DataItem0");
            this.SalesByCashiers.DataSource = this.Receipts;
            this.SalesByCashiers.InteractivityOptions.IgnoreMasterFilters = false;
            this.SalesByCashiers.Name = "Продажи по кассирам";
            this.SalesByCashiers.ParentContainer = this.dashboardItemGroup1;
            this.SalesByCashiers.SeriesDimensions.AddRange(new DevExpress.DashboardCommon.Dimension[] {
            dimension1});
            this.SalesByCashiers.ShowCaption = true;
            // 
            // Receipts
            // 
            this.Receipts.ComponentName = "Receipts";
            this.Receipts.Name = "Чеки";
            // 
            // PieSalesByCashiers
            // 
            this.PieSalesByCashiers.ComponentName = "PieSalesByCashiers";
            dimension2.DataMember = "Shift.User.FullName";
            dimension2.Name = "Кассир";
            measure2.DataMember = "PaidInCashless";
            measure2.Name = "Безналичными";
            measure3.DataMember = "PaidInCash";
            measure3.Name = "Наличными";
            this.PieSalesByCashiers.DataItemRepository.Clear();
            this.PieSalesByCashiers.DataItemRepository.Add(dimension2, "DataItem0");
            this.PieSalesByCashiers.DataItemRepository.Add(measure2, "DataItem2");
            this.PieSalesByCashiers.DataItemRepository.Add(measure3, "DataItem1");
            this.PieSalesByCashiers.DataSource = this.Receipts;
            this.PieSalesByCashiers.InteractivityOptions.IgnoreMasterFilters = false;
            this.PieSalesByCashiers.Name = "Диаграмма продажи по кассирам";
            this.PieSalesByCashiers.ParentContainer = this.dashboardItemGroup1;
            this.PieSalesByCashiers.SeriesDimensions.AddRange(new DevExpress.DashboardCommon.Dimension[] {
            dimension2});
            this.PieSalesByCashiers.ShowCaption = true;
            this.PieSalesByCashiers.Values.AddRange(new DevExpress.DashboardCommon.Measure[] {
            measure3,
            measure2});
            // 
            // dashboardItemGroup1
            // 
            this.dashboardItemGroup1.ComponentName = "dashboardItemGroup1";
            this.dashboardItemGroup1.InteractivityOptions.IgnoreMasterFilters = true;
            this.dashboardItemGroup1.InteractivityOptions.IsMasterFilter = false;
            this.dashboardItemGroup1.Name = "Кассиры";
            this.dashboardItemGroup1.ShowCaption = true;
            // 
            // dashboardItemGroup2
            // 
            this.dashboardItemGroup2.ComponentName = "dashboardItemGroup2";
            this.dashboardItemGroup2.InteractivityOptions.IgnoreMasterFilters = true;
            this.dashboardItemGroup2.InteractivityOptions.IsMasterFilter = false;
            this.dashboardItemGroup2.Name = "Группа 1";
            this.dashboardItemGroup2.ShowCaption = true;
            // 
            // dashboardItemGroup3
            // 
            this.dashboardItemGroup3.ComponentName = "dashboardItemGroup3";
            this.dashboardItemGroup3.InteractivityOptions.IgnoreMasterFilters = true;
            this.dashboardItemGroup3.InteractivityOptions.IsMasterFilter = false;
            this.dashboardItemGroup3.Name = "Группа 2";
            this.dashboardItemGroup3.ShowCaption = true;
            // 
            // dashboardItemGroup4
            // 
            this.dashboardItemGroup4.ComponentName = "dashboardItemGroup4";
            this.dashboardItemGroup4.InteractivityOptions.IgnoreMasterFilters = true;
            this.dashboardItemGroup4.InteractivityOptions.IsMasterFilter = false;
            this.dashboardItemGroup4.Name = "Группа 3";
            this.dashboardItemGroup4.ShowCaption = true;
            // 
            // RetailTradeDashboard
            // 
            this.DataSources.AddRange(new DevExpress.DashboardCommon.IDashboardDataSource[] {
            this.Receipts});
            this.Groups.AddRange(new DevExpress.DashboardCommon.DashboardItemGroup[] {
            this.dashboardItemGroup1,
            this.dashboardItemGroup2,
            this.dashboardItemGroup3,
            this.dashboardItemGroup4});
            this.Items.AddRange(new DevExpress.DashboardCommon.DashboardItem[] {
            this.PieSalesByCashiers,
            this.SalesByCashiers});
            dashboardLayoutItem1.DashboardItem = this.SalesByCashiers;
            dashboardLayoutItem1.Weight = 49.728260869565219D;
            dashboardLayoutItem2.DashboardItem = this.PieSalesByCashiers;
            dashboardLayoutItem2.Weight = 50.271739130434781D;
            dashboardLayoutGroup3.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutItem1,
            dashboardLayoutItem2});
            dashboardLayoutGroup3.DashboardItem = this.dashboardItemGroup1;
            dashboardLayoutGroup3.Orientation = DevExpress.DashboardCommon.DashboardLayoutGroupOrientation.Vertical;
            dashboardLayoutGroup3.Weight = 50D;
            dashboardLayoutGroup4.DashboardItem = this.dashboardItemGroup3;
            dashboardLayoutGroup4.Orientation = DevExpress.DashboardCommon.DashboardLayoutGroupOrientation.Vertical;
            dashboardLayoutGroup4.Weight = 50D;
            dashboardLayoutGroup2.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutGroup3,
            dashboardLayoutGroup4});
            dashboardLayoutGroup2.DashboardItem = null;
            dashboardLayoutGroup2.Weight = 50D;
            dashboardLayoutGroup6.DashboardItem = this.dashboardItemGroup2;
            dashboardLayoutGroup6.Orientation = DevExpress.DashboardCommon.DashboardLayoutGroupOrientation.Vertical;
            dashboardLayoutGroup6.Weight = 50D;
            dashboardLayoutGroup7.DashboardItem = this.dashboardItemGroup4;
            dashboardLayoutGroup7.Orientation = DevExpress.DashboardCommon.DashboardLayoutGroupOrientation.Vertical;
            dashboardLayoutGroup7.Weight = 50D;
            dashboardLayoutGroup5.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutGroup6,
            dashboardLayoutGroup7});
            dashboardLayoutGroup5.DashboardItem = null;
            dashboardLayoutGroup5.Weight = 50D;
            dashboardLayoutGroup1.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutGroup2,
            dashboardLayoutGroup5});
            dashboardLayoutGroup1.DashboardItem = null;
            dashboardLayoutGroup1.Orientation = DevExpress.DashboardCommon.DashboardLayoutGroupOrientation.Vertical;
            dashboardLayoutGroup1.Weight = 100D;
            this.LayoutRoot = dashboardLayoutGroup1;
            this.Title.Text = "Аналитическая панель";
            ((System.ComponentModel.ISupportInitialize)(measure1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(dimension1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SalesByCashiers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Receipts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(dimension2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(measure2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(measure3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PieSalesByCashiers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardItemGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardItemGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardItemGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardItemGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
        private PieDashboardItem PieSalesByCashiers;
        private CardDashboardItem SalesByCashiers;
        public DashboardObjectDataSource Receipts;
        private DashboardItemGroup dashboardItemGroup1;
        private DashboardItemGroup dashboardItemGroup2;
        private DashboardItemGroup dashboardItemGroup3;
        private DashboardItemGroup dashboardItemGroup4;
    }
}
