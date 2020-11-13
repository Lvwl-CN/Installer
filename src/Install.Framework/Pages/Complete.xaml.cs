using Shared;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Install.Framework.Pages
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
        public bool Run { get; set; } = true;
        public ICommand CompleteCommand { get; set; }
        public CompleteViewModel()
        {
            CompleteCommand = new DelegateCommand((obj) =>
            {
                var mainexepath = string.Concat(InstallationDirectoryViewModel.VM.SelectedPath, @"\", ApplicationParameters.ExePath);
                if (Run && File.Exists(mainexepath))
                {
                    System.Diagnostics.Process.Start(mainexepath);
                }
                Application.Current.Shutdown();
            });
        }
    }
}
