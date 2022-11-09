using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;

namespace Paway.Model
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        public StatuItemModel StatuItem => GetInstance<StatuItemModel>();
        public NameWindowModel Name => GetInstance<NameWindowModel>();
        public WelcomePageModel Welcome => GetInstance<WelcomePageModel>();

        /// <summary>
        /// 单实例模型已注册列表
        /// </summary>
        private static readonly List<string> viewModelList = new List<string>();
        protected T GetInstance<T>(string key = null) where T : class
        {
            var name = typeof(T).FullName;
            if (!viewModelList.Contains(name))
            {
                SimpleIoc.Default.Register<T>();
                viewModelList.Add(name);
            }
            return ServiceLocator.Current.GetInstance<T>(key);
        }

        /// <summary>
        /// 单实例视图列表
        /// </summary>
        private static readonly Dictionary<string, dynamic> dicView = new Dictionary<string, dynamic>();
        public T GetViewInstance<T>()
        {
            var name = typeof(T).FullName;
            if (!dicView.ContainsKey(name))
            {
                var obj = Activator.CreateInstance<T>();
                dicView.Add(name, obj);
            }
            return dicView[name];
        }
    }
}