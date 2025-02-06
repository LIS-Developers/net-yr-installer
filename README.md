# 尤复安装器 Installer for YR/RA2 or mods

> [!IMPORTANT]
>
> _ID: net-yuri-installer_  
> &emsp;&emsp;本项目适用于中国MOD社区普遍采用的整包发布（MOD与YR本体捆绑发布），并不用打算为国外社区所采用，为方便中文使用者和愿意协助的国外友人，请尽量采用中英并行文本讨论。

&emsp;&emsp;本安装器项目适用于绝大多数YRMOD（当然也包括其他任意类型的程序），灵感来源于[Westwood官方的CD安装包](https://www.uc129.com/xiazai/ra2/10802.html)。  
&emsp;&emsp;本人很喜欢红警安装界面，曾联系重聚未来制作组获取该安装包模版以用于我自己的MOD项目，但对方似乎并不愿意与我分享，于是无奈自己复刻了一个类似的出来，但是易语言报毒，C++不会写，Python做GUI不方便，平均下来还是C#好。  
&emsp;&emsp;希望各位喜欢或愿意支持本项目的朋友能捐赠 ≥20元 或 ≥$3.5 支持下作者（毕竟最初是他人找我花钱定制的，如果都随便拿去用对他不公平）。暂时没钱的朋友也可以以后再给，我不会亏待任何一位给了赞助的宝子。

## 使用注意

&emsp;&emsp;如果你要将本项目用于自己的MOD，现在还不是最佳时机，因为一切自定义都在源代码内进行，一旦遇到代码大改可能很难跟进。可以等到开发后期代码优化差不多了再使用。

&emsp;&emsp;程序默认读取setup.7z（本意用于本体）和setup1.7z（本意用于扩展包）作为安装文件，如果要修改请到ExtraControls.cs和生成后命令行。生成后命令行会将安装文件从项目文件夹复制到生成文件夹去。

## 解决方案组成

本解决方案由以下项目组成：
1. 安装器（C#）
    1) 组成
        + 源代码
        + 扩展控件
        + NuGet包及COM组件
    2) 说明
        + 整个解决方案最核心的部分，依赖于卸载程序项目，没了它这个解决方案可以说啥也不是。
        + 源代码是整个项目的核心部分，包含界面、控件、事件等。
        + \_StartWindow是主窗口逻辑，ExtraWindows是扩展窗口，MizukiTool是我的组件库。
        + 一些NuGet包和COM组件：`7z.lib`用于解压。
2. 卸载程序（VB）
    1) 组成
        + 源代码
    2) 说明
        + 用于卸载程序，如删除文件、注册表以及快捷方式。
        + 但是他会把整个文件夹都删掉，如果你把软件装在了C盘……抱歉C盘要被清空咯！
3. 打包器（Python）
    1) 组成
        + 源代码
        + 可执行文件描述文件
    2) 额外模块（import）
        + pypiwin32
        + pywin32
        + pyinstaller
    3) 说明
        + 动态链接安装文件和安装器。
4. 安装文件（7z）
    1) 说明
        + 安装怎么可以连安装什么都没有呢？一般命名为`setup*.7z`，取什么名字无所谓。使用多选项时若有覆盖的文件将会重复计算（以确保足够空间）。
5. 打包脚本（Bat）
    1) 说明
        + 引用Python打包器打包安装器和安装文件，生成的exe运行时将安装文件释放到临时目录。

&emsp;&emsp;生成解决方案/安装器的时候会先生成卸载程序，再将卸载程序载入到安装器的Resources里，最后生成安装器。生成完解决方案点击方案根目录的pack.bat即可将安装器与安装文件打包。

## 功能及计划

&emsp;&emsp;目前不考虑跨平台，也不考虑支持XP系统。  

特色功能：

- 由于卸载器也需要.Net，所以会生成cmd卸载脚本来应对无法使用.Net的情况。

已知Bug：

- 初次启动点击前进键偶尔会直接跳过许可协议进入安装选项界面。
- 启动有点卡顿。

总体功能计划：

- [x] 多语言基本支持[^1]
- [x] 优化源代码可读性
- [ ] 统一的安装包信息编辑
- [ ] 利用CodeAnalysis检查代码

安装器功能计划：

