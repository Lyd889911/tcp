using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpWinF
{
    public partial class Form1 : Form
    {
        TcpListener server = null;
        TcpClient client = null;
        List<string> list = new List<string>();
        int port = 0;
        int portTarget = 0;
        string ip = null;
        string ipTarget = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 发送消息给对方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string datastr = $"【{textBox4.Text}】：{textBox3.Text}";
            byte[] b = Encoding.Default.GetBytes(datastr);
            try
            {
                //连接服务端
                using(client = new TcpClient(ipTarget, portTarget))
                {
                    //得到这之间的网络数据流
                    using(NetworkStream stream = client.GetStream())
                    {
                        //像网络数据流写入数据，就是给服务端发请求
                        stream.Write(b, 0, b.Length);

                        //服务端响应了，会返回数据到网络数据流里面，客户端读取这个数据流，i是读取到了多少个字节
                        b = new byte[1];
                        int i = stream.Read(b, 0, b.Length);
                        //转换成字符串
                        string restr = Encoding.Default.GetString(b);

                        if (restr == "1")
                        {
                            label2.Text = "发送成功";
                            list.Add(datastr);
                            setListBox();
                            textBox3.Text = "";
                        }
                    }
                }

                //最后关闭
                //stream.Close();
                //client.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void setTcp()
        {
            Task task = Task.Run(TaskL);
        }
        /// <summary>
        /// 单独开一个任务监听请求
        /// </summary>
        private void TaskL()
        {
            try
            {
                //开始监听
                server = new TcpListener(IPAddress.Parse(ip), port);
                server.Start();

                byte[] bytes = new byte[2048];
                string data = null;

                //循环监听
                while (true)
                {
                    
                    using (TcpClient client = server.AcceptTcpClient())
                    {
                        //获取到数据流，在这个数据流里面可以读取出对方发来的消息
                        using(NetworkStream stream = client.GetStream())
                        {
                            int i = stream.Read(bytes, 0, bytes.Length);
                            data = Encoding.Default.GetString(bytes, 0, i);

                            //添加到list里面，刷新listbox
                            list.Add(data);
                            setListBox();

                            string str = "1";
                            byte[] rebyte = Encoding.Default.GetBytes(str);
                            stream.Write(rebyte, 0, rebyte.Length);
                        }

                    }

                }
            }
            catch
            {
                MessageBox.Show("异常");
            }
            finally
            {
                server.Stop();
            }
        }
        private void setListBox()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = list;
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            port = Convert.ToInt32(textBox2.Text.Trim());
            portTarget= Convert.ToInt32(textBox5.Text.Trim());
            ipTarget= textBox1.Text.Trim();
            ip = textBox6.Text.Trim();
            setTcp();//开始监听
            label8.Text = "启动成功";
        }
    }
}
