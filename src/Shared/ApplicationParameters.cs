namespace Shared
{
    public static class ApplicationParameters
    {
        /// <summary>
        /// 主启动程序路径
        /// </summary>
        public const string ExePath = "Setup.exe";
        /// <summary>
        /// 程序名称
        /// </summary>
        public const string DisplayName = "Setup";
        /// <summary>
        /// 显示版本号
        /// </summary>
        public const string DisplayVersion = "1.0.0.0";
        /// <summary>
        /// 作者名称
        /// </summary>
        public const string Publisher = "Lvwl-CN";

        /// <summary>
        /// 更新链接
        /// </summary>
        public const string URLUpdateInfo = "https://github.com/Lvwl-CN/Setup";
        /// <summary>
        /// 支持链接
        /// </summary>
        public const string URLInfoAbout = "https://github.com/Lvwl-CN";
        /// <summary>
        /// 帮助链接
        /// </summary>
        public const string HelpLink = "https://github.com/Lvwl-CN/Setup";


        /// <summary>
        /// 是否有许可协议
        /// </summary>
        public static bool HaveLicense = true;









        /// <summary>
        /// 64位注册表项
        /// </summary>
        //public static string UninstallRegeditPath = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
        /// <summary>
        /// 32位注册表项
        /// </summary>
        public const string UninstallRegeditPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        public static readonly string ApplicationUninstallRegeditPath = string.Concat(UninstallRegeditPath, @"\", DisplayName);
    }
}
