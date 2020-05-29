﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ProgressBar扩展
    /// </summary>
    public partial class ProgressBarEXT : ProgressBar
    {
        #region 启用监听，获取进度
        /// <summary>
        /// 启用监听，获取进度
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(ProgressBarEXT), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ProgressBarEXT bar)
            {
                bar.LayoutUpdated += delegate
                {
                    var value = $"{bar.Value * 100 / bar.Maximum:F0}%";
                    if (bar.ProgressValue != value) bar.SetValue(ProgressValueProperty, value);
                };
                if ((bool)e.NewValue)
                {
                    bar.ValueChanged += Bar_ValueChanged;
                }
                else
                {
                    bar.ValueChanged -= Bar_ValueChanged;
                }
            }
        }
        private static void Bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is ProgressBar bar)
            {
                bar.SetValue(ProgressValueProperty, $"{bar.Value * 100 / bar.Maximum:F0}%");
            }
        }
        /// <summary>
        /// 启用监听
        /// </summary>
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }

        #endregion

        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ProgressBarEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ForegroundStartColorProperty =
            DependencyProperty.RegisterAttached(nameof(ForegroundStartColor), typeof(Color), typeof(ProgressBarEXT),
            new PropertyMetadata(Color.FromArgb(85, 35, 175, 255)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ForegroundEndColorProperty =
            DependencyProperty.RegisterAttached(nameof(ForegroundEndColor), typeof(Color), typeof(ProgressBarEXT),
            new PropertyMetadata(Color.FromArgb(250, 35, 175, 255)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.RegisterAttached(nameof(ProgressValue), typeof(string), typeof(ProgressBarEXT));

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
        /// 进度条起始颜色
        /// </summary>
        [Category("扩展")]
        [Description("进度条起始颜色")]
        public Color ForegroundStartColor
        {
            get { return (Color)GetValue(ForegroundStartColorProperty); }
            set { SetValue(ForegroundStartColorProperty, value); }
        }
        /// <summary>
        /// 进度条终点颜色
        /// </summary>
        [Category("扩展")]
        [Description("进度条终点颜色")]
        public Color ForegroundEndColor
        {
            get { return (Color)GetValue(ForegroundEndColorProperty); }
            set { SetValue(ForegroundEndColorProperty, value); }
        }
        /// <summary>
        /// 进度条显示文本
        /// </summary>
        [Category("扩展")]
        [Description("进度条显示文本")]
        public string ProgressValue
        {
            get { return (string)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ProgressBarEXT()
        {
            DefaultStyleKey = typeof(ProgressBarEXT);
        }
    }
}
