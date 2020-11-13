using Microsoft.Win32;
using System;

namespace Shared.Utility
{
    /// <summary>
    /// 注册表帮助类
    /// </summary>
    public class RegeditUtility
    {
        /// <summary>
        /// 创建注册表项
        /// </summary>
        /// <param name="path">注册表路经</param>
        /// <returns>返回注册表对象</returns>
        public static bool CreateItemRegEdit(string path)
        {
            try
            {
                using (RegistryKey obj = Registry.LocalMachine)
                {
                    obj.CreateSubKey(path);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 删除注册表项
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteItemRegEdit(string path)
        {
            try
            {
                using (RegistryKey obj = Registry.LocalMachine)
                {
                    obj.DeleteSubKey(path);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置注册表项下面的值
        /// </summary>
        /// <param name="path">路经</param>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        public static bool SetValueRegEdit(string path, string name, string value)
        {
            try
            {
                using (RegistryKey obj = Registry.LocalMachine)
                {
                    using (RegistryKey objItem = obj.OpenSubKey(path, true))
                    {
                        objItem.SetValue(name, value);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool SetValueRegEdit(string path, string name, int value)
        {
            try
            {
                using (RegistryKey obj = Registry.LocalMachine)
                {
                    using (RegistryKey objItem = obj.OpenSubKey(path, Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl))
                    {
                        objItem.SetValue(name, value, RegistryValueKind.DWord);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 查看注册表指定项的值
        /// </summary>
        /// <param name="path">路经</param>
        /// <param name="name">项名称</param>
        /// <returns>项值</returns>
        public static string GetValueRegEdit(string path, string name)
        {
            try
            {
                using (RegistryKey obj = Registry.LocalMachine)
                {
                    using (RegistryKey objItem = obj.OpenSubKey(path))
                    {
                        return objItem.GetValue(name).ToString();
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 查看注册表项是否存在
        /// </summary>
        /// <param name="value">路经</param>
        /// <param name="name">项名称</param>
        /// <returns>是否存在</returns>
        public static bool SearchItemRegEdit(string path, string name)
        {
            string[] subkeyNames;
            using (RegistryKey hkml = Registry.LocalMachine)
            {
                using (RegistryKey software = hkml.OpenSubKey(path))
                {
                    if (software == null) return false;
                    subkeyNames = software.GetSubKeyNames();
                    //取得该项下所有子项的名称的序列，并传递给预定的数组中  
                    foreach (string keyName in subkeyNames)   //遍历整个数组   
                    {
                        if (keyName.ToUpper() == name.ToUpper()) //判断子项的名称   
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// 查看注册表的值是否存在
        /// </summary>
        /// <param name="value">路经</param>
        /// <param name="value">查看的值</param>
        /// <returns>是否成功</returns>
        public static bool SearchValueRegEdit(string path, string value)
        {
            string[] subkeyNames;
            using (RegistryKey hkml = Registry.LocalMachine)
            {
                using (RegistryKey software = hkml.OpenSubKey(path))
                {
                    subkeyNames = software.GetValueNames();
                    //取得该项下所有键值的名称的序列，并传递给预定的数组中  
                    foreach (string keyName in subkeyNames)
                    {
                        if (keyName.ToUpper() == value.ToUpper())   //判断键值的名称   
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

    }
}
