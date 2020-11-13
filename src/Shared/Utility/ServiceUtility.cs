using System.Collections;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace Shared.Utility
{
    /// <summary>
    /// Windows服务操作
    /// </summary>
    public class ServiceUtility
    {
        /// <summary>
        /// 判断服务是否存在
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            return services.Count(p => p.ServiceName.ToLower().Equals(serviceName.ToLower())) > 0;
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        /// <param name="serviceFilePath">服务文件路径</param>
        public static void InstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller() { UseNewContext = true, Path = serviceFilePath })
            {
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);
            }
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="serviceFilePath">服务文件路径</param>
        public static void UninstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller() { UseNewContext = true, Path = serviceFilePath })
                installer.Uninstall(null);
        }

        /// <summary>
        /// 服务是否在运行
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static bool IsServiceRuning(string serviceName)
        {
            if (!IsServiceExisted(serviceName)) return false;
            using (ServiceController control = new ServiceController(serviceName))
                return control.Status == ServiceControllerStatus.Running;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void ServiceStart(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
                if (control.Status == ServiceControllerStatus.Stopped)
                {
                    control.Start();
                    control.WaitForStatus(ServiceControllerStatus.Running);
                }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void ServiceStop(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
                if (control.Status == ServiceControllerStatus.Running)
                {
                    control.Stop();
                    control.WaitForStatus(ServiceControllerStatus.Stopped);
                }
        }
    }
}
