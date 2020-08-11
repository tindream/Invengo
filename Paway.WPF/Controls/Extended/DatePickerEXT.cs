﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// DatePicker扩展
    /// </summary>
    public partial class DatePickerEXT : DatePicker
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(DatePickerEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.RegisterAttached(nameof(IsReadOnly), typeof(bool), typeof(DatePickerEXT), new PropertyMetadata(true));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 只读属性
        /// </summary>
        [Category("扩展")]
        [Description("只读属性")]
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public DatePickerEXT() { }

        #region 扩展按钮
        private Popup popup;
        /// <summary>
        /// 响应扩展按钮
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            popup = this.GetTemplateChild("PART_Popup") as Popup;
            if (popup != null)
            {
                var calendar = popup.Child as Calendar;
                calendar.Loaded -= Calendar_Loaded;
                calendar.Loaded += Calendar_Loaded;
            }
        }
        private void Calendar_Loaded(object sender, RoutedEventArgs e)
        {
            if (Method.Child(sender, out CalendarItem item))
            {
                item.Loaded -= Item_Loaded;
                item.Loaded += Item_Loaded;
            }
        }
        private void Item_Loaded(object sender, RoutedEventArgs e)
        {
            if (Method.Child(sender, out ButtonEXT goToday, "PART_GoToday"))
            {
                goToday.Click -= GoToday_Click;
                goToday.Click += GoToday_Click;
            }
            if (Method.Child(sender, out ButtonEXT goClear, "PART_GoClear"))
            {
                goClear.Click -= GoClear_Click;
                goClear.Click += GoClear_Click;
            }
        }
        private void GoClear_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedDate = null;
            this.DisplayDate = DateTime.Now;
            popup.IsOpen = false;
        }
        private void GoToday_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedDate = DateTime.Now;
            this.DisplayDate = DateTime.Now;
            popup.IsOpen = false;
        }

        #endregion
    }
}
