﻿using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            tbPad.KeyDown += TbPad_KeyDown;
        }
        private void TbPad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.DataContext is LoginPageModel login) login.Action("登陆");
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Messenger.Default.Send(new LoginLoadMessage() { Obj = Root });
            Method.BeginInvoke(this, () => { cbxUserName.Focus(); });
        }
    }
}
