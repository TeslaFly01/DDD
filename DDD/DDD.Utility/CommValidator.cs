using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DDD.Utility
{
    public static class CommValidator
    {
        
        /// <summary>
        /// 登陆名称是否只包含数字、字母和下划线，且长度为6－20位。
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static bool IsValidName(string sName)
        {
            return (Regex.IsMatch(sName, @"^(\w{6,20})$"));
        }

        /// <summary>
        /// 登陆密码是否是6－20位常见的字符、字母或数字。
        /// </summary>
        /// <param name="sPwd"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string sPwd)
        {
            return (Regex.IsMatch(sPwd, @"^[\w!@#\$%\^&\*\(\)]{6,20}$"));
        }

        public static bool IsValidEmail(string sEmail)
        {
            return (Regex.IsMatch(sEmail, @"^[-0-9a-zA-Z~!$%^&*_=+}{\'?]+(\.[-0-9a-zA-Z~!$%^&*_=+}{\'?]+)*@([0-9a-zA-Z_][-0-9a-zA-Z_]*(\.[-0-9a-zA-Z_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[0-9a-zA-Z_][-0-9a-zA-Z_]*)|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$"));
        }
    }
}
