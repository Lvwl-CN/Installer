using Shared;
using Shared.Language;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Install.Framework.Pages
{
    /// <summary>
    /// Language.xaml 的交互逻辑
    /// </summary>
    public partial class LanguageSelect : Page
    {
        public static Uri ViewPath { get; } = new Uri("pack://application:,,,/Pages/LanguageSelect.xaml");
        public LanguageSelect()
        {
            InitializeComponent();
            this.DataContext = new LanguageSelectViewModel();
        }

        private void cbx_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton cbx && cbx.CommandParameter is Shared.Language.Language language)
            {
                LanguageConfiguration.Configuration.SelectedLanguage = language;
            }
        }
    }

    public class LanguageSelectViewModel : PropertyChangedBase
    {
        public Dictionary<string, Language> Languages { get { return LanguageConfiguration.Configuration.Languages; } }

        public ICommand NextCommand { get; private set; }
        public LanguageSelectViewModel()
        {
            NextCommand = new DelegateCommand((obj) =>
            {
                if (ApplicationParameters.HaveLicense) MainWindowViewModel.VM.Source = License.ViewPath;
                else MainWindowViewModel.VM.Source = InstallationDirectory.ViewPath;
            });
        }
    }

}
