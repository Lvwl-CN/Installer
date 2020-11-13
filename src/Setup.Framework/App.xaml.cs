using Shared.Language;
using System.Windows;

namespace Install.Framework
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LanguageConfiguration.Configuration.ReloadResource();
        }

    }
}
