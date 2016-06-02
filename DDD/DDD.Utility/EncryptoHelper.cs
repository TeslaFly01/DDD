using System;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;

namespace DDD.Utility
{
    /// <summary>
    /// 对称加密
    /// </summary>
    public class EncryptoHelper
    {
        //private readonly string IV = "kVuXrufS+K8=";
        private readonly string IV = string.Empty;
        //private readonly string Key = "vZix2Uaf5+gLi/sgUkRTy5AgGJBmJvWK";
        private readonly string Key = string.Empty;

        public EncryptoHelper()
        {
            this.IV = ConfigurationManager.AppSettings["TripleDES_IV"];
            this.Key = ConfigurationManager.AppSettings["TripleDES_Key"];
        }

        public EncryptoHelper(string IV, string Key)
        {
            this.IV = IV;
            this.Key = Key;
        }

        /// <summary>
        /// 获取加密后的字符串
        /// </summary>
        /// <param name="inputValue">输入值.</param>
        /// <returns></returns>
        public string GetEncryptedValue(string inputValue)
        {
            TripleDESCryptoServiceProvider provider = this.GetCryptoProvider();

            // 创建内存流来保存加密后的流
            MemoryStream mStream = new MemoryStream();

            // 创建加密转换流
            CryptoStream cStream = new CryptoStream(mStream,

            provider.CreateEncryptor(), CryptoStreamMode.Write);
            // 使用UTF8编码获取输入字符串的字节。
            byte[] toEncrypt = new UTF8Encoding().GetBytes(inputValue);

            // 将字节写到转换流里面去。
            cStream.Write(toEncrypt, 0, toEncrypt.Length);
            cStream.FlushFinalBlock();

            // 在调用转换流的FlushFinalBlock方法后，内部就会进行转换了,此时mStream就是加密后的流了。
            byte[] ret = mStream.ToArray();

            // Close the streams.
            cStream.Close();
            mStream.Close();

            //将加密后的字节进行64编码。
            return Convert.ToBase64String(ret);
        }

        /// <summary>
        /// 获取加密服务类
        /// </summary>
        /// <returns></returns>
        private TripleDESCryptoServiceProvider GetCryptoProvider()
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();

            provider.IV = Convert.FromBase64String(IV);
            provider.Key = Convert.FromBase64String(Key);

            return provider;

        }

        /// <summary>
        /// 获取解密后的值
        /// </summary>
        /// <param name="inputValue">经过加密后的字符串.</param>
        /// <returns></returns>
        public string GetDecryptedValue(string inputValue)
        {
            TripleDESCryptoServiceProvider provider = this.GetCryptoProvider();
            byte[] inputEquivalent = Convert.FromBase64String(inputValue);

            // 创建内存流保存解密后的数据
            MemoryStream msDecrypt = new MemoryStream();

            // 创建转换流。
            CryptoStream csDecrypt = new CryptoStream(msDecrypt,
            provider.CreateDecryptor(),
            CryptoStreamMode.Write);

            csDecrypt.Write(inputEquivalent, 0, inputEquivalent.Length);
            csDecrypt.FlushFinalBlock();

            csDecrypt.Close();

            //获取字符串。
            return new UTF8Encoding().GetString(msDecrypt.ToArray());
        }

    }
}
