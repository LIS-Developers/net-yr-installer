# -*- coding:utf-8 -*-
import sys
import os
from time import sleep
from win32 import win32api#, win32gui, win32print
from win32.lib import win32con
# # from win32.win32api import GetSystemMetrics
# import win32con
import re
import winreg
import webbrowser

# 记得先pip install -r requirements.txt
# 如果你不想支持win7就用高版本python，支持win7最高不要超过Python3.8.6，为了防止某些精简系统连python都启动不了，可以用Python3.4以下。
# 若您使用的是windows XP系统无法使用的Python版本，用户需安装VC++ 2015运行库。
# Python版本高了win7会打不开
# 该脚本（打包器）很重要，用来把安装器的一大堆dll引用打包到一起。

# 这是作者邮箱，若安装出现问题请找他反馈。若使用请修改为自己的邮箱。
email = "2068651347@qq.com"
# 这是安装程序崩溃日志
exceptFile = "except.txt"

def check_dotnet_4_8_installed():
    """判断是否缺少net48库"""
    try:
        # 打开.NET Framework的注册表键
        key = winreg.OpenKey(winreg.HKEY_LOCAL_MACHINE,
                             "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full")
        # 获取版本号的值
        version, _ = winreg.QueryValueEx(key, "Release")
        winreg.CloseKey(key)
        #.NET 4.8的版本号（Release值）判断
        if version >= 528040:
            return True
        else:
            return False
    except FileNotFoundError:
        return False

"""安装包框架打包"""
WindowedMode = False
a = None
argv = ""
  
def clear():
    """清除临时文件夹里的东西。"""
    print("删除临时目录。")
    for path in sys.path:
        if re.match(r'^_MEI\d+$', os.path.basename(path)):
            if os.path.exists(path):
                temppath = os.path.join(path, "Temp")
                
                # 清除安装文件，因为这玩意最占空间了
                try:
                    os.remove(temppath)
                except Exception: # Exception as e:
                    pass #print("出错了！（>_<）错误：I %s" % str(e))
                else:
                    print("已删除 %s" % temppath)
                
                # 清除其他临时文件
                try:
                    os.remove(path)
                except Exception:
                    pass
                else:
                    print("已删除 %s" % path)

"""
def get_real_resolution():
        \"\"\"获取真实的分辨率\"\"\"
        hDC = win32gui.GetDC(0)
        # 长
        w = win32print.GetDeviceCaps(hDC, win32con.DESKTOPHORZRES)
        # 宽
        h = win32print.GetDeviceCaps(hDC, win32con.DESKTOPVERTRES)
        return w, h
"""
    
# 生成资源文件目录访问路径
def resource_path(relative_path):
    """获取资源文件的路径"""
    if getattr(sys, 'frozen', False):  # 是否 Bundle Resource
        base_path = sys._MEIPASS
    else:
        base_path = os.path.abspath(".")
    return os.path.join(base_path, relative_path)

if len(sys.argv) > 1:
    argv = " ".join(sys.argv[1:])


"""
WindowedMode = "-win" in sys.argv
if not WindowedMode:
    a = get_real_resolution()
"""

try:
    filename = resource_path(os.path.join("temp", "setup.exe"))

    print("Unpacking files...")
    print("Copyright © 2024 Shimada Mizuki. All Rights Reserved. QQ: 2068651347")
        
    # 提示缺少运行库
    if not check_dotnet_4_8_installed():
        if win32api.MessageBox(0, "您的电脑目前还未安装.NET Framework 4.8，是否要在线安装？\n\n\
        如不安装程序可能无法运行。", "发现游戏运行所必需的环境", win32con.MB_OKCANCEL) == win32con.IDOK:
            webbrowser.open("https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/thank-you/net48-offline-installer")
            raise RuntimeError("没有运行库！")

    print("请不要在安装完成之前就把我关掉哦！")

    # 运行setup.exe
    os.system("%s %s" % (filename, argv))
    
except Exception as e:
    print("出错了！（>_<）错误：S %s" % str(e))
    
finally:
    try:
        exceptFile = resource_path(os.path.join("temp", exceptFile))
        if os.path.exists(exceptFile):
            if win32api.MessageBox(0, "检测到安装出现异常，是否向%s报告问题？" % email, "出现问题", win32con.MB_OKCANCEL) == win32con.IDOK:
                os.system(exceptFile)

        """
        if not WindowedMode:
            if get_real_resolution() != a:
                dm = win32api.EnumDisplaySettings(None, 0)
                dm.PelsHeight = a[1]
                dm.PelsWidth = a[0]
                win32api.ChangeDisplaySettings(dm, 0)
        """

        sleep(2)
        clear()
    except Exception as e:
        print("出错了！（>_<）错误：O %s" % str(e))
        os.system("pause")