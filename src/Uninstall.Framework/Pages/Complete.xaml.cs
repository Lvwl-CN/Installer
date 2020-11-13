using Shared;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Uninstall.Framework.Pages
{
    /// <summary>
    /// Complete.xaml 的交互逻辑
    /// </summary>
    public partial class Complete : Page
    {
        public static Uri ViewPath { get; } = new Uri("pack://application:,,,/Pages/Complete.xaml");
        public Complete()
        {
            InitializeComponent();
            this.DataContext = new CompleteViewModel();
        }
    }

    public class CompleteViewModel : PropertyChangedBase
    {
        public ICommand CompleteCommand { get; set; }
        public CompleteViewModel()
        {
            CompleteCommand = new DelegateCommand((obj) =>
            {
                Application.Current.Shutdown();
            });
        }
    }
}