- [x] 安装程序界面
- [x] 红警目录选择框、滚动框
- [x] 红警按钮点击动画
- [x] 写入注册表
- [x] 创建快捷方式
- [x] 内存优化
- [x] 弹出对话框
- [x] 自定义开始菜单文件夹名
- [x] 编辑框获得焦点变亮
- [ ] 在线安装功能
- [ ] 灵活且不同版本兼容的自定义安装信息
- [x] 安装包背景窗口
- [x] 安装包切换分辨率（未来可能弃用）

卸载器功能计划：

- [x] 删除注册表及快捷方式
- [x] 提供无需.net的卸载程序（如bat脚本文件）
- [ ] 仅删除安装的文件
<!-- [ ] 允许修复原程序 -->

打包器功能计划：

- [x] 链接安装器和安装文件
- [ ] 静态链接，以免资源浪费

## 简易生成步骤

在此之前请确保你已安装Python所需的所有依赖项。

1. 点击生成解决方案。  
2. 点击pack.bat。  
3. 打开Compiled文件夹，得到生成的文件。  

## 注意事项

* 本项目使用Visual Studio 2022开发，但不知道其他版本行不行，作者以为2019没用了就给卸了，所以无法测试。  
* 项目的资源放置在Resources文件夹，安装文件放在项目根目录。  
* 安装选项（比如要安装什么文件）设置在ExtraControls.cs里。  
* 禁止用于一切违法犯罪活动！  
* 若要商用本仓库的代码，请勿携带一切不属于自己版权的东西（如红警图片、声音等）！

## 需求

Windows .NET 4.8 运行库:

* [.NET Framework 4.8 Runtime](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/thank-you/net48-web-installer)

## 版权

&emsp;&emsp;本项目著作权属Enderseven Tina（Shimada Mizuki、Ender7 Tina、Enderseven1），Obfuscar插件之著作权属原作者，使用MIT协议。  

## 制作团队

+ 发起者、赞助商：Flactine
+ 制作者：Shimada Mizuki
+ 协助者：ChatGPT、豆包

&emsp;&emsp;如果你要赞助本项目，请不要忘记本项目的发起者*Flactine*的那份。

## 鸣谢列表

Thanks to 3F:
+ [7zip.Libs](https://github.com/3F/7z.Libs)

Thanks to Joel Ahlgren, Markovtsev Vadim, Jérémy Ansel: 
+ [SharpSevenZip](https://github.com/JeremyAnsel/SharpSevenZip)

Thanks to Mark Heath & Contributors:
+ [NAudio](https://github.com/naudio/NAudio)
+ NAudio.Asio
+ NAudio.Core
+ NAudio.Midi
+ NAudio.Wasapi
+ NAudio.WinForms

Thanks to [Microsoft](https://www.microsoft.com):
+ Microsoft.Bcl.Asynclnterfaces
+ System.Buffers
+ System.IO.Pipelines
+ System.Memory
+ System.Numerics.Vectors
+ System.Runtime.CompilerServices.Unsafe
+ System.Text.Encodings.Web
+ System.Text.Json
+ System.Threading.Tasks.Extensions
+ System.ValueTuple
+ Microsoft.CodeAnalysis.Analyzers
+ System.Collections.Immutable
+ System.Reflection.Metadata
+ System.Text.Encoding.CodePages
+ Microsoft.CodeAnalysis.Common
+ Microsoft.CodeAnalysis.Csharp
+ Microsoft.Win32.Registry
+ System.Security.AccessControl
+ System.Security.Principal.Windows

## 要饭

&emsp;&emsp;如果大大喜欢这个项目的话，烦请点个star请两瓶屌丝饮料支持一下（  
&emsp;&emsp;作者也是有成本的呀，不过不强制大家啦，愿意给就给吧！
![赞赏码](donate.png)

---

Copyright © 2024-2025 Shimada Mizuki. All Rights Reserved.

[^1]: 只有安装程序、卸载器支持，卸载器不支持动态切换语言、添加语言文件。
[^2]: 目前只有音频已经脱离WMP组件，不过已经做到通过条件编译把WMP砍掉。
[^3]: 在不修改该功能的前提下直接使用该功能可能引发Bug（因为该功能仍在测试），而且仅支持XXXXX-XXXXX-XXXXX-XXXXX-XXXXX格式。