using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HTBInject.Utils
{
    class SafetyTool
    {
        public static string encryptKey = "(B&@";    //定义密钥 

        #region 加密字符串 
        public static string Encrypt(string str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象  
            byte[] key = Encoding.Unicode.GetBytes(encryptKey); //定义字节数组，用来存储密钥   
            byte[] data = Encoding.Unicode.GetBytes(str);//定义字节数组，用来存储要加密的字符串 
            MemoryStream MStream = new MemoryStream(); //实例化内存流对象     
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);  //向加密流中写入数据     
            CStream.FlushFinalBlock();              //释放加密流     
            return Convert.ToBase64String(MStream.ToArray());
        }

        #endregion

        #region 解密字符串  

        public static string Decrypt(string str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象   
            byte[] key = Encoding.Unicode.GetBytes(encryptKey); //定义字节数组，用来存储密钥   
            byte[] data = Convert.FromBase64String(str);//定义字节数组，用来存储要解密的字符串 
            MemoryStream MStream = new MemoryStream(); //实例化内存流对象     
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);      //向解密流中写入数据    
            CStream.FlushFinalBlock();               //释放解密流     
            return Encoding.Unicode.GetString(MStream.ToArray());       //返回解密后的字符串 
        }

        #endregion
    }
}
