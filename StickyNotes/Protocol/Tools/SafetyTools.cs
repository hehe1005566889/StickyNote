using FStore.NetWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace ProjectFChat.Network.Utils
{
    /// <summary>
    /// DES加密工具
    /// </summary>
    class GlobalDES
    {

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="input">输入的数据</param>
        /// <param name="key">密钥</param>
        /// <returns>输出的数据</returns>
        public static byte[] Encrypt(byte[] input, byte[] key)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   
            MemoryStream MStream = new MemoryStream();   
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.WriteAsync(input, 0, input.Length);    
            CStream.FlushFinalBlock();
            CStream.Dispose();
            CStream.Close();
            return MStream.ToArray();
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <param name="key">密钥</param>
        /// <returns>输出数据</returns>
        public static byte[] Decrypt(byte[] input, byte[] key)
        {
        //    password.GetOut(key);
        //    MessageBox.Show("." + key[key.Length - 1]);
        //    password.GetOut(input);
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider(); 
            MemoryStream MStream = new MemoryStream();
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.WriteAsync(input, 0, input.Length); 
            CStream.FlushFinalBlock(); 
           // CStream.Close();
            Console.WriteLine(MStream.Length);
            return MStream.ToArray();  //返回解密后的字符串 
        }
    }
    /// <summary>
    /// AES加密工具
    /// </summary>
    class GlobalAES
    {
        private static byte[] Operation(byte[] src, byte[] Key, bool isEncrypt)
        {
            if(Key.Length <= 0) return null;

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform;

            if (isEncrypt) cTransform = rm.CreateEncryptor();
            else cTransform = rm.CreateDecryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(src, 0, src.Length);
            return resultArray;
        }

        /// <summary>
        /// AES加密方法
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="Key">密钥</param>
        /// <returns>返回</returns>
        public static byte[] Encrypt(byte[] input, byte[] Key)
        {
            return Operation(input, Key, true);
        }

        /// <summary>
        /// AES解密方法
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="Key">密钥</param>
        /// <returns>输出</returns>
        public static byte[] Decrypt(byte[] input, byte[] Key)
        {
            return Operation(input, Key, false);
        }
    }
    /// <summary>
    /// MD5计算工具
    /// </summary>
    class MD5Helper
    {
        /// <summary>
        /// 计算md5 hash 摘要
        /// </summary>
        /// <param name="input">输入的内容</param>
        /// <returns>输出的内容</returns>
        public static byte[] GetMD5Hash(byte[] input)
        {
            StringBuilder sb = new StringBuilder();
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                foreach(byte dat in md5.ComputeHash(input)) 
                    sb.Append(dat.ToString("X2"));
            }
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        /// <summary>
        /// 验证MD5哈希摘要
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="hash">输入hash值</param>
        /// <returns>是否一致</returns>
        public static bool VerifyMD5Hash(byte[] input, byte[] hash)
        {
            byte[] hashOfInput = GetMD5Hash(input);
            if (hashOfInput.Equals(hash)) return true;
            else return false;
        }
    }

    class Helper
    {
        public static string encryptKey = "*Z#@";    //定义密钥 

        #region 加密字符串 

        public static byte[] EncryptDll(byte[] bt)
        {
            System.Security.Cryptography.DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象  
            byte[] key = Encoding.Unicode.GetBytes(encryptKey); //定义字节数组，用来存储密钥   
            byte[] data = bt;//定义字节数组，用来存储要加密的字符串 
            MemoryStream MStream = new MemoryStream(); //实例化内存流对象     
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);  //向加密流中写入数据     
            CStream.FlushFinalBlock();              //释放加密流     
            return MStream.ToArray();
        }

        #endregion

        #region 解密字符串  

        public static byte[] DecryptDll(byte[] str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象   
            byte[] key = Encoding.Unicode.GetBytes(encryptKey); //定义字节数组，用来存储密钥   
            byte[] data = str;//定义字节数组，用来存储要解密的字符串 
            MemoryStream MStream = new MemoryStream(); //实例化内存流对象     
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);      //向解密流中写入数据    
            CStream.FlushFinalBlock();               //释放解密流     
            return MStream.ToArray();       //返回解密后的字符串 
        }

        #endregion
    }

    /// <summary>
    /// 辅助方法
    /// </summary>
    class safetyHelper
    {
        /// <summary>
        /// 转换byte数组到string
        /// </summary>
        /// <param name="input">输入byte数组</param>
        /// <returns>输出string</returns>
        public static string GetString(byte[] input)
        {
            return Encoding.UTF8.GetString(input);
        }

        /// <summary>
        /// 转换string到byte数组
        /// </summary>
        /// <param name="input">输入string</param>
        /// <returns>输出byte数组</returns>
        public static byte[] GetBytes(string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }
    }
}
