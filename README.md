# Installer
    基于.NetFramework 4.5 WPF开发的应用软件打包程序
    需要自行编译和文件拷贝等操作
    安装页面、多语言、安装步骤等，都可完全自定义
## 使用环境
* 在Windows平台运行的应用软件需要制作安装包
* Windows平台已经安装.NetFramework 4.5及以上版本，否则无法运行安装包
* 开发者熟悉C#语言和WPF
* 开发工具为Visual Studio 2019及以上版本
## 使用方法
1.  准备好需要打包的文件，包括主运行的exe文件，压缩到工程的Install.Framework\Application.zip文件里。
2.  修改Shared\ApplicationParameters.cs文件。

    >public const string ExePath = "主运行程序.exe";//主启动程序文件名称，包含后缀

    >public const string DisplayName = "软件名称";//程序名称，在开始菜单、桌面快捷方式、程序和功能中可见，可以与主运行程序名称不同
    
    >public const string DisplayVersion = "1.0.0.0";//显示版本号
    
    >public const string Publisher = "Lvwl-CN";//作者名称,公司名称或个人名称
    
    >public const string URLUpdateInfo = "";//更新链接
    
    >public const string URLInfoAbout = "";//支持链接
    
    >public const string HelpLink = "";//帮助链接
    
    >public static bool HaveLicense = true;//是否有许可协议，如果有许可协议，需要准备好协议文件，rtf格式

3.  如果有许可协议，替换Install.Framework\Licenses目录下的许可协议文件，支持不同语言使用不同协议文件。并更改Shared\Language目录下多语言包（xaml文件），设置x:Key="StrLicensePath"的值，指向对应的协议文件。

4. 如果需要更多种类语言包，可在Shared\Language目录下添加语言包，并修改Shared\Language\LanguageConfiguration.cs文件里的LanguageConfiguration类的构造函数，将语言包加入选项。

5.  此工程只做了解压文件到目录，写注册表和创建快捷方式。如果需要在安装前后或卸载前后执行额外操作，可自行编写功能代码。安装操作在Install.Framework\Pages\Installing.xaml.cs文件中。卸载操作在Uninstall.Framework\Pages\Uninstalling.xaml.cs文件中。

6.  更换Install.Framework和Uninstall.Framework的图标，需要自行准备图标，在属性=>应用程序=》图标 中修改。

7.  更换Install.Framework\MainWindow.xaml和Uninstall.Framework\MainWindow.xaml窗体中的矢量图标，位于窗体左上角。

8.  如果需要，可以更改Install.Framework的程序集名称为自己想要的名称。但Uninstall.Framework的程序集名称不能修改，必须为Uninstall,在代码中多次使用到该名称。

9.  生成Uninstall.Framework，得到Uninstall.exe文件，将其压缩到Install.Framework\Application.zip文件中。

10. 生成Install.Framework，得到最终的安装文件。

