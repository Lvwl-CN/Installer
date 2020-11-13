using Shared;
using Shared.Language;
using Shared.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Resources;

namespace Install.Framework.Pages
{
    /// <summary>
    /// Installing.xaml 的交互逻辑
    /// </summary>
    public partial class Installing : Page
    {
        public static Uri ViewPath { get; } = new Uri("pack://application:,,,/Pages/Installing.xaml");
        public Installing()
        {
            InitializeComponent();
            var vm = new InstallingViewModel(this.rtb);
            this.DataContext = vm;
            this.Loaded += (sender, e) =>
            {
                vm.StartInstall();
            };
        }
    }

    public class InstallingViewModel : PropertyChangedBase
    {
        private int index;
        /// <summary>
        /// 当前完成解压了几个
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

        static Uri ZipFileUri = new Uri("pack://application:,,,/Application.zip");
        private RichTextBox Richtbx { get; set; }

        public InstallingViewModel(RichTextBox richTextBox)
        {
            this.Richtbx = richTextBox;
        }

        /// <summary>
        /// 开始安装
        /// </summary>
        public async void StartInstall()
        {
            ShowMessage(LanguageConfiguration.GetStringResource("StrBegin"));
            var path = InstallationDirectoryViewModel.VM.SelectedPath;
            await Task.Run(() =>
            {
                if (MainWindowViewModel.VM.HasInstalled)
                {
                    ShowMessage(LanguageConfiguration.GetStringResource("StrRemoveOldVersion"));
                    RemoveOldVersionFiles();
                }
                else
                {
                    ShowMessage(string.Format(LanguageConfiguration.GetStringResource("StrCreateDirectory"), path));
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                }
                UnzipFile(path);
                CreateShortcut(path);
                CreateRegedit(path);
            });
            ShowMessage(LanguageConfiguration.GetStringResource("StrInstallComplete"));
            MainWindowViewModel.VM.Source = Complete.ViewPath;
        }

        /// <summary>
        /// 删除老版本所有文件
        /// </summary>
        private void RemoveOldVersionFiles()
        {
            if (!RegeditUtility.SearchItemRegEdit(ApplicationParameters.UninstallRegeditPath, ApplicationParameters.DisplayName) || !RegeditUtility.SearchValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "InstallDirectory")) return;
            var installdirectory = RegeditUtility.GetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "InstallDirectory");
            if (string.IsNullOrEmpty(installdirectory)) return;
            string filesinfo = string.Concat(installdirectory, @"\Uninstall.data");
            if (!File.Exists(filesinfo)) return;
            List<string> files = XmlUtility.DeserializeToObject<List<string>>(filesinfo);
            if (files == null || files.Count <= 0) return;
            foreach (var file in files)
            {
                if (File.Exists(file)) File.Delete(file);
            }
            File.Delete(filesinfo);
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="path"></param>
        private void UnzipFile(string path)
        {
            StreamResourceInfo info = Application.GetResourceStream(ZipFileUri);
            using (ZipArchive archive = new ZipArchive(info.Stream, ZipArchiveMode.Read))
            {
                var entries = archive.Entries;
                Count = entries.Count;
                try
                {
                    List<string> files = new List<string>();
                    for (var i = 0; i < entries.Count; i++)
                    {
                        var entry = entries[i];
                        if (!IsDirectory(entry))
                        {
                            ShowMessage(string.Format(LanguageConfiguration.GetStringResource("StrUnzipFile"), entry.FullName));
                            string filePath = System.IO.Path.Combine(path, entry.FullName);
                            var parentDirectory = System.IO.Path.GetDirectoryName(filePath);
                            if (!Directory.Exists(parentDirectory)) Directory.CreateDirectory(parentDirectory);
                            entry.ExtractToFile(filePath, true);
                            files.Add(filePath);
                        }
                        Index = i + 1;
                    }
                    XmlUtility.SaveObjectToXmlFile(files, string.Concat(path, @"\Uninstall.data"));
                    ShowMessage(LanguageConfiguration.GetStringResource("StrUnzipComplete"));
                }
                catch (Exception e)
                {
                    ShowMessage(e.Message);
                }
            }
        }
        /// <summary>
        /// zip包里面的内容是否是文件夹
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static bool IsDirectory(ZipArchiveEntry entry)
        {
            return string.IsNullOrEmpty(entry.Name) && (entry.FullName.EndsWith("/") || entry.FullName.EndsWith(@"\"));
        }

        /// <summary>
        /// 创建快捷方式
        /// </summary>
        /// <param name="path"></param>
        private void CreateShortcut(string path)
        {
            string exepath = string.Concat(path, @"\", ApplicationParameters.ExePath);
            string uninstallexepath = string.Concat(path, @"\Uninstall.exe");
            ShowMessage(LanguageConfiguration.GetStringResource("StrCreateMenuShortcut"));
            ShortcutUtility.CreateShortcutOnStartMenuWithAllUser(ApplicationParameters.Publisher, ApplicationParameters.DisplayName, exepath);
            ShortcutUtility.CreateShortcutOnStartMenuWithAllUser(ApplicationParameters.Publisher, string.Format(LanguageConfiguration.GetStringResource("StrUninstallAppName"), ApplicationParameters.DisplayName), uninstallexepath);
            if (InstallationDirectoryViewModel.VM.CreateDesktopShortcut)
            {
                ShowMessage(LanguageConfiguration.GetStringResource("StrCreateDesktopShortcut"));
                ShortcutUtility.CreateShortcutOnDesktopWithAllUser(ApplicationParameters.DisplayName, exepath);
            }
        }

        /// <summary>
        /// 写注册表
        /// </summary>
        private void CreateRegedit(string path)
        {
            string exepath = string.Concat(path, @"\", ApplicationParameters.ExePath);
            string uninstallexepath = string.Concat(path, @"\Uninstall.exe");
            if (!MainWindowViewModel.VM.HasInstalled) RegeditUtility.CreateItemRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath);
            RegeditUtility.SetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "DisplayIcon", exepath);
            RegeditUtility.SetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "DisplayName", ApplicationParameters.DisplayName);
            RegeditUtility.SetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "DisplayVersion", ApplicationParameters.DisplayVersion);
            RegeditUtility.SetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "Publisher", ApplicationParameters.Publisher);
            RegeditUtility.SetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "UninstallString", uninstallexepath);
            RegeditUtility.SetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "Language", LanguageConfiguration.Configuration.SelectedLanguage.Key);
            RegeditUtility.SetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "InstallDirectory", path);
            if (!string.IsNullOrEmpty(ApplicationParameters.URLUpdateInfo)) RegeditUtility.SetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "URLUpdateInfo", ApplicationParameters.URLUpdateInfo);
            if (!string.IsNullOrEmpty(ApplicationParameters.URLInfoAbout)) RegeditUtility.SetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "URLInfoAbout", ApplicationParameters.URLInfoAbout);
            if (!string.IsNullOrEmpty(ApplicationParameters.HelpLink)) RegeditUtility.SetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "HelpLink", ApplicationParameters.HelpLink);
        }

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
