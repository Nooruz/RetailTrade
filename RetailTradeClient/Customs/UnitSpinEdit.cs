﻿using DevExpress.Xpf.Editors;
using System.Windows;

namespace RetailTradeClient.Customs
{
    public class UnitSpinEdit : SpinEdit
    {
        #region Dependency Properties

        public static readonly DependencyProperty UnitNameProperty =
            DependencyProperty.Register(nameof(UnitName), typeof(string), typeof(UnitSpinEdit), new PropertyMetadata(string.Empty));

        #endregion

        #region Public Properties

        public string UnitName
        {
            get => (string)GetValue(UnitNameProperty);
            set => SetValue(UnitNameProperty, value);
        }

        #endregion
    }
}
