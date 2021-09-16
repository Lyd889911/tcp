using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpC
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "9300CN管理员|COguest|CP28C17AAA-9BC4-4BE4-9490-1ADCC28BBFA3|AY1AZEAE5";
            byte[] data = Encoding.Default.GetBytes(str);

            //IPAddress remoteIP = IPAddress.Parse("106.55.10.168");//远程主机IP
            string IP = "127.0.0.1";//远程主机ip
            int Port = 13000;//远程主机端口

            //接收的数据
            string restr;
            byte[] redata = new byte[1024];

            TcpClient client = null;

            try
            {
                //连接服务端
                client = new TcpClient(IP, Port);
                //得到这之间的网络数据流
                NetworkStream stream = client.GetStream();
                //像网络数据流写入数据，就是给服务端发请求
                stream.Write(data, 0, data.Length);
                stream.Flush();
                Console.WriteLine($"发送地址：{IP}:{Port},数据：{str}");
                //服务端响应了，会返回数据到网络数据流里面，客户端读取这个数据流，i是读取到了多少个字节
                int i = stream.Read(redata, 0, redata.Length);
                //转换成字符串
                restr = Encoding.Default.GetString(redata);

                Console.WriteLine($"响应数据:{restr}");
                //最后关闭
                stream.Close();
                client.Close();
            }
            catch
            {
                Console.WriteLine($"异常！");
            }
        }
    }
}
