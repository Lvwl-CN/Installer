using Shared.Language;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace Uninstall.Framework
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

        /// <summary>
        /// 是否卸载了，卸载后需要删除本exe
        /// </summary>
        public static bool IsUninstalled { get; set; } = false;

        public static List<string> FilesNeedRemove { get; } = new List<string>();

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            if (IsUninstalled)
            {
                var temp = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("/C ping 1.1.1.1 -n 1 -w 2000 > Nul & del \"");
                sb.Append(temp);
                sb.Append("Uninstall.exe\" ");
                foreach (var file in FilesNeedRemove)
                {
                    sb.Append("& del \"");
                    sb.Append(file);
                    sb.Append("\" ");
                }
                sb.Append("& rd \"");
                sb.Append(temp);
                sb.Append("\" & exit");

                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", sb.ToString())
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                };
                Process.Start(psi);
            }
        }
    }
}
