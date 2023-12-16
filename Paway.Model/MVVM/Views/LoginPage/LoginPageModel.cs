using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 登陆基类，必须实现登陆方法
    /// </summary>
    public abstract class LoginPageModel : ViewModelBasePlus
    {
        #region 属性
        /// <summary>
        /// 根控件
        /// </summary>
        protected DependencyObject Root;
        /// <summary>
        /// 数据绑定列表
        /// </summary>
        public ObservableCollection<object> ObList { get; private set; } = new ObservableCollection<object>();
        /// <summary>
        /// 登陆锁
        /// </summary>
        private volatile bool iLogining;
        /// <summary>
        /// 过滤锁
        /// </summary>
        private volatile bool iFiltering;
        private string _userName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    if (iFiltering) return;
                    try
                    {
                        iFiltering = true;
                        FilterUser(value);
                    }
                    finally
                    {
                        iFiltering = false;
                    }
                    _userName = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 用户列表过滤
        /// </summary>
        protected virtual void FilterUser(string userName) { }

        private string _password = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private bool _iUserList;
        /// <summary>
        /// 显示用户列表标记
        /// </summary>
        public bool IUserList
        {
            get { return _iUserList; }
            set { _iUserList = value; OnPropertyChanged(); }
        }

        private bool _iAuto;
        /// <summary>
        /// 自动登陆标记
        /// </summary>
        public bool IAuto
        {
            get { return _iAuto; }
            set { _iAuto = value; OnPropertyChanged(); }
        }

        private string welcome = "欢迎使用";
        /// <summary>
        /// 欢迎谗
        /// </summary>
        public string Welcome
        {
            get { return welcome; }
            set { welcome = value; OnPropertyChanged(); }
        }
        private ImageSource _logoImage;
        /// <summary>
        /// Logo
        /// </summary>
        public ImageSource LogoImage
        {
            get { return _logoImage; }
            set { _logoImage = value; OnPropertyChanged(); }
        }

        private bool _iSetting;
        /// <summary>
        /// 显示设置按钮标记
        /// </summary>
        public bool ISetting
        {
            get { return _iSetting; }
            set { _iSetting = value; OnPropertyChanged(); }
        }

        private bool _iClose;
        /// <summary>
        /// 显示关闭按钮标记
        /// </summary>
        public bool IClose
        {
            get { return _iClose; }
            set { _iClose = value; OnPropertyChanged(); }
        }

        #endregion

        #region 命令
        /// <summary>
        /// 密码检查
        /// </summary>
        protected bool CheckPad()
        {
            if (Password.IsEmpty())
            {
                PMethod.Hit(Root, "请输入密码");
                if (PMethod.Find(Root, out PasswordBox tbPassword, "tbPassword")) tbPassword.Focus();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 登陆命令
        /// </summary>
        public abstract void Login();
        /// <summary>
        /// 显示设置页
        /// </summary>
        protected virtual Window SetWindow() { return null; }
        /// <summary>
        /// 设置操作
        /// </summary>
        protected virtual void OnSet(DependencyObject obj) { }
        /// <summary>
        /// 关闭操作
        /// </summary>
        protected virtual void OnClose(DependencyObject obj) { }
        /// <summary>
        /// 通用动作命令
        /// </summary>
        protected override void Action(string item)
        {
            base.Action(item);
            switch (item)
            {
                case "登陆":
                    if (iLogining) break;
                    try
                    {
                        iLogining = true;
                        if (UserName.IsEmpty())
                        {
                            PMethod.Hit(Root, "请输入用户名");
                            if (IUserList)
                            {
                                if (PMethod.Find(Root, out ComboBoxEXT cbxUserName, "cbxUserName")) cbxUserName.Focus();
                            }
                            else
                            {
                                if (PMethod.Find(Root, out TextBoxEXT tbUserName, "tbUserName")) tbUserName.Focus();
                            }
                            return;
                        }
                        Login();
                    }
                    finally
                    {
                        iLogining = false;
                    }
                    break;
                case "设置":
                    var window = SetWindow();
                    if (window != null && PMethod.ShowWindow(Root, window) == true)
                    {
                        OnSet(Root);
                    }
                    break;
                case "关闭": OnClose(Root); break;
            }
        }

        #endregion

        /// <summary>
        /// 登陆页模型
        /// </summary>
        public LoginPageModel()
        {
            Messenger.Default.Register<LoginLoadMessage>(this, msg => this.Root = msg.Obj);
        }
    }
}