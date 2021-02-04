﻿using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.Test.ViewModel;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowEXT
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Messenger.Default.Send(new StatuMessage(TConfig.Loading));
            Method.Progress(this, () =>
            {
                DataService.Default.Load();
            }, () =>
            {
                Messenger.Default.Send(new StatuMessage("加载完成"));
            },
            ex =>
            {
                ex.Log();
                Messenger.Default.Send(new StatuMessage(ex.Message()));
                Method.Error(this, ex.Message());
            });
            Animation(e1);
            Animation(e2, 750);
            Animation(e3, 1500);
            Animation(e4, 2250);
        }
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            Method.EllipseAdorner(e, 30);
            base.OnPreviewMouseDown(e);
        }
        private void Animation(Ellipse ellipse, double beginTime = 0, double time = 3000)
        {
            var widthAnimation = new DoubleAnimation(10, 100, new Duration(TimeSpan.FromMilliseconds(time)));
            widthAnimation.BeginTime = TimeSpan.FromMilliseconds(beginTime);
            widthAnimation.RepeatBehavior = RepeatBehavior.Forever;
            ellipse.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
            ellipse.BeginAnimation(FrameworkElement.HeightProperty, widthAnimation);

            var colorAnimation = new ColorAnimation(Color.FromArgb(160, 255, 0, 0), Color.FromArgb(10, 255, 0, 0), new Duration(TimeSpan.FromMilliseconds(time)));
            colorAnimation.BeginTime = TimeSpan.FromMilliseconds(beginTime);
            colorAnimation.RepeatBehavior = RepeatBehavior.Forever;
            var solid = ellipse.Fill = (SolidColorBrush)ellipse.Fill.Clone();
            solid.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        private bool b;
        private Path pathNew;
        private object pathCurrent;
        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            var r = Validation.GetHasError(tb);
            Method.Toast(this, r);
            var xml = Method.GetTemplateXaml(dp);
            //Method.Toast(this, xml);
            b = !b;
            if (b) Method.Progress(this, false);
            else Method.Hide();
            if (this.pathCurrent == null)
            {
                this.pathCurrent = transition.Content;
                this.pathNew = new Path
                {
                    Style = this.FindResource("PathRound") as Style
                };
            }
            transition.TransitionType = (TransitionType)new Random().Next(0, 5);
            //transition.TransitionType = TransitionType.Left;
            if (b) transition.Content = this.pathNew;
            else transition.Content = this.pathCurrent;

            var opacity1 = new DoubleAnimation(line1.X1, 0, new Duration(TimeSpan.FromMilliseconds(200)));
            line1.BeginAnimation(Line.X1Property, opacity1);
            var opacity2 = new DoubleAnimation(line2.X2, 100, new Duration(TimeSpan.FromMilliseconds(200)));
            line2.BeginAnimation(Line.X2Property, opacity2);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var opacity1 = new DoubleAnimation(line1.X1, 100, new Duration(TimeSpan.FromMilliseconds(200)));
            line1.BeginAnimation(Line.X1Property, opacity1);
            var opacity2 = new DoubleAnimation(line2.X2, 0, new Duration(TimeSpan.FromMilliseconds(200)));
            line2.BeginAnimation(Line.X2Property, opacity2);
        }
    }
}
