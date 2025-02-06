using System;
using System.Collections.Generic;
using System.Linq;

namespace YuriInstaller.MizukiTools
{

    /// <summary>这个异常用于该项目内的自定义功能。<br />
    /// This Exception happens when Liang Ruxuan doesn't love me.</summary>
    public class LRXDoesntLoveMeException : Exception
    {
        public LRXDoesntLoveMeException() : base()
        {
        }

        public LRXDoesntLoveMeException(string message) : base(message)
        {
        }

        public LRXDoesntLoveMeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class Bytes
    {
        private readonly long _value;
        public static implicit operator string(Bytes obj) => obj._value.ToString();
        public static implicit operator long(Bytes b) => b._value;
        public static implicit operator Bytes(long value) => new Bytes(value);

        public Bytes(long value)
        {
            _value = value;
        }

        public double ToKBytes() => (double)_value / 1024;

        public double ToMBytes() => (double)_value / (int)Math.Pow(1024, 2);

        public double ToGBytes() => (double)_value / (int)Math.Pow(1024, 3);

        public double ToTBytes() => (double)_value / (int)Math.Pow(1024, 4);

        public override string ToString() => _value.ToString();

        public static Bytes ParseFromKBytes(double a) => (long)a * 1024;

        public static Bytes ParseFromMBytes(double a) => (long)a * (int)Math.Pow(1024, 2);

        public static Bytes ParseFromGBytes(double a) => (long)a * (int)Math.Pow(1024, 3);

        public static Bytes ParseFromTBytes(double a) => (long)a * (int)Math.Pow(1024, 4);
    }

    /// <summary>用来生成命令行的类。</summary>
    public class CMDScript
    {
        protected List<string> _content;

        protected List<string> Content
        {
            get
            {
                var list = new List<string>(_content);
                if (!string.IsNullOrEmpty(CommandBeforeRun))
                {
                    list.Insert(0, CommandBeforeRun);
                }
                if (!EchoOnOff)
                {
                    list.Insert(0, "echo Off");
                }
                return list;
            }
        }

        /// <summary>始终插入在脚本前Echo后的命令。</summary>
        public string CommandBeforeRun { get; set; }

        /// <summary>若为false, 始终在最前面添加echo Off语句。</summary>
        public bool EchoOnOff { get; set; } = false;

        public CMDScript()
        {
            _content = new List<string>();
        }

        public CMDScript(string cmdBeforeRun) : this()
        {
            CommandBeforeRun = cmdBeforeRun;
        }

        public CMDScript(IEnumerable<string> commands)
        {
            _content = commands.ToList();
        }

        public CMDScript(string cmdBeforeRun, List<string> commands)
        {
            CommandBeforeRun = cmdBeforeRun;
            _content = commands;
        }

        /// <summary>清空脚本。</summary>
        public virtual void Clear() => _content.Clear();

        /// <summary>输出为字符串。</summary>
        public override string ToString() => ToScript("\n");

        /// <summary>输出CMD脚本，以自定义字符串拼接。</summary>
        public virtual string ToScript(string a) => string.Join<string>(a, Content);

        /// <summary>输出CMD脚本，以 & 拼接。</summary>
        public virtual string ToScript() => ToScript(" & ");

        /// <summary>输出CMD脚本，以 && 拼接。</summary>
        public virtual string ToAllDoneScript() => ToScript(" && ");

        /// <summary>添加注册表项。</summary>
        public virtual void AddAddRegCommand(string a) => AddCommand($"reg add \"{a}\" /reg:32");

        /// <summary>删除注册表项中的键。<br />
        /// 注：key参数结尾不能是斜杠。</summary>
        public virtual void AddDeleteRegKeyCommand(string key, string value) => AddCommand($"reg delete \"{key}\" /v \"{value}\" /f /reg:32");

        /// <summary>删除注册表项。</summary>
        public virtual void AddDeleteRegCommand(string a) => AddCommand($"reg delete \"{a}\" /f /reg:32");

        /// <summary>删除文件夹。</summary>
        public virtual void AddDeleteFolderCommand(string a) => AddCommand($"rd \"{a}\" /s/q");

        /// <summary>删除文件。</summary>
        public virtual void AddDeleteFileCommand(string a) => AddCommand($"del \"{a}\"");

        /// <summary>添加自定义命令。</summary>
        public virtual void AddCommand(string a) => _content.Add(a);

        /// <summary>转换为列表。</summary>
        public virtual List<string> ToList() => _content;

        /// <summary>转换为数组。</summary>
        public virtual string[] ToArray() => _content.ToArray();
    }
}