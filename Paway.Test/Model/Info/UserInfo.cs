﻿using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    [Serializable]
    [Table("Users")]
    public class UserInfo : ModelBase, IName
    {
        private string name;
        [Text("用户名")]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        [NoShow]
        public string Pad { get; set; }

        [NoShow]
        public bool Statu { get; set; }

        [NoShow]
        public UserType UserType { get; set; }

        [Text("最后登陆")]
        public DateTime LoginOn { get; set; }

        [NoShow]
        public DateTime CreateOn { get; set; }

        public override string ToString()
        {
            return $"{this.name}";
        }
        public UserInfo()
        {
            this.Statu = true;
            this.CreateOn = DateTime.Now;
        }
    }
}