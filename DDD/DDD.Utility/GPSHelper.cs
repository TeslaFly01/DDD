using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDD.Utility
{
    public static class GPSHelper
    {


        /// <summary>
        ///  计算坐标距离
        /// </summary>
        /// <param name="lngA">坐标A经度</param>
        /// <param name="latA">坐标A纬度</param>
        /// <param name="lngB">坐标B经度</param>
        /// <param name="latB">坐标B纬度</param>
        /// <returns></returns>
        public static double getDistance(double lngA, double latA, double lngB, double latB)
        {
            double DistanceLng = 102834.74258026089786013677476285;
            double DistanceLat = 111712.69150641055729984301412873;
            double LngAbs = Math.Abs((lngA - lngB) * DistanceLng);
            double LatAbs = Math.Abs((latA - latB) * DistanceLat);
            return Math.Sqrt((LatAbs * LatAbs + LngAbs * LngAbs));
        }

        public static string getFormatDistance(double lngA, double latA, double lngB, double latB)
        {
            var disText = string.Empty;
            try
            {
                var distance = Convert.ToInt32(getDistance(lngA, latA, lngB, latB));
                disText = distance > 1000 ? Math.Round((float)distance / 1000, 1) + "公里" : distance + "米";
            }
            catch (Exception)
            {
            }
            
            return disText;
        }


    }
}
