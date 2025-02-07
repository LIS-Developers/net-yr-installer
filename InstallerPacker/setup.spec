# -*- mode: python ; coding: utf-8 -*-

a = Analysis(
    ['Setup.py'],
    pathex=[],
    binaries=[],
    datas=[
        # 安装程序主程序
        ('..\\YuriInstaller\\bin\\setup.exe', 'temp'), 

        # 安装文件（请确保C盘有足够空间）
        ('..\\YuriInstaller\\setup.7z', 'temp'), 
        ('..\\YuriInstaller\\setup1.7z', 'temp'), 

        # 安装程序程序集引用配置
        ('..\\YuriInstaller\\bin\\Setup.exe.config', 'temp'), 

        # 安装程序依赖库（动态链接）
        ('..\\YuriInstaller\\bin\\x86', 'temp\\x86'), 
        ('..\\YuriInstaller\\bin\\x64', 'temp\\x64'), 
        ('..\\YuriInstaller\\bin\\Microsoft.Bcl.AsyncInterfaces.dll', 'temp'),
        ('..\\YuriInstaller\\bin\\Microsoft.Win32.Registry.dll', 'temp'),

        ('..\\YuriInstaller\\bin\\NAudio.Asio.dll', 'temp'),
        ('..\\YuriInstaller\\bin\\NAudio.Core.dll', 'temp'),
        ('..\\YuriInstaller\\bin\\NAudio.dll', 'temp'),
        ('..\\YuriInstaller\\bin\\NAudio.Midi.dll', 'temp'),
        ('..\\YuriInstaller\\bin\\NAudio.Wasapi.dll', 'temp'),
        ('..\\YuriInstaller\\bin\\NAudio.WinForms.dll', 'temp'),
        ('..\\YuriInstaller\\bin\\NAudio.WinMM.dll', 'temp'),

        ('..\\YuriInstaller\\bin\\SharpSevenZip.dll', 'temp'), 
        ('..\\YuriInstaller\\bin\\System.Buffers.dll', 'temp'), 
        ('..\\YuriInstaller\\bin\\System.IO.Pipelines.dll', 'temp'), 
        ('..\\YuriInstaller\\bin\\System.Memory.dll', 'temp'), 
        ('..\\YuriInstaller\\bin\\System.Numerics.Vectors.dll', 'temp'), 
        ('..\\YuriInstaller\\bin\\System.Runtime.CompilerServices.Unsafe.dll', 'temp'), 
        ('..\\YuriInstaller\\bin\\System.Security.AccessControl.dll', 'temp'),
        ('..\\YuriInstaller\\bin\\System.Security.Principal.Windows.dll', 'temp'),
        ('..\\YuriInstaller\\bin\\System.Text.Encodings.Web.dll', 'temp'), 
        ('..\\YuriInstaller\\bin\\System.Text.Json.dll', 'temp'), 
        ('..\\YuriInstaller\\bin\\System.Threading.Tasks.Extensions.dll', 'temp'), 
        ('..\\YuriInstaller\\bin\\System.ValueTuple.dll', 'temp')
    ],
    hiddenimports=[],
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=[],
    noarchive=False,
    optimize=2,
)
pyz = PYZ(a.pure)

exe = EXE(
    pyz,
    a.scripts,
    a.binaries,
    a.datas,
    [],
    name='Setup',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    upx_exclude=[],
    runtime_tmpdir=None,
    console=True,
    disable_windowed_traceback=False,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
    icon = '..\\YuriInstaller\\icon.ico',
)
