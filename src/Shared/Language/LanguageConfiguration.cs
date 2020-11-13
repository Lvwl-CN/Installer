using Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Shared.Language
{
    public class LanguageConfiguration
    {
        public static LanguageConfiguration Configuration = new LanguageConfiguration();
        public Dictionary<string, Language> Languages { get; set; }
        private Language selectedLanguage;
        /// <summary>
        /// 选中的语言
        /// </summary>
        public Language SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                ResourceInvert(value);
            }
        }

        #region event
        /// <summary>
        /// 语言包路径
        /// </summary>
        static ResourceDictionary resourceDictionary = Application.Current.Resources.MergedDictionaries[0];//资源类
        #endregion

        public LanguageConfiguration()
        {
            Languages = new Dictionary<string, Language>()
            {
                {"zh-CN", new Language() {Key="zh-CN", Display = "简体中文", Value =new Uri("pack://application:,,,/Language/zh-CN.xaml") } },
                {"en-US",new Language() {Key="en-US", Display = "English", Value = new Uri("pack://application:,,,/Language/en-US.xaml") } }
            };
        }
        #region Method

        /// <summary>
        /// 加载多语言文件
        /// </summary>
        public void ReloadResource()
        {
            string tempName = (RegeditUtility.SearchItemRegEdit(ApplicationParameters.UninstallRegeditPath, ApplicationParameters.DisplayName) && RegeditUtility.SearchValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "Language")) ? RegeditUtility.GetValueRegEdit(ApplicationParameters.ApplicationUninstallRegeditPath, "Language") : System.Globalization.CultureInfo.InstalledUICulture.Name;
            if (!Languages.ContainsKey(tempName)) tempName = "zh-CN";
            SelectedLanguage = Languages[tempName];
        }

        /// <summary>
        /// 切换语言资源文件
        /// </summary>
        /// <param name="languageFilePath"></param>
        public static void ResourceInvert(Language lan)
        {
            if (resourceDictionary != null && Application.Current.Resources.MergedDictionaries.Contains(resourceDictionary))
                Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
            resourceDictionary = new ResourceDictionary() { Source = lan.Value };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        /// <summary>
        /// 获取语言值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringResource(string key)
        {
            if (string.IsNullOrEmpty(key) || resourceDictionary == null) return string.Empty;
            var item = resourceDictionary[key];
            if (item == null) return key;
            return item.ToString();
        }

        #endregion
    }

    /// <summary>
    /// 语言对象
    /// </summary>
    public class Language
    {
        public string Key { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Display { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public Uri Value { get; set; }
    }
}
