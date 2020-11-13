using Shared;
using Shared.Language;
using Shared.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uninstall.Framework.Pages
{
    /// <summary>
    /// Uninstalling.xaml 的交互逻辑
    /// </summary>
    public partial class Uninstalling : Page
    {
        public static Uri ViewPath { get; } = new Uri("pack://application:,,,/Pages/Uninstalling.xaml");
        public Uninstalling()
        {
            InitializeComponent();
            var vm = new UninstallingViewModel(this.rtb);
            this.DataContext = vm;
            this.Loaded += (sender, e) =>
            {
                vm.StartUninstall();
            };
        }
    }

    public class UninstallingViewModel : PropertyChangedBase
    {
        private int index = 0;
        /// <summary>
        /// 当前删除了几个
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; OnPropertyChanged("Index"); }
        }
        private int count;
        /// <summary>
        /// 文件总数量
        /// </summary>
        public int Count
        {
            get { return count; }
            set { count = value; OnPropertyChanged("Count"); }
        }
        private RichTextBox Richtbx { get; set; }
        public UninstallingViewModel(RichTextBox richTextBox)
        {
            this.Richtbx = richTextBox;
        }

        public async void StartUninstall()
        {
            ShowMessage(LanguageConfiguration.GetStringResource("StrStartUninstall"));
            await Task.Run(() =>
            {
                RemoveFiles();
                RemoveShortcut();
                RemoveRegedit();
                ShowMessage(LanguageConfiguration.GetStringResource("StrUninstallComplete"));
            });
            App.IsUninstalled = true;
            MainWindowViewModel.VM.Source = Complete.ViewPath;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        private void RemoveFiles()
        {
            var installdirectory = (RegeditUtility.SearchItemRegEdit(ApplicationParameters.UninstallRegeditPath, ApplicationParameters.DisplayName) && RegeditUtility.SearchValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "InstallDirectory")) ? RegeditUtility.GetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "InstallDirectory") : AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrEmpty(installdirectory) || !Directory.Exists(installdirectory)) installdirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filesinfo = string.Concat(installdirectory, @"\Uninstall.data");
            if (!File.Exists(filesinfo)) return;
            List<string> files = XmlUtility.DeserializeToObject<List<string>>(filesinfo);
            Count = files.Count;
            if (files == null || files.Count <= 0) return;
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                if (file.EndsWith(@"\Uninstall.exe")) continue;
                ShowMessage(string.Format(LanguageConfiguration.GetStringResource("StrRemoveFile"), file));
                try
                {
                    if (File.Exists(file)) File.Delete(file);
                }
                catch (Exception e)
                {
                    App.FilesNeedRemove.Add(file);
                    ShowMessage(e.Message);
                }
                Index = i + 1;
            }
            try
            {
                File.Delete(filesinfo);
            }
            catch (Exception e)
            {
                App.FilesNeedRemove.Add(filesinfo);
                ShowMessage(e.Message);
            }
            ShowMessage(string.Format(LanguageConfiguration.GetStringResource("StrRemoveFile"), filesinfo));
            Shared.Uninstall.KillEmptyDirectory(installdirectory);
        }

        /// <summary>
        /// 删除快捷方式
        /// </summary>
        /// <param name="path"></param>
        private void RemoveShortcut()
        {
            ShowMessage(LanguageConfiguration.GetStringResource("StrRemoveShortcut"));
            string startshortcutdirectory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), @"\", ApplicationParameters.Publisher);
            if (Directory.Exists(startshortcutdirectory)) Directory.Delete(startshortcutdirectory, true);
            var desktopshortcutfile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), string.Format("{0}.lnk", ApplicationParameters.DisplayName));
            if (File.Exists(desktopshortcutfile)) File.Delete(desktopshortcutfile);
        }

        /// <summary>
        /// 删除注册表
        /// </summary>
        private void RemoveRegedit()
        {
            ShowMessage(LanguageConfiguration.GetStringResource("StrRemoveRegedit"));
            RegeditUtility.DeleteItemRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath);
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="msg"></param>
        private void ShowMessage(string msg)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(msg);
                Richtbx.Document.Blocks.Add(paragraph);
                Richtbx.ScrollToEnd();
            });
        }
    }
}
