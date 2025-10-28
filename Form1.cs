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

        // 导入FOCAS库函数
        [DllImport("FWLIB32.dll")]
        private static extern short cnc_machine(ushort FlibHndl, short axis, short length, ref ODBAXIS machine);

        // 机床位置数据结构体（对应文档中的ODBAXIS）
        [StructLayout(LayoutKind.Sequential)]
        public struct ODBAXIS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] // 最多支持32轴
            public long[] data;
        }




        // 导入读取宏变量的函数
        [DllImport("FWLIB32.dll")]
        private static extern short cnc_rdmacro(ushort FlibHndl, short number, short length, ref ODBM macro);

        // 宏变量数据结构体（对应文档中的ODBM）
        [StructLayout(LayoutKind.Sequential)]
        public struct ODBM
        {
            public long mcr_val;   // 宏变量数值
            public short dec_val;  // 小数点位数
        }



        public Form1()
        {
            InitializeComponent();
            // 初始化结构体数组
            machinePos.data = new long[32];
        }


        private short ret; // FOCAS函数返回值
        private ushort h;  // 机床连接句柄
        private ODBAXIS machinePos = new ODBAXIS(); // 机床位置数据




       
        private void button1_Click(object sender, EventArgs e)
        {
            string ip = "192.168.8.10";
            int port = 8193;
            int timeout = 3;

            ret = Focas1.cnc_allclibhndl3(ip, Convert.ToUInt16(port), timeout, out h);
            if (ret != Focas1.EW_OK)
            {
               
                textBox1.Text = $"设备未连接上\r\n错误代码: {ret}\r\n句柄: {h}";
            }
            else
            {
                textBox1.Text = "设备连接成功";
               
                // 连接成功后启用读取按钮
                button2.Enabled = true;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            fun002();
        }

        private void fun001()
        {
            if (h == 0)
            {
                MessageBox.Show("请先连接机床");
                return;
            }

            // 读取第2轴的机床位置（文档示例中常用2轴为例）
            // 参数说明：句柄、轴号(1开始)、数据长度(4+4*1)、位置数据结构体
            ret = cnc_machine(h, 2, 4 + 4 * 1, ref machinePos);

            if (ret == Focas1.EW_OK)
            {
                // 显示读取到的位置数据（原始值，实际应用需根据机床参数转换单位）
                MessageBox.Show($"机床第2轴位置: {machinePos.data[0]}\n" +
                                "注：数据为原始脉冲值，需根据机床参数转换为实际单位(mm/inch)");
            }
            else
            {
                MessageBox.Show($"读取失败，错误代码: {ret}");
            }
        }




        private ODBM macroData = new ODBM(); // 宏变量数据
        // 读取工件计数（假设存储在宏变量#100中）
        private void fun002()
        {
            if (h == 0)
            {
                textBox1.Text = "请先连接机床";                
                return;
            }

            for (int i=500;i<=600;i++) {

                short macroNumber = (short)i; // 宏变量编号（根据实际存储位置修改）
                short dataLength = 10;   // 数据长度（固定为10）

                ret = cnc_rdmacro(h, macroNumber, dataLength, ref macroData);

                if (ret == Focas1.EW_OK)
                {
                  
                    // 转换为实际数值（考虑小数点位数）
                    double countValue = macroData.mcr_val / Math.Pow(10, macroData.dec_val);

                    textBox1.Text += $"工件计数：{countValue}\r\n";

                }
                else
                {
                    textBox1.Text += $"读取失败，错误代码：{ret}\r\n";

                }


            }


            




        }







        private void button3_Click(object sender, EventArgs e)
        {
            if (h != 0)
            {
                ret = Focas1.cnc_freelibhndl(h);
                if (ret == Focas1.EW_OK)
                {
                    textBox1.Text += "已断开连接";
                   
                    h = 0;
                    button2.Enabled = false;
                }
                else
                {
                    textBox1.Text += $"断开连接失败，错误代码: {ret}";
                  
                }
            }
        }
    }
}
