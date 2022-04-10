namespace SalePageServer.Report
{
    public partial class LabelReport
    {
        public LabelReport()
        {
            InitializeComponent();
        }

        public void ChangeSize(int width, int height)
        {
            PageWidth = width;
            PageHeight = height;
            barCode1.WidthF = ToFloat(width * 0.8913);
            lbName.WidthF = ToFloat(width * 0.8913);
            barCode1.HeightF = ToFloat(height * 0.52);
        }

        private float ToFloat(double value)
        {
            return (float)value;
        }
    }
}
