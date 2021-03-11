﻿using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 自定义默认、鼠标划过时、鼠标点击时的Brush颜色
    /// <para>需要反射实体对象，获取默认值，故注意无限循环，不可引用自身</para>
    /// </summary>
    [TypeConverter(typeof(BrushEXTConverter))]
    public class BrushEXT : ModelBase, IEquatable<BrushEXT>, IElementStatu<Brush>
    {
        private Brush normal = new SolidColorBrush(Colors.LightGray);
        /// <summary>
        /// 默认的颜色
        /// </summary>
        public Brush Normal { get { return normal; } set { normal = value; OnPropertyChanged(); } }
        private Brush mouse = new SolidColorBrush(TMethod.ThemeColor(210));
        /// <summary>
        /// 鼠标划过时的颜色
        /// </summary>
        public Brush Mouse { get { return mouse; } set { mouse = value; OnPropertyChanged(); } }
        private Brush pressed = new SolidColorBrush(TMethod.ThemeColor(210));
        /// <summary>
        /// 鼠标点击时的颜色
        /// </summary>
        public Brush Pressed { get { return pressed; } set { pressed = value; OnPropertyChanged(); } }
        /// <summary>
        /// 鼠标划过时的选中颜色
        /// </summary>
        public Brush PressedMouse
        {
            get
            {
                var pressed = Pressed as SolidColorBrush;
                return new SolidColorBrush(TMethod.AlphaColor(pressed.Color.A - Alpha / 2, pressed.Color));
            }
        }
        /// <summary>
        /// 颜色Alpha值变量
        /// </summary>
        public int Alpha { get; set; } = 50;
        internal bool IHigh { get; set; }

        /// <summary>
        /// </summary>
        public BrushEXT()
        {
            TConfig.ColorChanged += Config_ColorChanged;
        }
        private void Config_ColorChanged(Color obj)
        {
            if (this.Normal is SolidColorBrush normal && normal.Color.R == obj.R && normal.Color.G == obj.G && normal.Color.B == obj.B)
            {
                if (normal.Color != Colors.Transparent && normal.Color != Colors.Gray && normal.Color != Colors.LightGray && normal.Color != Color.FromArgb(255, 33, 33, 33) && normal.Color != Color.FromArgb(255, 66, 66, 66))
                {
                    this.Normal = new SolidColorBrush(TMethod.ThemeColor(normal.Color.A));
                }
            }
            if (this.Mouse is SolidColorBrush mouse && mouse.Color.R == obj.R && mouse.Color.G == obj.G && mouse.Color.B == obj.B)
            {
                if (mouse.Color != Colors.Transparent && mouse.Color != Colors.White && mouse.Color != Color.FromArgb(255, 33, 33, 33))
                {
                    this.Mouse = new SolidColorBrush(TMethod.ThemeColor(mouse.Color.A));
                }
            }
            if (this.Pressed is SolidColorBrush pressed && pressed.Color.R == obj.R && pressed.Color.G == obj.G && pressed.Color.B == obj.B)
            {
                if (pressed.Color != Colors.Transparent && pressed.Color != Colors.White)
                {
                    this.Pressed = new SolidColorBrush(TMethod.ThemeColor(pressed.Color.A));
                }
            }
            High();
        }
        /// <summary>
        /// 主题深色
        /// </summary>
        private void High()
        {
            if (IHigh)
            {
                var alpha = this.Normal is SolidColorBrush normal ? normal.Color.A : (byte)250;
                this.Normal = new SolidColorBrush(TMethod.AlphaColor(alpha, TConfig.Color.AddLight(0.96)));
                alpha = this.Mouse is SolidColorBrush mouse ? mouse.Color.A : (byte)250;
                this.Mouse = new SolidColorBrush(TMethod.AlphaColor(alpha, TConfig.Color.AddLight(0.96)));
                this.Pressed = new SolidColorBrush(TConfig.Color.AddLight(-90));
            }
        }
        /// <summary>
        /// 主题色：设置所有颜色，自动Alpha变量
        /// </summary>
        public BrushEXT(byte normal) : this(TMethod.ThemeColor(normal)) { }
        /// <summary>
        /// 主题色：设置所有颜色，普通、鼠标移过、按下
        /// </summary>
        public BrushEXT(byte normal, byte mouse, byte pressed, bool iHigh = false) : this(TMethod.ThemeColor(normal), mouse, pressed)
        {
            this.IHigh = iHigh;
            High();
        }
        /// <summary>
        /// 主题色：普通、鼠标移过、按下
        /// </summary>
        public BrushEXT(Color? normal, byte mouse, byte pressed) : this(normal, TMethod.ThemeColor(mouse), TMethod.ThemeColor(pressed)) { }
        /// <summary>
        /// </summary>
        public BrushEXT(Color? normal, Color? mouse = null, Color? pressed = null, int? alpha = null, BrushEXT value = null) : this()
        {
            if (alpha != null) Alpha = alpha.Value;
            else if (value != null) Alpha = value.Alpha;

            if (normal != null) Normal = new SolidColorBrush(normal.Value);
            else if (value != null) Normal = value.Normal;

            if (mouse != null) Mouse = new SolidColorBrush(mouse.Value);
            else if (normal != null) Reset(normal.Value, Alpha);
            else if (value != null) Mouse = value.Mouse;

            if (pressed != null) Pressed = new SolidColorBrush(pressed.Value);
            else if (mouse != null) Focused(mouse.Value, Alpha);
            else if (normal == null && value != null) Pressed = value.Pressed;
        }
        /// <summary>
        /// 设置所有颜色，指定Alpha变量
        /// </summary>
        private BrushEXT Reset(Color color, int alpha)
        {
            Normal = new SolidColorBrush(color);
            Mouse = new SolidColorBrush(TMethod.AlphaColor(color.A - alpha, color));
            Pressed = new SolidColorBrush(TMethod.AlphaColor(color.A + alpha, color));
            return this;
        }
        /// <summary>
        /// 设置鼠标划过、点击时的颜色
        /// </summary>
        private BrushEXT Focused(Color color, int alpha)
        {
            Mouse = new SolidColorBrush(color);
            Pressed = new SolidColorBrush(TMethod.AlphaColor(color.A + alpha, color));
            return this;
        }
        /// <summary>
        /// </summary>
        public bool Equals(BrushEXT other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed) && Alpha.Equals(other.Alpha);
        }
    }
    /// <summary>
    /// 字符串转BrushEXT
    /// </summary>
    internal class BrushEXTConverter : TypeConverter
    {
        /// <summary>
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        /// <summary>
        /// </summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        /// <summary>
        /// 自定义字符串转换，以分号(;)隔开
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                throw base.GetConvertFromException(value);
            }
            if (value is string str)
            {
                var result = TMethod.ElementStatu<BrushEXT, Color>(context, culture, str, Parse, ParseValue);
                return new BrushEXT(result.Item2, result.Item3, result.Item4, result.Item5, result.Item1);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private Color? ParseValue(BrushEXT old, string name)
        {
            if (old == null) return null;
            return (old.GetValue(name) as SolidColorBrush).Color;
        }
        private Color Parse(string str)
        {
            if (byte.TryParse(str, out byte alpha))
            {
                return TMethod.ThemeColor(alpha);
            }
            return (Color)ColorConverter.ConvertFromString(str);
        }
        /// <summary>
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);
            return value.ToString();
        }
    }
}
