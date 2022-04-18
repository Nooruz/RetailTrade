using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RetailTradeClient.Customs
{
    public class UnitTextBox : TextBox
    {
        private FormattedText _unitText;
        private Rect _unitTextBounds;

        public static DependencyProperty UnitTextProperty =
            DependencyProperty.Register(
                name: "UnitText",
                propertyType: typeof(string),
                ownerType: typeof(UnitTextBox),
                typeMetadata: new FrameworkPropertyMetadata(
                    default(string),
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public string UnitText
        {
            get { return (string)GetValue(UnitTextProperty); }
            set { SetValue(UnitTextProperty, value); }
        }

        public static DependencyProperty UnitPaddingProperty =
            DependencyProperty.Register(
                name: "UnitPadding",
                propertyType: typeof(Thickness),
                ownerType: typeof(UnitTextBox),
                typeMetadata: new FrameworkPropertyMetadata(
                    new Thickness(5d, 0d, 0d, 0d),
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public Thickness UnitPadding
        {
            get { return (Thickness)GetValue(UnitPaddingProperty); }
            set { SetValue(UnitPaddingProperty, value); }
        }

        public static DependencyProperty TextBoxWidthProperty =
            DependencyProperty.Register(
                name: "TextBoxWidth",
                propertyType: typeof(double),
                ownerType: typeof(UnitTextBox),
                typeMetadata: new FrameworkPropertyMetadata(
                    double.NaN,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double TextBoxWidth
        {
            get { return (double)GetValue(TextBoxWidthProperty); }
            set { SetValue(TextBoxWidthProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == ForegroundProperty)
                EnsureUnitText(invalidate: true);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var textBoxWidth = this.TextBoxWidth;
            var unit = EnsureUnitText(invalidate: true);
            var padding = this.UnitPadding;

            if (unit != null)
            {
                var unitWidth = unit.Width + padding.Left + padding.Right;
                var unitHeight = unit.Height + padding.Top + padding.Bottom;

                constraint = new Size(
                    constraint.Width - unitWidth,
                    Math.Max(constraint.Height, unitHeight));
            }

            var hasFixedTextBoxWidth = !double.IsNaN(textBoxWidth) &&
                                       !double.IsInfinity(textBoxWidth);

            if (hasFixedTextBoxWidth)
                constraint = new Size(textBoxWidth, constraint.Height);

            var baseSize = base.MeasureOverride(constraint);
            var baseWidth = hasFixedTextBoxWidth ? textBoxWidth : baseSize.Width;

            if (unit != null)
            {
                var unitWidth = unit.Width + padding.Left + padding.Right;
                var unitHeight = unit.Height + padding.Top + padding.Bottom;

                return new Size(
                    baseWidth + unitWidth,
                    Math.Max(baseSize.Height, unitHeight));
            }

            return new Size(baseWidth, baseSize.Height);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var textSize = arrangeBounds;
            var unit = EnsureUnitText(invalidate: false);
            var padding = this.UnitPadding;

            if (unit != null)
            {
                var unitWidth = unit.Width + padding.Left + padding.Right;
                var unitHeight = unit.Height + padding.Top + padding.Bottom;

                textSize.Width -= unitWidth;

                _unitTextBounds = new Rect(
                    textSize.Width + padding.Left,
                    (arrangeBounds.Height - unitHeight) / 2 + padding.Top,
                    textSize.Width,
                    textSize.Height);
            }

            var baseSize = base.ArrangeOverride(textSize);

            if (unit != null)
            {
                var unitWidth = unit.Width + padding.Left + padding.Right;
                var unitHeight = unit.Height + padding.Top + padding.Bottom;

                return new Size(
                    baseSize.Width + unitWidth,
                    Math.Max(baseSize.Height, unitHeight));
            }

            return baseSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var unitText = EnsureUnitText(invalidate: false);
            if (unitText != null)
                drawingContext.DrawText(unitText, _unitTextBounds.Location);
        }

        private FormattedText EnsureUnitText(bool invalidate = false)
        {
            if (invalidate)
                _unitText = null;

            if (_unitText != null)
                return _unitText;

            var unit = UnitText;

            if (!string.IsNullOrEmpty(unit))
            {
                _unitText = new FormattedText(
                    unit,
                    CultureInfo.InvariantCulture,
                    FlowDirection,
                    new Typeface(
                        FontFamily,
                        FontStyle,
                        FontWeight,
                        FontStretch),
                    FontSize,
                    Foreground);
            }

            return _unitText;
        }
    }
}
