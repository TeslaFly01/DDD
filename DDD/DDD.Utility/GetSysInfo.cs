using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace DDD.Utility
{
    public class GetSysInfo
    {
        public GetSysInfo()
        {
        }
        public static string GetCpuInfo()//得到cpu信息 
        {
            string _cpuInfo = "";
            try
            {
                ManagementClass cimobject = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = cimobject.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    _cpuInfo = mo.Properties["ProcessorId"].Value.ToString();

                }
            }
            catch { }
            return _cpuInfo;
        }

        public static string GetHDInfo()//获取第一个硬盘ID 
        {
            string _HDInfo = "";
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject mo in searcher.Get())
                {
                    _HDInfo = mo["Model"].ToString().Trim();
                    break;
                }

            }
            catch { }
            return _HDInfo;
        }

        public static string GetMacInfo()//获取网卡硬件地址 
        {
            string _MacAddress = "";
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc2 = mc.GetInstances();
                foreach (ManagementObject mo in moc2)
                {
                    if ((bool)mo["IPEnabled"] == true)
                        _MacAddress = mo["MacAddress"].ToString();
                    mo.Dispose();
                }
            }
            catch { }
            return _MacAddress;
        }
    }
}
