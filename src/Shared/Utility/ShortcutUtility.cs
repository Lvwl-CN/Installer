using IWshRuntimeLibrary;
using System;
using System.IO;

namespace Shared.Utility
{
    /// <summary>
    /// 快捷方式帮助类
    /// </summary>
    public class ShortcutUtility
    {
        /// <summary>
        /// 创建快捷方式
        /// </summary>
        /// <param name="directory">快捷方式所处的文件夹</param>
        /// <param name="shortcutName">快捷方式名称</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标路径，格式为"可执行文件或DLL路径, 图标编号"，
        /// 例如System.Environment.SystemDirectory + "\\" + "shell32.dll, 165"</param>
        /// <remarks></remarks>
        public static bool CreateShortcut(string directory, string shortcutName, string targetPath, string description = null, string iconLocation = null)
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            string shortcutPath = Path.Combine(directory, string.Format("{0}.lnk", shortcutName));
            if (System.IO.File.Exists(shortcutPath)) return true;
            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);//创建快捷方式对象
                shortcut.TargetPath = targetPath;//指定目标路径
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);//设置起始位置
                shortcut.WindowStyle = 1;//设置运行方式，默认为常规窗口
                shortcut.Description = description;//设置备注
                shortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;//设置图标路径
                shortcut.Save();//保存快捷方式
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 创建桌面快捷方式
        /// </summary>
        /// <param name="shortcutName">快捷方式名称</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标路径，格式为"可执行文件或DLL路径, 图标编号"</param>
        /// <remarks></remarks>
        public static bool CreateShortcutOnDesktop(string shortcutName, string targetPath, string description = null, string iconLocation = null)
        {
            return CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), shortcutName, targetPath, description, iconLocation);
        }
        /// <summary>
        /// 创建所有用户桌面快捷方式
        /// </summary>
        /// <param name="shortcutName">快捷方式名称</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标路径，格式为"可执行文件或DLL路径, 图标编号"</param>
        /// <remarks></remarks>
        public static bool CreateShortcutOnDesktopWithAllUser(string shortcutName, string targetPath, string description = null, string iconLocation = null)
        {
            return CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), shortcutName, targetPath, description, iconLocation);
        }

        /// <summary>
        /// 创建开机启动快捷方式
        /// </summary>
        /// <param name="shortcutName">快捷方式名称</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标路径，格式为"可执行文件或DLL路径, 图标编号"</param>
        public static bool CreateShortcutOnStartup(string shortcutName, string targetPath, string description = null, string iconLocation = null)
        {
            return CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Startup), shortcutName, targetPath, description, iconLocation);
        }

        /// <summary>
        /// 创建所有用户开机启动快捷方式，需要管理员权限
        /// </summary>
        /// <param name="shortcutName">快捷方式名称</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标路径，格式为"可执行文件或DLL路径, 图标编号"</param>
        public static bool CreateShortcutOnStartupWithAllUser(string shortcutName, string targetPath, string description = null, string iconLocation = null)
        {
            return CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup), shortcutName, targetPath, description, iconLocation);
        }

        /// <summary>
        /// 创建开始菜单快捷方式
        /// </summary>
        /// <param name="shortcutName">快捷方式名称</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标路径，格式为"可执行文件或DLL路径, 图标编号"</param>
        /// <returns></returns>
        public static bool CreateShortcutOnStartMenu(string directory, string shortcutName, string targetPath, string description = null, string iconLocation = null)
        {
            return CreateShortcut(string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), @"\", directory), shortcutName, targetPath, description, iconLocation);
        }

        /// <summary>
        /// 创建所有用户开始菜单快捷方式
        /// </summary>
        /// <param name="shortcutName">快捷方式名称</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标路径，格式为"可执行文件或DLL路径, 图标编号"</param>
        /// <returns></returns>
        public static bool CreateShortcutOnStartMenuWithAllUser(string directory, string shortcutName, string targetPath, string description = null, string iconLocation = null)
        {
            return CreateShortcut(string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), @"\", directory), shortcutName, targetPath, description, iconLocation);
        }
    }
}
