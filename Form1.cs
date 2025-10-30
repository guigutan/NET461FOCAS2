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
            Func001();
        }                
        private string  ConnMachine()
        {
            string res = "";
            string ip = "192.168.8.10";
            int port = 8193;
            int timeout = 3;

            ret = Focas1.cnc_allclibhndl3(ip, Convert.ToUInt16(port), timeout, out hndl);
            if (ret != Focas1.EW_OK)
            {
                res = $"设备未连接上\r\n错误代码: {ret}\r\n句柄: {hndl}";
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
            if (hndl != 0)
            {
                ret = Focas1.cnc_freelibhndl(hndl);
                if (ret == Focas1.EW_OK)
                {
                    result += "已断开连接";

                    hndl = 0;
                    button2.Enabled = false;
                }
                else
                {
                    result += $"断开连接失败，错误代码: {ret}";
                }
            }
            return result;
        }





        private short ret; // FOCAS函数返回值
        private ushort hndl;  // 机床连接句柄
          
        private void Func001()
        {
            
        }


       




    }
}
