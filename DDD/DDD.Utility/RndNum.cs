using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDD.Utility
{
    /// <summary>
    /// 随机数
    /// </summary>
    public class RndNum
    {
        /// <summary>
        /// 生成随机数(A-Z)
        /// </summary>
        /// <param name="myLen">生成多少位</param>
        /// <returns></returns>
        public static string GenRndLetter(int myLen)
        {
            Random randomGenerator = new Random(DateTime.Now.Millisecond);

            string RandData = string.Empty;
            for (int i = 0; i < myLen; i++)
                RandData += Convert.ToChar(randomGenerator.Next(97, 122));

            return RandData.ToUpper();
        }

        /// <summary>
        /// 生成1-9随机数，可能重复数字
        /// </summary>
        /// <param name="myLen"></param>
        /// <returns></returns>
        public static string GenRndNum(int myLen)
        {
            string RandData = string.Empty;
            //var randomGenerator = new Random(i * unchecked((int)DateTime.Now.Ticks));
            var randomGenerator = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 1; i < myLen+1; i++)
            {
                RandData += randomGenerator.Next(1, 9);
            }
            return RandData;
        }

        public static string GenRndNum(int myLen,Random rd)
        {
            string RandData = string.Empty;
            for (int i = 1; i < myLen + 1; i++)
            {
                RandData += rd.Next(1, 9);
            }
            return RandData;
        }

        /// <summary>
        ///生成随机数函数中从Vchar数组中随机抽取
        ///字母区分大小写,前一个与后一个无重复字符
        /// </summary>
        /// <param name="VcodeNum">生成多少位</param>
        /// <param name="IsNum">是否纯数字</param>
        /// <returns></returns>
        public static string GenRndNum(int VcodeNum, bool IsNum)
        {
            string Vchar = "";
            if (IsNum) Vchar = "0,1,2,3,4,5,6,7,8,9";
            else Vchar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,N,P,Q,R,S,T,U,X,Y,Z";

            string[] VcArray = Vchar.Split(',');
            string VNum = "";//由于字符串很短，就不用StringBuilder了
            int temp = -1;//记录上次随机数值，尽量避免生产几个一样的随机数

            //采用一个简单的算法以保证生成随机数的不同
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    //rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                    rand = new Random(Guid.NewGuid().GetHashCode());
                }
                int t = rand.Next(VcArray.Length);
                if (temp != -1 && temp == t)
                {
                    return GenRndNum(VcodeNum, IsNum);
                }
                temp = t;
                VNum += VcArray[t];

            }
            return VNum;
        }


        /// <summary>
        /// 返回随机数组
        /// </summary>
        /// <param name="len"></param>
        /// <param name="tmp"> GenRndNum(len)</param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static void GenerateListRndNum(int len, string tmp, ref List<string> list, Random rd)
        {
            if (list.Any(x => x == tmp))
            {
                tmp = GenRndNum(len, rd);
                GenerateListRndNum(len, tmp, ref list,rd);
            }
            else
            {
                list.Add(tmp);
            }
        }

    }
}
