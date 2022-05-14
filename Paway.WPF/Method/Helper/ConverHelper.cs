﻿using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ConverHelper
    {
        #region Labbda表达式
        /// <summary>
        /// 自动生成泛型谓词条件
        /// </summary>
        public static Func<T, bool> Predicate<T>(this ObservableCollection<DataGridColumn> columns, string value, Func<T, bool> action = null)
        {
            return typeof(T).Predicate(columns, value, action);
        }
        /// <summary>
        /// 自动生成泛型谓词条件
        /// </summary>
        public static Func<dynamic, bool> Predicate<dynamic>(this Type type, ObservableCollection<DataGridColumn> columns, string value, Func<dynamic, bool> action = null)
        {
            var list = new List<string>();
            foreach (var column in columns)
            {
                if (column.Visibility != Visibility.Visible) continue;
                var name = column.Header.ToStrings();
                if (column.ClipboardContentBinding is Binding binding)
                {
                    name = binding.Path.Path;
                }
                list.Add(name);
            }
            return type.Predicate<dynamic>(list, value, action);
        }

        #endregion

        #region Color
        /// <summary>
        /// 颜色转换(Add)
        /// </summary>
        public static Color AddLight(this Color color, int value)
        {
            var result = RGBToHSL(color.R / 255.0, color.G / 255.0, color.B / 255.0);
            var l = result[2] + value * 1.0 / 240;
            return HSLToRGB(result[0], result[1], l);
        }
        /// <summary>
        /// 颜色转换(Mult)
        /// </summary>
        public static Color AddLight(this Color color, double value)
        {
            var result = RGBToHSL(color.R / 255.0, color.G / 255.0, color.B / 255.0);
            return HSLToRGB(result[0], result[1], value);
        }
        /// <summary>
        /// RGB空间到HSL空间的转换
        /// 色调-饱和度-亮度(HSB) 转 Color
        /// </summary>
        private static Color HSLToRGB(double h, double s, double l)
        {
            if (l < 0) l = 0;
            if (l > 1) l = 1;
            double R, G, B;
            double var1, var2;
            if (s == 0) //HSL values = 0 ÷ 1
            {
                R = l * 255.0; //RGB results = 0 ÷ 255
                G = l * 255.0;
                B = l * 255.0;
            }
            else
            {
                if (l < 0.5) var2 = l * (1 + s);
                else var2 = l + s - s * l;

                var1 = 2.0 * l - var2;

                R = 255.0 * Hue2RGB(var1, var2, h + 1.0 / 3.0);
                G = 255.0 * Hue2RGB(var1, var2, h);
                B = 255.0 * Hue2RGB(var1, var2, h - 1.0 / 3.0);
            }

            var r = (byte)R.ToInt();
            var g = (byte)G.ToInt();
            var b = (byte)B.ToInt();
            return System.Windows.Media.Color.FromArgb(255, r, g, b);
        }
        private static double Hue2RGB(double v1, double v2, double vH)
        {
            if (vH < 0) vH += 1;
            if (vH > 1) vH -= 1;
            if (6.0 * vH < 1) return v1 + (v2 - v1) * 6.0 * vH;
            if (2.0 * vH < 1) return v2;
            if (3.0 * vH < 2) return v1 + (v2 - v1) * (2.0 / 3.0 - vH) * 6.0;
            return v1;
        }
        /// <summary>
        /// RGB空间到HSL空间的转换
        /// Color 转 色调-饱和度-亮度(HSB)
        /// </summary>
        internal static double[] RGBToHSL(double r, double g, double b)
        {
            double Max, Min, delR, delG, delB, delMax;

            Min = Math.Min(r, Math.Min(g, b)); //Min. value of RGB
            Max = Math.Max(r, Math.Max(g, b)); //Max. value of RGB
            delMax = Max - Min; //Delta RGB value

            var L = (Max + Min) / 2.0;
            double H = 0, S;
            if (delMax == 0) //This is a gray, no chroma...
            {
                //H = 2.0/3.0;          //Windows下S值为0时，H值始终为160（2/3*240）
                H = 0; //HSL results = 0 ÷ 1
                S = 0;
            }
            else //Chromatic data...
            {
                if (L < 0.5) S = delMax / (Max + Min);
                else S = delMax / (2 - Max - Min);

                delR = ((Max - r) / 6.0 + delMax / 2.0) / delMax;
                delG = ((Max - g) / 6.0 + delMax / 2.0) / delMax;
                delB = ((Max - b) / 6.0 + delMax / 2.0) / delMax;

                if (r == Max) H = delB - delG;
                else if (g == Max) H = 1.0 / 3.0 + delR - delB;
                else if (b == Max) H = 2.0 / 3.0 + delG - delR;

                if (H < 0) H += 1;
                if (H > 1) H -= 1;
            }

            var HSL = new double[3] { H, S, L };
            return HSL;
        }

        #endregion

        #region 特性
        /// <summary>
        /// 特性-枚举颜色
        /// </summary>
        public static Color Color(this ColorType type)
        {
            switch (type)
            {
                case ColorType.Color: return PConfig.Color;
                case ColorType.High: return PConfig.High;

                case ColorType.Success: return PConfig.Success;
                case ColorType.Warn: return PConfig.Warn;
                case ColorType.Error: return PConfig.Error;

                case ColorType.Text: return PConfig.TextColor;
                case ColorType.TextSub: return PConfig.TextSub;
                case ColorType.TextLight: return PConfig.TextLight;

                case ColorType.Light: return PConfig.Light;

                default: return Colors.Transparent;
            }
        }
        /// <summary>
        /// 特性-枚举颜色
        /// </summary>
        public static Color Color(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(ColorAttribute), false) as ColorAttribute[];
            if (list.Length == 1)
            {
                return list[0].Color;
            }
            return Colors.Transparent;
        }
        /// <summary>
        /// 获取枚举颜色
        /// </summary>
        public static Color Color(this Enum e)
        {
            if (e == null) return Colors.Transparent;
            var value = e.ToString();
            foreach (var field in e.GetType().GetFields(TConfig.Flags))
            {
                if (value == field.Name)
                {
                    return field.Color();
                }
            }
            return Colors.Transparent;
        }

        #endregion
    }
}