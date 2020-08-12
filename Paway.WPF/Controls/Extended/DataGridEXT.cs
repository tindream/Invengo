﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// DataGrid扩展
    /// </summary>
    public partial class DataGridEXT : DataGrid
    {
        #region 扩展
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(DataGridEXT),
                new PropertyMetadata(new BrushEXT(Color.FromArgb(120, Config.Color.R, Config.Color.G, Config.Color.B))));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty HeaderStartProperty =
            DependencyProperty.RegisterAttached(nameof(HeaderStart), typeof(ColorEXT), typeof(DataGridEXT),
                new PropertyMetadata(new ColorEXT(Color.FromArgb(255, 254, 254, 254), Color.FromArgb(85, Config.Color.R, Config.Color.G, Config.Color.B))));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty HeaderEndProperty =
            DependencyProperty.RegisterAttached(nameof(HeaderEnd), typeof(ColorEXT), typeof(DataGridEXT),
                new PropertyMetadata(new ColorEXT(Color.FromArgb(255, 220, 220, 220), Color.FromArgb(170, Config.Color.R, Config.Color.G, Config.Color.B))));

        /// <summary>
        /// 自定义项背景色
        /// </summary>
        [Category("扩展")]
        [Description("自定义项背景色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 标题列背景开始颜色
        /// </summary>
        [Category("扩展")]
        [Description("标题列背景开始颜色")]
        public ColorEXT HeaderStart
        {
            get { return (ColorEXT)GetValue(HeaderStartProperty); }
            set { SetValue(HeaderStartProperty, value); }
        }
        /// <summary>
        /// 标题列背景结束颜色
        /// </summary>
        [Category("扩展")]
        [Description("标题列背景结束颜色")]
        public ColorEXT HeaderEnd
        {
            get { return (ColorEXT)GetValue(HeaderEndProperty); }
            set { SetValue(HeaderEndProperty, value); }
        }

        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        public DataGridEXT()
        {
            //AutoGenerateColumns = true;
            //this.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
            //this.ColumnHeaderHeight = 42;
        }

        #region 自定义排序
        /// <summary>
        /// 自定义排序-字符串
        /// </summary>
        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            var column = eventArgs.Column;
            //use a ListCollectionView to do the sort.
            var source = CollectionViewSource.GetDefaultView(this.ItemsSource);
            var property = this.type.Property(column.SortMemberPath);
            if (source is ListCollectionView list && property.PropertyType.Name == nameof(String))
            {
                //i do some custom checking based on column to get the right comparer
                //i have different comparers for different columns. I also handle the sort direction
                //in my comparer

                // prevent the built-in sort from sorting
                //eventArgs.Handled = true;

                var direction = (column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
                //set the sort order on the column
                column.SortDirection = direction;

                //this is my custom sorter it just derives from IComparer and has a few properties
                //you could just apply the comparer but i needed to do a few extra bits and pieces
                var comparer = new StringComparer(column.SortMemberPath, column.SortDirection == ListSortDirection.Ascending);

                //apply the sort
                list.CustomSort = comparer;
            }
            else
            {
                base.OnSorting(eventArgs);
            }
        }
        /// <summary>
        /// 字符串比较
        /// </summary>
        internal class StringComparer : IComparer
        {
            private readonly string Name;
            private readonly int Comparer = 1;
            public StringComparer(string name, bool asc)
            {
                this.Name = name;
                this.Comparer = asc ? 1 : -1;
            }
            /// <summary>
            /// 重写字符串比较方法
            /// </summary>
            public int Compare(object x, object y)
            {
                return Comparer * (x.GetValue(Name) as string).Compare(y.GetValue(Name) as string);
            }
        }

        #endregion

        #region 绑定数据，自动列
        /// <summary>
        /// 当前绑定的数据类型
        /// </summary>
        private Type type;
        /// <summary>
        /// 外部自定义列
        /// </summary>
        private readonly List<DataGridColumn> columnsReady = new List<DataGridColumn>();
        /// <summary>
        /// 获取或设置用于生成 System.Windows.Controls.ItemsControl 的内容的集合。
        /// <para>重载数据绑定</para>
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IEnumerable ItemsSource
        {
            get { return base.ItemsSource; }
            set
            {
                base.ItemsSource = value;
                LoadColumns();
            }
        }
        /// <summary>
        /// 初始化时缓存自定义列样式
        /// </summary>
        protected override void OnInitialized(EventArgs e)
        {
            columnsReady.AddRange(this.Columns);
            base.OnInitialized(e);
        }
        /// <summary>
        /// 加载列
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (base.ItemsSource != null)
            {
                this.type = base.ItemsSource.GetType().GenericType();
                LoadColumns();
            }
        }
        private void LoadColumns()
        {
            if (base.ItemsSource is PagedCollectionView paged)
            {
                this.type = paged.SourceCollection.GetType().GenericType();
            }
            else
            {
                this.type = base.ItemsSource.GetType().GenericType();
            }
            var columns = new List<DataGridColumn>();
            var properties = this.type.PropertiesCache();
            foreach (var property in properties)
            {
                var column = columnsReady.Find(c => (c.ClipboardContentBinding is Binding binding && binding.Path.Path == property.Name) || c.Header.ToStrs() == property.Name || c.Header.ToStrs() == property.Text());
                if (column != null)
                {
                    columns.Add(column);
                }
                else
                {
                    column = new DataGridTextColumn { Binding = new Binding(property.Name) };
                    columns.Add(column);
                }
                column.MinWidth = 64;
                column.Header = property.Text();
                if (column is DataGridTextColumn text && text.ElementStyle.Setters.Count == 1 && (text.ElementStyle.Setters[0] as Setter).Property.Name == "Margin")
                {
                    text.ElementStyle = (Style)FindResource("TextLeft");
                }
                column.Visibility = property.IShow() ? Visibility.Visible : Visibility.Collapsed;
            }
            this.Columns.Clear();
            foreach (var column in columns) this.Columns.Add(column);
        }

        #endregion

        #region 扩展公共方法
        /// <summary>
        /// 获取指定名称列
        /// </summary>
        public DataGridColumn GetColumn(string name)
        {
            foreach (var item in Columns)
            {
                if (item.ClipboardContentBinding is Binding binding && binding.Path.Path == name)
                {
                    return item;
                }
            }
            return new DataGridTextColumn();
        }

        #endregion
    }
}
