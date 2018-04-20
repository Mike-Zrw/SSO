using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AbsUserCenter.Tool
{
    public class MD5Lib
    {
        private MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();


        #region md5加密码

        public static string MD5(string str)
        {
            return EncryptMD5ToHexString(str, System.Text.Encoding.UTF8.CodePage);
        }
        public static string EncryptMD5ToHexString(string s, int EncodingCodePage)
        {
            if (s == null || s == "") return null;
            return ByteArrayToHexString(EncryptMD5ToByteArray(s, EncodingCodePage));
        }
        public static string ByteArrayToHexString(byte[] ba)
        {
            string r = "";
            string s = "";
            for (long l = 0; l < ba.Length; l++)
            {
                byte b = (byte)(ba.GetValue(l));
                s = Convert.ToString(b, 16);
                if (s.Length < 2) s = "0" + s;
                r += s;
            }
            return r;
        }
        public static byte[] EncryptMD5ToByteArray(string s, int EncodingCodePage)
        {
            return EncryptMD5ToByteArray(StringToByteArray(s, EncodingCodePage));
        }
        public static byte[] EncryptMD5ToByteArray(byte[] ba)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider oMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] r = oMD5.ComputeHash(ba);
            oMD5 = null;
            return r;
        }
        public static byte[] StringToByteArray(string s, int EncodingCodePage)
        {
            return System.Text.Encoding.GetEncoding(EncodingCodePage).GetBytes(s);
        }
        #endregion

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            string _encode = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                _encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                _encode = source;
            }
            return _encode;
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
 

        /// <summary>
        /// 生成MD5加密摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>MD5加密后</returns>
        public byte[] GenerateMD5(byte[] original)
        {
            byte[] keyhash = md5Provider.ComputeHash(original);
            return keyhash;
        }
        /// <summary>
        /// 生成MD5加密摘要
        /// </summary>
        /// <param name="strOriginal">字符串数据源</param>
        /// <returns>MD5加密后</returns>
        public string GenerateMD5(string strOriginal)
        {
            byte[] btemp = GenerateMD5(Encoding.UTF8.GetBytes(strOriginal));
            ////把加密后的字节转换成精度2的十六进制数据
            //StringBuilder ret = new StringBuilder();
            //foreach (byte b in btemp)
            //{
            //    ret.AppendFormat("{0:X2}", b);
            //}
            //return ret.ToString();
            return BitConverter.ToString(btemp).Replace("-", "");
        }


        public void DisposeMd5()
        {
            md5Provider.Clear();
        }
    }
}
