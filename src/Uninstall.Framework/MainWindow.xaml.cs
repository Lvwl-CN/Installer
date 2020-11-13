using Shared;
using Shared.Control;
using Shared.Utility;
using System;
using System.Windows;

namespace Uninstall.Framework
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.SetWindowSystemButton();
            this.DataContext = new MainWindowViewModel();
        }
    }

    public class MainWindowViewModel : PropertyChangedBase
    {
        public static MainWindowViewModel VM { get; private set; }

        private Uri source;
        public Uri Source
        {
            get { return source; }
            set { source = value; OnPropertyChanged("Source"); }
        }

        public bool HasInstalled { get; set; } = RegeditUtility.SearchItemRegEdit(ApplicationParameters.UninstallRegeditPath, ApplicationParameters.DisplayName);
        public MainWindowViewModel()
        {
            Source = Pages.Confirm.ViewPath;
            VM = this;
        }
    }
}
