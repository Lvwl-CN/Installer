using Shared;
using Shared.Language;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Resources;

namespace Install.Framework.Pages
{
    /// <summary>
    /// License.xaml 的交互逻辑
    /// </summary>
    public partial class License : Page
    {
        public static Uri ViewPath { get; } = new Uri("pack://application:,,,/Pages/License.xaml");
        public License()
        {
            InitializeComponent();
            this.DataContext = new LicenseViewModel();
            this.Loaded += License_Loaded;
        }

        private void License_Loaded(object sender, RoutedEventArgs e)
        {
            var licensefilepath = new Uri(LanguageConfiguration.GetStringResource("StrLicensePath"));
            StreamResourceInfo info = Application.GetResourceStream(licensefilepath);
            FlowDocument flowDocument = new FlowDocument();
            TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            textRange.Load(info.Stream, DataFormats.Rtf);
            rtb.Document = flowDocument;
        }
    }

    public class LicenseViewModel : PropertyChangedBase
    {
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }

        public LicenseViewModel()
        {
            PreviousCommand = new DelegateCommand((obj) => { MainWindowViewModel.VM.Source = LanguageSelect.ViewPath; });
            NextCommand = new DelegateCommand((obj) => { MainWindowViewModel.VM.Source = InstallationDirectory.ViewPath; });
        }
    }
}
