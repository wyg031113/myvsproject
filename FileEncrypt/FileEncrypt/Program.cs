using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Security.Cryptography;
using System;
namespace FileEncrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            RijndaelManaged rij = new RijndaelManaged();
            rij.KeySize = 128;
            byte[] result = Encoding.Default.GetBytes("HELLO,WORD");    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            //rij.Key = new byte[16];
            int i;
            
            //for (i = 0; i < 16; i++)
            //    rij.Key[i] = output[i];
            byte[] hello = rij.Key;
            rij.Key = output;
            rij.IV = md5.ComputeHash(output);
            string fp = @"F:\TroubleIsAFriend.mp3";
            string sPhysicalFilePath =fp+".AES";
            string fw = @"F:\DecTroubleIsAFriend.mp3";
            Console.WriteLine("Encrypting begin...");
            encryption(rij, fp, sPhysicalFilePath);
            decryption(rij, sPhysicalFilePath, fw);

        }
        //用于加密的函数
        public static void encryption(RijndaelManaged rij, string readfile, string writefile)
        {
            try
            {
                byte[] key = rij.Key;
                byte[] iv = rij.IV;
                byte[] buffer = new byte[4096];


                Rijndael crypt = Rijndael.Create();
                ICryptoTransform transform = crypt.CreateEncryptor(key, iv);
                //写进文件
                FileStream fswrite = new FileStream(writefile, FileMode.Create);
                CryptoStream cs = new CryptoStream(fswrite, transform, CryptoStreamMode.Write);
                //打开文件
                FileStream fsread = new FileStream(readfile, FileMode.Open);
                int length;
                //while ((length = fsread.ReadByte()) != -1)
                //cs.WriteByte((byte)length);
                while ((length = fsread.Read(buffer, 0, 4096)) > 0)
                    cs.Write(buffer, 0, (int)length);

                fsread.Close();
                cs.Close();
                fswrite.Close();
                Console.WriteLine("Encrypt Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("Encrypt Faile" + e.ToString());
            }
        }
        //用于解密的函数
        public static void decryption(RijndaelManaged rij, string readfile, string writefile)
        {
            try
            {
                byte[] key = rij.Key;
                byte[] iv = rij.IV;
                byte[] buffer = new byte[4096];
                Rijndael crypt = Rijndael.Create();
                ICryptoTransform transform = crypt.CreateDecryptor(key, iv);
                //读取加密后的文件 
                FileStream fsopen = new FileStream(readfile, FileMode.Open);
                CryptoStream cs = new CryptoStream(fsopen, transform, CryptoStreamMode.Read);
                //把解密后的结果写进文件
                FileStream fswrite = new FileStream(writefile, FileMode.OpenOrCreate);

                int length;
                //while ((length = cs.ReadByte()) != -1)
                //fswrite.WriteByte((byte)length);
                while ((length = cs.Read(buffer, 0, 4096)) > 0)
                    fswrite.Write(buffer, 0, (int)length);
                fswrite.Close();
                cs.Close();
                fsopen.Close();
                Console.WriteLine("Decrypt Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("Decrypt Failed" + e.ToString());
            }
        }
    }
}
