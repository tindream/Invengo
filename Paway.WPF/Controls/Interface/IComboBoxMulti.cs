﻿using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ComboBoxMulti数接口据定义
    /// </summary>
    public interface IComboBoxMulti
    {
        /// <summary>
        /// 标识符
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// 选中标记
        /// </summary>
        bool IsChecked { get; set; }
    }
    /// <summary>
    /// IComboBoxMulti数据模型
    /// </summary>
    public class ComboBoxMultiModel : ModelBase, IComboBoxMulti
    {
        private int id;
        /// <summary>
        /// 标识符
        /// </summary>
        [NoShow]
        public virtual int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        private string text;
        /// <summary>
        /// 文本
        /// </summary>
        public virtual string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged(); }
        }
        private bool isChecked;
        /// <summary>
        /// 选中标记
        /// </summary>
        public virtual bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// </summary>
        public ComboBoxMultiModel()
        {
            this.Id = this.GetHashCode();
        }
        /// <summary>
        /// </summary>
        public ComboBoxMultiModel(string text) : this()
        {
            this.Text = text;
        }
    }
}