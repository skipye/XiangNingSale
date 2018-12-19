using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class StringOperation
    {
        #region "MD5加密"
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string MD5(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }

            if (code == 32)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }
            return strEncrypt;
        }
        #endregion
        # region 简单的加解密方法
        //字符串加密   
        public static string encode(string strText)
        {
            Byte[] Iv64 = { 11, 22, 33, 44, 55, 66, 77, 88 };
            Byte[] byKey64 = { 10, 20, 30, 40, 50, 60, 70, 80 };
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey64, Iv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //字符串解密   
        public static string decode(string strText)
        {
            Byte[] Iv64 = { 11, 22, 33, 44, 55, 66, 77, 88 };
            Byte[] byKey64 = { 10, 20, 30, 40, 50, 60, 70, 80 };
            Byte[] inputByteArray = new byte[strText.Length];
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey64, Iv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion
        #region 生成指定长度的字符串,即生成strLong个str字符串
        /// <summary>
        /// 生成指定长度的字符串,即生成strLong个str字符串
        /// </summary>
        /// <param name="strLong">生成的长度</param>
        /// <param name="str">以str生成字符串</param>
        /// <returns></returns>
        public static string StringOfChar(int strLong, string str)
        {
            string ReturnStr = "";
            for (int i = 0; i < strLong; i++)
            {
                ReturnStr += str;
            }
            return ReturnStr;
        }
        #endregion

        #region 截取指定长长的字符串，后面以"..."代替
        /// <summary>
        /// 截取指定长长的字符串，后面以"..."代替
        /// </summary>
        /// <param name="StrLong">截取长度</param>
        /// <param name="SourceStr">源字符串</param>
        /// <returns>截取后字符串</returns>
        public static string SubString(int StrLong, string SourceStr)
        {
            string ReturnStr = string.Empty;
            if (string.IsNullOrEmpty(SourceStr)) { return string.Empty; }
            if (SourceStr.Length > StrLong)
                ReturnStr = SourceStr.Substring(0, StrLong) + "...";
            else
                ReturnStr = SourceStr;

            return ReturnStr;
        }
        #endregion

        #region 字符串截取
        /// <summary>
        /// 左截取
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Left(string inputString, int len)
        {
            if (inputString.Length < len)
                return inputString;
            else
                return inputString.Substring(0, len) + "....";
        }
        /// <summary>
        /// 右截取
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Right(string inputString, int len)
        {
            if (inputString.Length < len)
                return inputString;
            else
                return inputString.Substring(inputString.Length - len, len);
        }
        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutTitle(string sText, int iLength, bool bl)
        {
            if (iLength < 1) return sText;
            byte[] b = System.Text.Encoding.Default.GetBytes(sText);
            double n = 0.0;
            int m = 0;
            bool l0 = false, l1 = false, l2 = false;
            for (int i = 0; i < b.Length; i++)
            {
                l0 = ((int)b[i] > 128);
                if (l0) i++;
                n += (l0 ? 1.0 : 0.5);
                if (n > iLength)
                {
                    string strOut = (l2 ? sText.Substring(0, m - 1) : sText.Substring(0, m - 2));
                    if (System.Text.Encoding.GetEncoding("GB2312").GetByteCount(strOut) + 2 > iLength * 2)
                        strOut = strOut.Substring(0, strOut.Length - 1);
                    if (bl)
                        strOut += "..";
                    return strOut;
                }
                m++;
                l2 = l1;
                l1 = l0;
            }
            return sText;
        }
        #endregion
        #region Html字符处理
        /// <summary>
        /// 除去所有的Html标记
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string StripHTML(string strHtml)
        {
            string strOutput = strHtml;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"<[^>]+>|</[^>]+>");
            strOutput = regex.Replace(strOutput, "");
            return strOutput;
        }
        public static string StripHTML2(string strHtml)
        {
            string strOutput = strHtml;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"<[^>]+>|</[^>]+>");
            strOutput = regex.Replace(strOutput, "");
            strOutput = System.Text.RegularExpressions.Regex.Replace(strOutput, "\\s*|\t|\r|\n", "");
            return strOutput;
        }

        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace(" ", "&nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "&#39;");
            theString = theString.Replace("\n", "<br/> ");
            return theString;
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public static string HtmlDecode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace(" &nbsp;", "  ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/> ", "\n");
            return theString;
        }
        #endregion
        #region 统计字符串sin在str中出现的次数
        /// <summary>
        /// 统计字符串sin在str中出现的次数
        /// </summary>
        /// <param name="str">长字符</param>
        /// <param name="sin">要统计的字符</param>
        /// <returns></returns>
        public static int GetSubStringCount(string str, string sin)
        {
            int i = 0;
            int ibit = 0;
            while (true)
            {
                ibit = str.IndexOf(sin, ibit);
                if (ibit > 0)
                {
                    ibit += sin.Length;
                    i++;
                }
                else
                {
                    break;
                }
            }
            return i;
        }
        #endregion
       
    }
}
