using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Reflection;
using System.Linq;

namespace DDD.Utility
{
    public static class EnumHelper
    {
        public static void EnumTypeBind2Dropdownlist<T>(DropDownList myDpl)
        {
            string[] names = Enum.GetNames(typeof(T));
            int[] values = (int[])Enum.GetValues(typeof(T));
            myDpl.Items.Clear();
            for (int i = 0; i < names.Length; i++)
            {
                myDpl.Items.Add(new ListItem(names[i], values[i].ToString()));
            }
        }

        public static void EnumTypeBind2RadioButtonList<T>(RadioButtonList myRdo)
        {
            string[] names = Enum.GetNames(typeof(T));
            int[] values = (int[])Enum.GetValues(typeof(T));
            myRdo.Items.Clear();
            for (int i = 0; i < names.Length; i++)
            {
                myRdo.Items.Add(new ListItem(names[i], values[i].ToString()));
            }
        }

        public static string getEnumName<T>(int i)
        {
            string[] names = Enum.GetNames(typeof(T));
            int[] values = (int[])Enum.GetValues(typeof(T));
            for (int j = 0; j < names.Length; j++)
            {
                if (values[j] == i) return names[j];
            }
            return "未知";
        }

        public static T Change2EnumType<T, K>(K n)
        {
            if (Enum.IsDefined(typeof(T), n))
            {
                T value = (T)Convert.ChangeType(n, Enum.GetUnderlyingType(typeof(T)));
                return value;
            }
            else
                throw new Exception(n + " is not defined");
        }

        public static bool CheckEnumValue<T, K>(K n)
        {
            if (Enum.IsDefined(typeof(T), n))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 返回枚举中的对应的资源文件的值
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetLocalizedDescription(this object @enum)
        {
            if (@enum == null)
                return null;

            string description = @enum.ToString();

            FieldInfo fieldInfo = @enum.GetType().GetField(description);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Any())
                return attributes[0].Description;

            return description;
        }
        /// <summary>
        /// 返回枚举的值
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static byte GetValue(this object @enum)
        {
            if (@enum == null)
                return 0;
            return (byte)@enum;
        }

        /// <summary>
        /// 返回枚举的值
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static int GetValueToInt(this object @enum)
        {
            if (@enum == null)
                return 0;
            return (int)@enum;
        }
        /// <summary>
        /// 返回枚举的值 格式化成字符串
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetValueToString(this object @enum)
        {
            if (@enum == null)
                return "0";
            return ((byte)@enum).ToString();
        }
        /// <summary>
        /// 返回枚举的键名 格式化成字符串
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetToString(this object @enum)
        {
            if (@enum == null)
                return "";
            return @enum.ToString();
        }

        /// <summary>
        /// 返回是否
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        public static string GetBoolenDescription(this bool @bl)
        {
            if (@bl)
                return "是";
            else
                return "否";
        }
        /// <summary>
        /// 返回性别
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        public static string GetSexDescription(this bool @bl)
        {
            if (@bl)
                return "男";
            else
                return "女";
        }
    }

    public class LocalizedEnumAttribute : DescriptionAttribute
    {
        private PropertyInfo _nameProperty;
        private Type _resourceType;

        public LocalizedEnumAttribute(string displayNameKey)
            : base(displayNameKey)
        {

        }

        public Type NameResourceType
        {
            get
            {
                return _resourceType;
            }
            set
            {
                _resourceType = value;

                _nameProperty = _resourceType.GetProperty(this.Description, BindingFlags.Static | BindingFlags.Public);
            }
        }

        public override string Description
        {
            get
            {
                //check if nameProperty is null and return original display name value 
                if (_nameProperty == null)
                {
                    return base.Description;
                }

                return (string)_nameProperty.GetValue(_nameProperty.DeclaringType, null);
            }
        }
    }

}
