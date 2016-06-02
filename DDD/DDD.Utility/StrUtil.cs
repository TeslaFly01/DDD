using System;
using System.Linq;
using System.Drawing;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing.Imaging;
using System.Text;
using System.Web.Security;
using System.Collections.Generic;
using DDD.Utility;

namespace DDD.Utility
{
    /// <summary>
    /// 对一些字符串进行操作的类
    /// </summary>
    public class StrUtil : PageBaseUtil
    {

        private static string passWord;	//加密字符串

        /// <summary>
        /// 判断输入是否数字
        /// </summary>
        /// <param name="num">要判断的字符串</param>
        /// <returns></returns>
        static public bool VldInt(string num)
        {
            #region
            try
            {
                Convert.ToInt32(num);
                return true;
            }
            catch { return false; }
            #endregion
        }


        /// <summary>
        /// 修改特殊字符
        /// </summary>
        /// <param name="str">要替换的字符串</param>
        /// <returns></returns>
        static public string CheckStr(string str)
        {
            #region
            return str.Replace("&", "&amp;").Replace("'", "&apos;").Replace(@"""", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace(" where ", " wh&#101;re ").
                Replace(" select ", " sel&#101;ct ").Replace(" insert ", " ins&#101;rt ").Replace(" create ", " cr&#101;ate ").Replace(" drop ", " dro&#112 ").
                Replace(" alter ", " alt&#101;r ").Replace(" delete ", " del&#101;te ").Replace(" update ", " up&#100;ate ").Replace(" or ", " o&#114; ").Replace("\"", @"&#34;")
                .Replace("[ft^", "<img src='" + PageBaseUtil.WebAppPath() + "images/FileTypeIcon/").Replace("^ft]", ".gif' />");
            #endregion
        }

        /// <summary>
        /// 恢复特殊字符
        /// </summary>
        /// <param name="str">要替换的字符串</param>
        /// <returns></returns>
        static public string UnCheckStr(string str)
        {
            #region
            return str.Replace("&amp;", "&").Replace("&apos;", "'").Replace("&quot;", @"""").Replace("&lt;", "<").Replace("&gt;", ">").Replace(" wh&#101;re ", " where ").
                Replace(" sel&#101;ct ", " select ").Replace(" ins&#101;rt ", " insert ").Replace(" cr&#101;ate ", " create ").Replace(" dro&#112 ", " drop ").
                Replace(" alt&#101;r ", " alter ").Replace(" del&#101;te ", " delete ").Replace(" up&#100;ate ", " update ").Replace(" o&#114; ", " or ").Replace(@"&#34;", "\"");
            #endregion
        }


        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(string theString)
        {
            if (string.IsNullOrEmpty(theString))
                return string.Empty;
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "&#39;");
            theString = theString.Replace("\n", "<br/> ");
            return theString;
        }

        /// <summary>
        /// 替换指定字符串长度html中的特殊字符
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string HtmlEncodeLength(string theString, int maxLength)
        {
            if (string.IsNullOrEmpty(theString))
                return string.Empty;
            if (theString.Length > maxLength)
                theString = theString.Substring(0, maxLength);
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "&#39;");
            theString = theString.Replace("\n", "<br/> ");
            return theString;
        }


        /// <summary>
        /// 恢复html中的特殊字符(编辑文本框及Excel显示时调用，web页面显示不需要调用)
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public static string HtmlDiscode(string theString)
        {
            if (string.IsNullOrEmpty(theString))
                return string.Empty;
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace(" &nbsp;", "  ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/> ", "\n");
            return theString;
        }

        /// <summary>
        /// 过滤指定长度的输入信息（一般用于用户名、文章标题等过滤）
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>
        public static string ChkInputTextLength(string text, int maxLength)
        {
            #region

            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = text.Trim();
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            text = text.Replace("'", "''");
            return text;
            #endregion
        }

        /// <summary>
        /// 过滤输入信息
        /// </summary>
        /// <param name="text">内容</param>
        /// <returns></returns>
        public static string ChkInputText(string text)
        {
            #region

            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = text.Trim();
            text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            text = text.Replace("'", "''");
            return text;
            #endregion
        }

        /// <summary>
        /// 生成随机数，并写cookie
        /// </summary>
        /// <returns></returns>
        static public string GenerateCheckCode()
        {
            #region
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }

