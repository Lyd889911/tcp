using System;
using System.Net;
using System.Net.Sockets;

namespace TcpS
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            int port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            try
            {
                server = new TcpListener(localAddr, port);
                server.Start();
                byte[] bytes = new byte[1024];
                string data = null;
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("连接上了!");
                    NetworkStream stream = client.GetStream();
                    int i = stream.Read(bytes, 0, bytes.Length);

                    data = System.Text.Encoding.Default.GetString(bytes, 0, i);
                    Console.WriteLine("服务端接收请求: {0}", data);

                    string str = "响应数据1";
                    bytes = System.Text.Encoding.Default.GetBytes(str);
                    stream.Write(bytes, 0, bytes.Length);
                    Console.WriteLine($"响应数据:{str}");

                    client.Close();
                }
            }
            catch
            {
                Console.WriteLine("异常");
            }
            finally
            {
                server.Stop();
            }

        }
    }
}
