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
            Func005();
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
                res = "设备连接成功\r\n";               
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
            // 检查是否已建立连接
            if (hndl == 0)
            {
                MessageBox.Show("请先连接机床", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 定义并初始化存储进给速度的结构体（按值传递需确保初始化）
                ODBACT actfData = new ODBACT();
                
                // 关键修正：移除ref和out，直接传递结构体实例
                ret = Focas1.cnc_actf(hndl, actfData);

                // 处理返回结果
                if (ret == Focas1.EW_OK)
                {
                    // 进给速度数据在data字段中
                    textBox1.AppendText($"data: {actfData.data} \r\n");
                    foreach (short dum in actfData.dummy)
                    {
                        textBox1.AppendText($"{dum}-");
                    }
                    textBox1.AppendText($"\r\n");
                }
                else
                {
                    textBox1.AppendText($"读取进给速度失败，错误代码: {ret}\r\n");
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText($"操作出错: {ex.Message}\r\n");
            }
        }
        private void Func002()
        {
            // 检查是否已建立连接
            if (hndl == 0)
            {
                MessageBox.Show("请先连接机床", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                
                ODBTLIFE3 mydata = new  ODBTLIFE3();
                
                ret = Focas1.cnc_rdcount(hndl,1, mydata);

                // 处理返回结果
                if (ret == Focas1.EW_OK)
                {
                    // 进给速度数据在data字段中
                    textBox1.AppendText($"data: {mydata.data} \r\n");
                    textBox1.AppendText($"datano: {mydata.datano} \r\n");
                    textBox1.AppendText($"dummy: {mydata.dummy} \r\n");                   
                }
                else
                {
                    textBox1.AppendText($"错误代码（{ret}）: {GetErrorMessage(ret)}\r\n");
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText($"操作出错: {ex.Message}\r\n");
            }
        }
        private void Func003()
        {
            // 检查是否已建立连接
            if (hndl == 0)
            {
                MessageBox.Show("请先连接机床", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                ODBNC_1 mydata = new ODBNC_1();

                //若需要二进制数值（便于直接计算）：a=0，b=12。
                //若需要 ASCII 字符串（便于显示）：a = 1，b = 31。
                //这两个参数的取值是固定的，需严格匹配，否则可能导致数据读取错误或函数调用失败。
                ret = Focas1.cnc_rdproginfo(hndl, 1,31, mydata);

                // 处理返回结果
                if (ret == Focas1.EW_OK)
                {
                   
                    textBox1.AppendText($"reg_prg: {mydata.reg_prg} \r\n");
                    textBox1.AppendText($"unused_mem: {mydata.unused_mem} \r\n");
                    textBox1.AppendText($"used_mem: {mydata.used_mem} \r\n");
                    textBox1.AppendText($"unreg_prg: {mydata.unreg_prg} \r\n");
                }
                else
                {
                    textBox1.AppendText($"错误代码（{ret}）: {GetErrorMessage(ret)}\r\n");
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText($"操作出错: {ex.Message}\r\n");
            }
        }
        private void Func004()
        {
            // 检查是否已建立连接
            if (hndl == 0)
            {
                MessageBox.Show("请先连接机床", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                ODBNC_2 mydata = new ODBNC_2();

                //若需要二进制数值（便于直接计算）：a=0，b=12。
                //若需要 ASCII 字符串（便于显示）：a = 1，b = 31。
                //这两个参数的取值是固定的，需严格匹配，否则可能导致数据读取错误或函数调用失败。
                ret = Focas1.cnc_rdproginfo(hndl, 1, 31, mydata);

                // 处理返回结果
                if (ret == Focas1.EW_OK)
                {
                    foreach (char asc in mydata.asc)
                    {
                        textBox1.AppendText($"{asc} \r\n");
                    }  
                   
                }
                else
                {
                    textBox1.AppendText($"错误代码（{ret}）: {GetErrorMessage(ret)}\r\n");
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText($"操作出错: {ex.Message}\r\n");
            }
        }


        private void Func005()
        {
            string _toolGroupNum =textBox2.Text.Trim();
            short toolGroupNum = short.Parse(_toolGroupNum);
            Focas1.ODBTLIFE3 toolNum = new Focas1.ODBTLIFE3();
            Focas1.ODBTLIFE3 toolCnt = new Focas1.ODBTLIFE3();
            Focas1.ODBTLIFE3 toolLife = new Focas1.ODBTLIFE3();

            //(1)库句柄；   (2)a刀组号；[in]   (3) ODBTLIFE3 b[out]相对应组内的刀具数量；
            short ret1 = Focas1.cnc_rdntool(hndl, toolGroupNum, toolNum);
            short ret2 = Focas1.cnc_rdcount(hndl, toolGroupNum, toolCnt);
            short ret3 = Focas1.cnc_rdlife(hndl, toolGroupNum, toolLife);


            textBox1.AppendText($"data：{toolNum.data.ToString()}  datano：{toolNum.datano.ToString()}\r\n");
            textBox1.AppendText($"data：{toolCnt.data.ToString()}  datano：{toolCnt.datano.ToString()}\r\n");
            textBox1.AppendText($"data：{toolLife.data.ToString()}  datano：{toolLife.datano.ToString()}\r\n");





        }




    }
}
