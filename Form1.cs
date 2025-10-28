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

namespace NET461FOCAS2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private short ret; // FOCAS函数返回值，必须是short类型
        private ushort h;     // 机床连接句柄，int类型
        private void button1_Click(object sender, EventArgs e)
        {
            string ip = "192.168.8.10";
            int port = 8193;
            int timeout = 3; 
         

            this.ret = Focas1.cnc_allclibhndl3(ip, Convert.ToUInt16(port), Convert.ToInt32(timeout), out h);
            if (this.ret != Focas1.EW_OK)
            {
                //设备未连接上
                MessageBox.Show("设备未连接上\r\n"+ this.ret.ToString()+ "\r\n" + h.ToString());
            }
            else if (this.ret == Focas1.EW_OK)
            {
                MessageBox.Show("设备连接成功");
               
            }



        }
    }
}