            HttpContext.Current.Response.Cookies.Add(new HttpCookie("CheckCode", checkCode));

            return checkCode;
            #endregion
        }

        /// <summary>
        /// 获取汉字第一个拼音
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public string getSpells(string input)
        {
            #region
            int len = input.Length;
            string reVal = "";
            for (int i = 0; i < len; i++)
            {
                reVal += getSpell(input.Substring(i, 1));
            }
            return reVal;
            #endregion
        }

        /// <summary>
        /// 汉字编码转换,解决IE地址栏中文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public string Strencode(string input)
        {
            #region
            return System.Web.HttpUtility.UrlEncode(ChkInputTextLength(input, 100));
            #endregion
        }

        static public string getSpell(string cn)
        {
            #region
            byte[] arrCN = Encoding.Default.GetBytes(cn);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "?";
            }
            else return cn;
            #endregion
        }


        /// <summary>
        /// 半角转全角
        /// </summary>
        /// <param name="BJstr"></param>
        /// <returns></returns>
        static public string GetQuanJiao(string BJstr)
        {
            #region
            char[] c = BJstr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 0)
                    {
                        b[0] = (byte)(b[0] - 32);
                        b[1] = 255;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }

            string strNew = new string(c);
            return strNew;

            #endregion
        }

        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="QJstr"></param>
        /// <returns></returns>
        static public string GetBanJiao(string QJstr)
        {
            #region
            char[] c = QJstr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            string strNew = new string(c);
            return strNew;
            #endregion
        }

        #region 加密的类型
        /// <summary>
        /// 加密的类型
        /// </summary>
        public enum PasswordType
        {
            SHA1,
            MD5
        }
        #endregion


        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="PasswordString">要加密的字符串</param>
        /// <param name="PasswordFormat">要加密的类别</param>
        /// <returns></returns>
        static public string EncryptPassword(string PasswordString, string PasswordFormat)
        {
            #region
            switch (PasswordFormat)
            {
                case "SHA1":
                    {
                        passWord = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "SHA1");
                        break;
                    }
                case "MD5":
                    {
                        passWord = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "MD5");
                        break;
                    }
                default:
                    {
                        passWord = string.Empty;
                        break;
                    }
            }
            return passWord;
            #endregion
        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Is32or16">返回32或16位</param>
        /// <param name="toLower">是否转换成小写</param>
        /// <returns></returns>
        public static string Md5(string str, bool Is32or16, bool toLower)
        {
            string re = string.Empty;
            try
            {
                re = Is32or16 ? FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5") : FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
                if (toLower) re = re.ToLower();
            }
            catch (Exception)
            {
            }
            return re;
        }

        /// <summary>
        /// 是否是10进制小数
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string Value)
        {
            try
            {
                decimal i = Convert.ToDecimal(Value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是大于0的10进制小数
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsPosNumeric(string Value)
        {
            try
            {
                decimal i = Convert.ToDecimal(Value);
                if (i > 0) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是大于等于0的十进制小数
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool Is0PosNumeric(string Value)
        {
            try
            {
                decimal i = Convert.ToDecimal(Value);
                if (i >= 0) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是大于0的整形数字(32位)
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsPosInt(string Value)
        {
            try
            {
                int i = Convert.ToInt32(Value);
                if (i > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是大于0的整形数字(64位)
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsPosLong(string Value)
        {
            try
            {
                long i = Convert.ToInt64(Value);
                if (i > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是0-255的字节
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsByte(string Value)
        {
            try
            {
                byte i = Convert.ToByte(Value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否含有中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsHZ(string str)//判断是否含有中文字符
        {
            bool result = false;
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    result = true;
                }

            }
            return result;
        }

        /// <summary>
        /// 取得字符串长度（包含中文）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int StrLength(string str)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int Length = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    Length += 2;
                }
                else
                {
                    Length += 1;
                }
            }
            return Length;
        }

        /// <summary>
        /// 截取字符串（包含中文字符）
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetCutString(string inputString, int len)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
                if (tempLen > len)
                    break;

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }


            }

            return tempString;
        }

        /// <summary>
        /// 截取字符串指定长度,加'...'(包含中文字符)
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string FormatContent(string inputString, int len)
        {

            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
                if (tempLen > len)
                    break;

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

            }
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString = GetCutString(tempString, len - 2) + "…";
            return tempString;

        }
        /// <summary>
        /// 截取字符串,加'...'
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string SubMixText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            else
            {
                string strReturn = "";
                string strTemp = text;
                //半角全角
                if (Regex.Replace(strTemp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= maxLength)
                {
                    strReturn = strTemp;
                }
                else
                {
                    for (int i = strTemp.Length; i >= 0; i--)
                    {
                        strTemp = strTemp.Substring(0, i);
                        if (Regex.Replace(strTemp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= maxLength)
                        {
                            strReturn = strTemp + "…";
                            break;
                        }
                    }
                }
                return strReturn;
            }

        }


        /// <summary>
        /// 截取字符串指定长度+...，不足字符补空格(包含中文字符)
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string FormatContentEmpty(string inputString, int len)
        {

            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString = GetCutString(tempString, len - 2) + "…";
            else
            {
                string nbsp = "";
                int subi = len - mybyte.Length;
                for (int i = 0; i < subi; i++)
                    nbsp += "&nbsp;";
                tempString += nbsp;
            }
            return tempString;

        }
        /// <summary>
        /// 取中间的字符(支持中文)
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string MidString(string inputString, int start, int len)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
                if (start <= tempLen)
                {
                    try
                    {
                        tempString += inputString.Substring(i, 1);
                    }
                    catch
                    {
                        break;
                    }
                }

                if (tempLen - start >= len)
                    break;
            }

            //蛋疼的补丁...赶时间不管了
            if (StrLength(tempString) > len + 1)
                tempString = tempString.Substring(1);
            if (StrLength(tempString) == len + 1)
            {
                tempString = tempString.Substring(0, tempString.Length - 1);
                tempString += " ";
            }
            return tempString;
        }
        /// <summary>
        /// 是否是时间类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDate(string input)
        {
            try
            {
                DateTime.Parse(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是0或正整数
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool Is0PosInt(string s)
        {
            try
            {
                int i = int.Parse(s);
                if (0 <= i && i <= Int32.MaxValue) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是0或正浮点数
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool Is0PosFloat(string f)
        {
            return (Regex.IsMatch(f, @"^(0?|[1-9]\d*)(\.\d{0,1})?$"));
        }



        /// <summary>
        /// 截取包含Html代码的字符串（html代码不计数）
        /// </summary>
        /// <param name="AHtml"></param>
        /// <param name="ALength"></param>
        /// <returns></returns>
        public static string HtmlSubstring(string AHtml, int ALength)
        {
            string vReturn = "";
            int vLength = 0; // 增加的文字长度
            int vFlag = 0; // 当前扫描的区域 0:普通区 1:标记区 // 不考虑在标记中出现<button value="<">情况
            foreach (char vChar in AHtml)
            {
                switch (vFlag)
                {
                    case 0: // 普通区
                        if (vChar == '<')
                        {
                            vReturn += vChar;
                            vFlag = 1;
                        }
                        else
                        {
                            vLength++;
                            if (vLength <= ALength)
                                vReturn += vChar;
                        }
                        break;
                    case 1: // 标记区
                        if (vChar == '>') vFlag = 0;
                        vReturn += vChar;
                        break;
                }
            }
            #region 删除无效标记 // "<span><b></b></span>" -> ""
            string vTemp = Regex.Replace(vReturn, @"<[^>^\/]*?><\/[^>]*?>", "", RegexOptions.IgnoreCase); // 删除空标记
            while (vTemp != vReturn)
            {
                vReturn = vTemp;
                vTemp = Regex.Replace(vReturn, @"<[^>\/]*?><\/[^>]*?>", "", RegexOptions.IgnoreCase); // 删除空标记
            }
            #endregion
            return vReturn;
        }

        /// <summary>
        /// 移除html代码
        /// </summary>
        /// <param name="strHTML"></param>
        /// <returns></returns>
        public static string RemoveHTML(string strHTML)
        {
            if (string.IsNullOrWhiteSpace(strHTML))
            {
                return string.Empty;
            }
            Regex Regexp = new Regex("<.+?>");
            string strReturn = Regexp.Replace(strHTML, "");
            return strReturn;
        }


        /// <summary>
        /// 提取摘要，是否清除HTML代码 (完美版)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <param name="StripHTML"></param>
        /// <returns></returns>
        public static string GetContentSummary(string content, int length, bool StripHTML)
        {
            if (string.IsNullOrEmpty(content) || length == 0)
                return "";
            if (StripHTML)
            {
                Regex re = new Regex("<[^>]*>");
                content = re.Replace(content, "");
                content = content.Replace("　", "").Replace(" ", "");
                if (content.Length <= length)
                    return content;
                else
                    return content.Substring(0, length) + "……";
            }
            else
            {
                if (content.Length <= length)
                    return content;

                int pos = 0, npos = 0, size = 0;
                bool firststop = false, notr = false, noli = false;
                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    if (pos >= content.Length)
                        break;
                    string cur = content.Substring(pos, 1);
                    if (cur == "<")
                    {
                        string next = content.Substring(pos + 1, 3).ToLower();
                        if (next.IndexOf("p") == 0 && next.IndexOf("pre") != 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                        }
                        else if (next.IndexOf("/p") == 0 && next.IndexOf("/pr") != 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                                sb.Append("<br/>");
                        }
                        else if (next.IndexOf("br") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                                sb.Append("<br/>");
                        }
                        else if (next.IndexOf("img") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                                size += npos - pos + 1;
                            }
                        }
                        else if (next.IndexOf("li") == 0 || next.IndexOf("/li") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                            }
                            else
                            {
                                if (!noli && next.IndexOf("/li") == 0)
                                {
                                    sb.Append(content.Substring(pos, npos - pos));
                                    noli = true;
                                }
                            }
                        }
                        else if (next.IndexOf("tr") == 0 || next.IndexOf("/tr") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                            }
                            else
                            {
                                if (!notr && next.IndexOf("/tr") == 0)
                                {
                                    sb.Append(content.Substring(pos, npos - pos));
                                    notr = true;
                                }
                            }
                        }
                        else if (next.IndexOf("td") == 0 || next.IndexOf("/td") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                            }
                            else
                            {
                                if (!notr)
                                {
                                    sb.Append(content.Substring(pos, npos - pos));
                                }
                            }
                        }
                        else
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            sb.Append(content.Substring(pos, npos - pos));
                        }
                        if (npos <= pos)
                            npos = pos + 1;
                        pos = npos;
                    }
                    else
                    {
                        if (size < length)
                        {
                            sb.Append(cur);
                            size++;
                        }
                        else
                        {
                            if (!firststop)
                            {
                                sb.Append("……");
                                firststop = true;
                            }
                        }
                        pos++;
                    }

                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 检查是否是全球唯一标识
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ChkIsGuid(string s)
        {
            Guid gv = Guid.Empty;
            try
            {
                gv = new Guid(s);
            }
            catch
            {

            }
            return (gv != Guid.Empty) ? true : false;

        }

        /// <summary>
        /// 格式化连接字符串
        /// </summary>
        /// <param name="Sepstr">连接字符使用的分隔符,如:"_"</param>
        /// <param name="ss"></param>
        /// <returns></returns>
        public static string FormatJoinStr(string Sepstr, params object[] ss)
        {
            string re = "";
            if (ss != null)
            {
                bool flag = false;
                for (int i = 0; i < ss.Length; i++)
                {
                    re += ss[i].ToString() + Sepstr;
                    flag = true;
                }
                if (flag)
                    re = re.Substring(0, re.LastIndexOf(Sepstr));
            }
            return re;
        }

        /// <summary>
        /// 给字符串前面加0
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string AddZeroToStr(int str, int length)
        {
            string restr = str.ToString();

            for (int i = 0; i < length - str.ToString().Length; i++)
            {
                restr = "0" + restr;
            }

            return restr;
        }

        /// <summary>
        /// 返回以","串连的且过滤重复的字符串(by Linq)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterRepStr(string str)
        {
            string[] arrr = str.Split(',').Distinct().ToArray();
            return string.Join(",", arrr);
        }


        /// <summary>
        /// 将字符串转换成数字字符串(9进制滴)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertStringToNumbers(string value)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in value)
            {
                int cAscil = (int)c;
                sb.Append(Convert.ToString(cAscil, 8) + "9");
            }

            return sb.ToString();
        }
        /// <summary>
        /// 将数字字符串转换成普通字符字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertNumbersToString(string value)
        {
            string[] splitInt = value.Split(new char[] { '9' }, StringSplitOptions.RemoveEmptyEntries);

            var splitChars = splitInt.Select(s => Convert.ToChar(
                                              Convert.ToInt32(s, 8)
                                            ).ToString());

            return string.Join("", splitChars);
        }

        /// <summary>
        /// 获取指定长度的制表符
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetTabByLength(string tab, int length)
        {
            string tabs = string.Empty;
            for (int i = 0; i < length; i++)
            {
                tabs += tab;
            }
            return tabs;
        }


        /// <summary>
        /// 格式化不显示为“0”的小数（最多支持4位小数）
        /// </summary>
        /// <param name="oldValue"></param>
        /// <returns></returns>
        public static decimal AutoPrecision(decimal? oldValue)
        {
            if (!oldValue.HasValue)
            {
                return 0;
            }

            var value = oldValue.ToString();
            var precision = 0;
            var twoPart = value.Split(new char[] { '.' });
            if (twoPart.Length > 1)
            {
                var listNum = twoPart[1].Select(n => int.Parse(n.ToString())).ToList();
                if (listNum[3] > 0)
                {
                    precision = 4;
                }
                else if (listNum[2] > 0)
                {
                    precision = 3;
                }
                else if (listNum[1] > 0)
                {
                    precision = 2;
                }
                else if (listNum[0] > 0)
                {
                    precision = 1;
                }
            }
            else
            {
                precision = 0;
            }
            var formatStr = "{0:NX}".Replace("X", precision.ToString());
            return decimal.Parse(string.Format(formatStr, oldValue));
        }

        /// <summary>
        /// 遮蔽部分关键字符
        /// </summary>
        /// <param name="s"></param>
        /// <param name="startIndex">开始位置从1开始</param>
        /// <param name="maskLength">遮蔽长度</param>
        /// <param name="maskStr">替换的字符 默认为*</param>
        /// <returns></returns>
        public static string MaskKeyWordString(string s,int startIndex,int maskLength,string maskStr)
        {
            string re = s;
            if (string.IsNullOrWhiteSpace(s)) return re;
            if (string.IsNullOrWhiteSpace(maskStr)) maskStr = "*";
            var maskCh = maskStr[0];
            char[] ch = re.ToCharArray();
            var maxLength = ch.Length;
            if (startIndex >= maxLength) return re;
            var toMaskLength = maskLength + startIndex-1;
            toMaskLength = maxLength < toMaskLength ? maxLength : toMaskLength;
            var start = startIndex - 1;
            for (int i = start; i < toMaskLength; i++)
            {
                ch[i] = maskCh;
            }
            re = new string(ch);
            return re;
        }


        readonly static string[] _BlackInFormParamNames =
            {
               "&","＆","#","%","+"
            };


        /// <summary>
        /// 表单拼接提交名称检测 (如支付宝表单参数构造)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="alertName">异常描述主体名称：商品名称 </param>
        public static void CheckFormParamName(string s, string alertName)
        {
            if (_BlackInFormParamNames.Any(s.Contains))
            {
                throw new InvalidOperationException(string.Format("{0}含有非法字符！",alertName));
            }
        }


        private const string RegEmail =
            @"^[-0-9a-zA-Z~!$%^&*_=+}{\'?]+(\.[-0-9a-zA-Z~!$%^&*_=+}{\'?]+)*@([0-9a-zA-Z_][-0-9a-zA-Z_]*(\.[-0-9a-zA-Z_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[0-9a-zA-Z_][-0-9a-zA-Z_]*)|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$";
        /// <summary>
        /// 是否是合法的邮箱
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool IsMail(string f)
        {
            return (Regex.IsMatch(f, RegEmail));
        }

        /// <summary>
        /// 替换手机号中位****
        /// </summary>
        /// <param name="phonenum"></param>
        /// <returns></returns>
        public static string GetEncryptPhone(string phonenum)
        {
            if (!string.IsNullOrEmpty(phonenum))
            {
                if (phonenum.Length == 11)
                {
                    var r = new Regex(@"(\d{3})\d{4}(\d{4})");
                    return r.Replace(phonenum, "$1****$2");
                }
            }
            return phonenum;
        }
    }
}
