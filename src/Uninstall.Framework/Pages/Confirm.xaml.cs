using Shared;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Uninstall.Framework.Pages
{
    /// <summary>
    /// Confirm.xaml 的交互逻辑
    /// </summary>
    public partial class Confirm : Page
    {

        public static Uri ViewPath { get; } = new Uri("pack://application:,,,/Pages/Confirm.xaml");
        public Confirm()
        {
            InitializeComponent();
            this.DataContext = new ConfirmViewModel();
        }
    }

    public class ConfirmViewModel : PropertyChangedBase
    {
        public ICommand UninstallCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ConfirmViewModel()
        {
            CancelCommand = new DelegateCommand((obj) => { Application.Current.Shutdown(); });
            UninstallCommand = new DelegateCommand((obj) => { MainWindowViewModel.VM.Source = Uninstalling.ViewPath; });
        }
    }
}
