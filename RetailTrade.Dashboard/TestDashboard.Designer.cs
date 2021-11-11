
namespace RetailTrade.Dashboard
{
    partial class TestDashboard
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
            DevExpress.DashboardCommon.Dimension dimension1 = new DevExpress.DashboardCommon.Dimension();
            DevExpress.DashboardCommon.Measure measure1 = new DevExpress.DashboardCommon.Measure();
            DevExpress.DashboardCommon.Measure measure2 = new DevExpress.DashboardCommon.Measure();
            DevExpress.DashboardCommon.Measure measure3 = new DevExpress.DashboardCommon.Measure();
            DevExpress.DashboardCommon.Card card1 = new DevExpress.DashboardCommon.Card();
            DevExpress.DashboardCommon.CardStretchedLayoutTemplate cardStretchedLayoutTemplate1 = new DevExpress.DashboardCommon.CardStretchedLayoutTemplate();
            DevExpress.DashboardCommon.Dimension dimension2 = new DevExpress.DashboardCommon.Dimension();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup1 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem1 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem2 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            this.Receipts = new DevExpress.DashboardCommon.DashboardObjectDataSource();
            this.pieDashboardItem1 = new DevExpress.DashboardCommon.PieDashboardItem();
            this.cardDashboardItem1 = new DevExpress.DashboardCommon.CardDashboardItem();
            ((System.ComponentModel.ISupportInitialize)(this.Receipts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pieDashboardItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dimension1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(measure1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(measure2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardDashboardItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(measure3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dimension2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Receipts
            // 
            this.Receipts.ComponentName = "Receipts";
            this.Receipts.DataSource = typeof(RetailTrade.Dashboard.Receipts);
            this.Receipts.Name = "Чеки";
            // 
            // pieDashboardItem1
            // 
            this.pieDashboardItem1.ComponentName = "pieDashboardItem1";
            dimension1.DataMember = "Shifts.Users.FullName";
            measure1.DataMember = "PaidInCash";
            measure1.Name = "Наличными";
            measure2.DataMember = "PaidInCashless";
            measure2.Name = "Безналичными";
            this.pieDashboardItem1.DataItemRepository.Clear();
            this.pieDashboardItem1.DataItemRepository.Add(dimension1, "DataItem0");
            this.pieDashboardItem1.DataItemRepository.Add(measure1, "DataItem1");
            this.pieDashboardItem1.DataItemRepository.Add(measure2, "DataItem2");
            this.pieDashboardItem1.DataSource = this.Receipts;
            this.pieDashboardItem1.InteractivityOptions.IgnoreMasterFilters = false;
            this.pieDashboardItem1.Name = "Диаграммы 1";
            this.pieDashboardItem1.SeriesDimensions.AddRange(new DevExpress.DashboardCommon.Dimension[] {
            dimension1});
            this.pieDashboardItem1.ShowCaption = true;
            this.pieDashboardItem1.Values.AddRange(new DevExpress.DashboardCommon.Measure[] {
            measure1,
            measure2});
            // 
            // cardDashboardItem1
            // 
            measure3.DataMember = "Sum";
            measure3.Name = "Сумма продажи";
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
            card1.AddDataItem("ActualValue", measure3);
            this.cardDashboardItem1.Cards.AddRange(new DevExpress.DashboardCommon.Card[] {
            card1});
            this.cardDashboardItem1.ComponentName = "cardDashboardItem1";
            dimension2.DataMember = "Shifts.Users.FullName";
            dimension2.Name = "Кассир";
            this.cardDashboardItem1.DataItemRepository.Clear();
            this.cardDashboardItem1.DataItemRepository.Add(dimension2, "DataItem0");
            this.cardDashboardItem1.DataItemRepository.Add(measure3, "DataItem1");
            this.cardDashboardItem1.DataSource = this.Receipts;
            this.cardDashboardItem1.InteractivityOptions.IgnoreMasterFilters = false;
            this.cardDashboardItem1.Name = "Карточки 1";
            this.cardDashboardItem1.SeriesDimensions.AddRange(new DevExpress.DashboardCommon.Dimension[] {
            dimension2});
            this.cardDashboardItem1.ShowCaption = true;
            // 
            // TestDashboard
            // 
            this.DataSources.AddRange(new DevExpress.DashboardCommon.IDashboardDataSource[] {
            this.Receipts});
            this.Items.AddRange(new DevExpress.DashboardCommon.DashboardItem[] {
            this.pieDashboardItem1,
            this.cardDashboardItem1});
            dashboardLayoutItem1.DashboardItem = this.cardDashboardItem1;
            dashboardLayoutItem1.Weight = 50D;
            dashboardLayoutItem2.DashboardItem = this.pieDashboardItem1;
            dashboardLayoutItem2.Weight = 50D;
            dashboardLayoutGroup1.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutItem1,
            dashboardLayoutItem2});
            dashboardLayoutGroup1.DashboardItem = null;
            dashboardLayoutGroup1.Orientation = DevExpress.DashboardCommon.DashboardLayoutGroupOrientation.Vertical;
            dashboardLayoutGroup1.Weight = 100D;
            this.LayoutRoot = dashboardLayoutGroup1;
            this.Title.Text = "Аналитическая панель";
            ((System.ComponentModel.ISupportInitialize)(this.Receipts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(dimension1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(measure1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(measure2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pieDashboardItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(measure3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(dimension2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardDashboardItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.DashboardCommon.DashboardObjectDataSource Receipts;
        private DevExpress.DashboardCommon.PieDashboardItem pieDashboardItem1;
        private DevExpress.DashboardCommon.CardDashboardItem cardDashboardItem1;
    }
}
