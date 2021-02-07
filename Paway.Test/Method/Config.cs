﻿using log4net;
using Paway.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Paway.Test
{
    public class Config : Paway.WPF.Config
    {
        #region 常量
        public const string Text = "测试系统";

        #endregion

        #region 全局数据
        public static Window Window { get; set; }
        public static AdminInfo Admin { get; set; }
        public static AuthInfo Auth { get; set; } = new AuthInfo();

        #endregion
    }
}