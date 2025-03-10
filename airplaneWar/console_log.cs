using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

//此文件存放的是窗口控件的回调函数

namespace Untrie
{
    public partial class Form1 : Form
    {

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();//关联一个控制台窗口
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();//释放关联的控制台窗口

        public Form1()
        {

            AllocConsole(); //关联一个控制台窗口用于显示信息

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //窗口关闭前 回调函数
            FreeConsole();//释放关联的控制台，不然会报错

        }

    }
}