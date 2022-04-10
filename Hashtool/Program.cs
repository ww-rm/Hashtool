using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Hashtool
{
    static class Program
    {
        public static string SrcCodeURL { get { return "https://github.com/ww-rm/Hashtool"; } }
        public static string IntroStr { get { return "本程序是一个使用C#语言开发的开源哈希值计算工具，可以对多个文件的常见哈希值进行计算。"; } }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWnd());
        }
    }
}
