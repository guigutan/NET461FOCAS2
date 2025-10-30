using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Focas1;

namespace NET461FOCAS2
{
    public partial class Form1 : Form
    { 
        public Form1()
        {
            InitializeComponent();          
        }

        private short ret; // FOCAS函数返回值
        private ushort h;  // 机床连接句柄
     

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = ConnMachine();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = ConnSotp();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Fun01();
        }


        private string  ConnMachine()
        {
            string res = "";
            string ip = "192.168.8.10";
            int port = 8193;
            int timeout = 3;

            ret = Focas1.cnc_allclibhndl3(ip, Convert.ToUInt16(port), timeout, out h);
            if (ret != Focas1.EW_OK)
            {
                res = $"设备未连接上\r\n错误代码: {ret}\r\n句柄: {h}";
            }
            else
            {
                res = "设备连接成功";               
                button2.Enabled = true;
            }
            return res;
        }

        private string ConnSotp()
        {
            string result = "";
            if (h != 0)
            {
                ret = Focas1.cnc_freelibhndl(h);
                if (ret == Focas1.EW_OK)
                {
                    result += "已断开连接";

                    h = 0;
                    button2.Enabled = false;
                }
                else
                {
                    result += $"断开连接失败，错误代码: {ret}";
                }
            }
            return result;
        }

        private void Fun01() 
        {
           
        }

        /// <summary>
        /// 读取指定工具组的寿命计数（使用FWLIB32.cs中的结构体）
        /// </summary>
        private void ReadToolLifeCount(short toolGroupNumber)
        {
            if (h == 0)
            {
                textBox1.Text = "请先连接机床\r\n";
                return;
            }

            // 直接使用FWLIB32中的ODBTLIFE3
            Focas1.ODBTLIFE3 countData;
            short ret = Focas1.cnc_rdcount(h, toolGroupNumber, out countData);//参数 3 不可与关键字“out”一起传递

            if (ret == Focas1.EW_OK)
            {
                textBox1.Text += $"工具组{countData.datano}计数：{countData.data}\r\n";
            }
            else
            {
                textBox1.Text += $"读取失败，错误码：{ret}\r\n";
            }
        }












    }
}
