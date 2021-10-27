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
    /// CheckBox扩展-按钮样式
    /// </summary>
    public partial class CheckButton : CheckBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(CheckButton), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBorderProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBorder), typeof(ThicknessEXT), typeof(CheckButton), new PropertyMetadata(new ThicknessEXT(1, 0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(CheckButton),
                new PropertyMetadata(new BrushEXT(Colors.LightGray, PConfig.Alpha - PConfig.Interval, PConfig.Alpha + PConfig.Interval)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemForeground), typeof(BrushEXT), typeof(CheckButton),
            new PropertyMetadata(new BrushEXT(PConfig.TextColor, Colors.White, Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.RegisterAttached(nameof(Type), typeof(ColorType), typeof(CheckButton),
            new UIPropertyMetadata(ColorType.Normal, OnColorTypeChanged));
        private static void OnColorTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is CheckButton cbxBtn)
            {
                if (cbxBtn.Type != ColorType.None)
                {
                    var color = cbxBtn.Type.Color();
                    cbxBtn.ItemBrush = new BrushEXT(PMethod.AlphaColor(PConfig.Alpha, color));
                    cbxBtn.ItemBrush.Normal = new SolidColorBrush(Colors.LightGray);
                }
                cbxBtn.UpdateDefaultStyle();
            }
        }

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
        /// 自定义边框线
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框线")]
        public ThicknessEXT ItemBorder
        {
            get { return (ThicknessEXT)GetValue(ItemBorderProperty); }
            set { SetValue(ItemBorderProperty, value); }
        }
        /// <summary>
        /// 自定义文本颜色
        /// </summary>
        [Category("扩展")]
        [Description("自定义文本颜色")]
        public BrushEXT ItemForeground
        {
            get { return (BrushEXT)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 颜色样式
        /// </summary>
        [Category("扩展.颜色样式")]
        [Description("颜色样式")]
        public ColorType Type
        {
            get { return (ColorType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public CheckButton()
        {
            DefaultStyleKey = typeof(CheckButton);
        }
    }
}