using Shared;
using Shared.Language;
using Shared.Utility;
using System;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Install.Framework.Pages
{
    /// <summary>
    /// Directory.xaml 的交互逻辑
    /// </summary>
    public partial class InstallationDirectory : Page
    {
        public static Uri ViewPath { get; } = new Uri("pack://application:,,,/Pages/InstallationDirectory.xaml");
        public InstallationDirectory()
        {
            InitializeComponent();
            this.DataContext = new InstallationDirectoryViewModel();
        }
    }

    public class InstallationDirectoryViewModel : PropertyChangedBase
    {
        public static InstallationDirectoryViewModel VM { get; private set; }

        private string selectedPath;
        public string SelectedPath { get { return selectedPath; } set { selectedPath = value; OnPropertyChanged("SelectedPath"); } }

        public DelegateCommand PathSearchCommand { get; set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }
        /// <summary>
        /// 是否创建桌面快捷方式
        /// </summary>
        public bool CreateDesktopShortcut { get; set; } = true;

        public InstallationDirectoryViewModel()
        {
            if (MainWindowViewModel.VM.HasInstalled && RegeditUtility.SearchValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "InstallDirectory"))
            {
                var installdirectory = RegeditUtility.GetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "InstallDirectory");
                if (string.IsNullOrEmpty(installdirectory)) SelectedPath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"\", ApplicationParameters.Publisher, @"\", ApplicationParameters.DisplayName);
                else SelectedPath = installdirectory;
            }
            else SelectedPath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"\", ApplicationParameters.Publisher, @"\", ApplicationParameters.DisplayName);
            PathSearchCommand = new DelegateCommand(SearchPath);
            PreviousCommand = new DelegateCommand((obj) =>
            {
                if (ApplicationParameters.HaveLicense) MainWindowViewModel.VM.Source = License.ViewPath;
                else MainWindowViewModel.VM.Source = LanguageSelect.ViewPath;
            });
            NextCommand = new DelegateCommand((obj) => { MainWindowViewModel.VM.Source = Installing.ViewPath; });
            VM = this;
        }

        public void SearchPath(object obj)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = SelectedPath,
                Description = LanguageConfiguration.GetStringResource("StrInstallationDirectorySelect")
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var path = dialog.SelectedPath;
                var temp = string.Concat(@"\", ApplicationParameters.Publisher, @"\", ApplicationParameters.DisplayName);
                if (path.EndsWith(temp)) SelectedPath = path;
                else SelectedPath = string.Concat(path, temp);
            }
        }
    }
}
