

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 
using Tamir.SharpSsh;
using Tamir.SharpSsh.jsch;
namespace SSHTestCSharp
{
    class Program : Scp
    {
        void mysend()
        {
            Channel channel = null;
            Stream server = null;
            bool m_cancelled = false;
            string dir="/root";
            string localPath = "D:\\hello.txt";
            string remotePath = "/root/abc.txt";
            SCP_ConnectTo(out channel, out server, remotePath, m_cancelled);
            SCP_SendFile(server, localPath, remotePath);
            channel.disconnect();
           
        }
        static void testScp()
        {
            Scp s = new Scp("192.168.0.3", "root","123456");
            s.Connect();
           
     
            if (s.Connected)
            {
                Console.WriteLine("connect server success");
                s.From("/root/hello.txt", "D:\\");
            }
        }
        static void Main(string[] args)
        {
            testScp();
            try
            {
                string host = "192.168.0.3";
                string user = "root";
                string pass = "123456";
 
                Console.WriteLine("主机地址: {0}", host);
                Console.WriteLine("登陆用户: {0}", user);
                Console.WriteLine("登录密码: {0}", pass);
 
                SshShell shell = new SshShell(host, user);
                shell.Password = pass;
 
                shell.RedirectToConsole();
                Console.Write("正在连接...");
                shell.Connect();
                Console.WriteLine("连接完毕！");
                Console.WriteLine("=========");
 
                while (shell.ShellOpened)
                {
                    System.Threading.Thread.Sleep(500);
                }
 
                Console.WriteLine("=========");
                Console.WriteLine("断开连接中...");
                shell.Close();
                Console.WriteLine("断开完毕");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
 
            Console.Write("按任意键继续...");
            Console.ReadKey();
            Console.WriteLine("\b");
            Environment.Exit(0);
        }
    }
}