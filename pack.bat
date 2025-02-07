echo Off && chcp 65001 && pyinstaller "InstallerPacker\setup.spec" && (if not exist "Compiled\" (md "Compiled\") || true) && copy "dist\Setup.exe" "Compiled\Setup.exe" /y && rd /s/q "build\" && rd /s/q "dist\" && goto then

@REM 小样，还想阻止我写注释，反了你了CMD

命令解释：
1、切换代码页到65001（UTF-8）；
2、利用Pyinstaller打包安装文件、引用程序集和安装程序；
3、创建Compiled文件夹；
4、把生成的文件复制到Compiled文件夹；
5、删除Pyinstaller生成的缓存文件夹（build、dist）；
6、跳过注释，暂停。

如果运行出错，就把goto then后面（包括这堆注释）的全删了，把goto then改成pause。

:then
pause