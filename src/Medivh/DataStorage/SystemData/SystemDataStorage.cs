﻿using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Medivh.Common;
using Microsoft.VisualBasic.Devices;

namespace Medivh.DataStorage.SystemData
{
    internal class SystemDataStorage
    {

        static Process currentProcess = Process.GetCurrentProcess();
        static ComputerInfo ci = new ComputerInfo();

        private static double cpuUsed = 0;

        static SystemDataStorage()
        { 
            Task.Run(() =>
            {
                //间隔时间（毫秒）
                int interval = 1000;
                //上次记录的CPU时间
                var prevCpuTime = TimeSpan.Zero;
                while (true)
                {
                    //当前时间
                    var curTime = currentProcess.TotalProcessorTime;
                    //间隔时间内的CPU运行时间除以逻辑CPU数量
                    var value = (curTime - prevCpuTime).TotalMilliseconds / interval / Environment.ProcessorCount * 100;
                    prevCpuTime = curTime;
                    //输出 
                    cpuUsed = value; 
                    Thread.Sleep(interval);
                }
            });
        }
        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <returns></returns>
        public static object GetSystemInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("{");
            stringBuilder.AppendFormat("\"OSarch\":\"{0}\",", OSarch());
            stringBuilder.AppendFormat("\"OSname\":\"{0}\",", OSname());
            stringBuilder.AppendFormat("\"OSversion\":\"{0}\",", OSversion());
            stringBuilder.AppendFormat("\"TotalPhysicalMemory\":{0},", TotalPhysicalMemory());
            stringBuilder.AppendFormat("\"TotalVirtualMemory\":{0},", TotalVirtualMemory());
            stringBuilder.AppendFormat("\"AvailablePhysicalMemory\":{0},", AvailablePhysicalMemory());
            stringBuilder.AppendFormat("\"AvailableVirtualMemory\":{0},", AvailableVirtualMemory());
            stringBuilder.AppendFormat("\"PrivatePhysicalMemory\":{0},", PrivatePhysicalMemory());
            stringBuilder.AppendFormat("\"CpuNum\":{0},", GetCpuNumber());
            //stringBuilder.AppendFormat("\"Cpu\":{0},", GetCpuUse());
            stringBuilder.AppendFormat("\"AppCpu\":{0},", GetAppCpuUse());
            stringBuilder.AppendFormat("\"StartTime\":\"{0}\"", StartTime());

            stringBuilder.Append("}");
            var json = (stringBuilder.ToString());
            return JsonHelper.JosnStringToObject<object>(json);
        }

        private static PerformanceCounter _oPerformanceCounter = null;
        public static double GetCpuUse()
        {
            if (_oPerformanceCounter == null)
            {
                try
                {
                    _oPerformanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                }
                catch
                {
                }
            }
            if (_oPerformanceCounter != null)
            {
                return _oPerformanceCounter.NextValue();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取cpu使用率
        /// </summary>
        /// <returns></returns>
        public static double GetAppCpuUse()
        { 
            return cpuUsed;
        }




        /// <summary>
        /// 获取cpu数量
        /// </summary>
        /// <returns></returns>
        public static int GetCpuNumber()
        {
            return Environment.ProcessorCount;
        }

        /// <summary>
        /// 总物理内存
        /// </summary>
        /// <returns></returns>
        public static ulong TotalPhysicalMemory()
        {
            return ci.TotalPhysicalMemory / 1024 / 1024;
        }

        /// <summary>
        /// 总虚拟内存
        /// </summary>
        /// <returns></returns>
        public static ulong TotalVirtualMemory()
        {
            return ci.TotalVirtualMemory / 1024 / 1024;
        }

        /// <summary>
        /// 可用物理内存
        /// </summary>
        /// <returns></returns>
        public static ulong AvailablePhysicalMemory()
        {
            return ci.AvailablePhysicalMemory / 1024 / 1024;
        }

        /// <summary>
        /// 可用虚拟内存
        /// </summary>
        /// <returns></returns>
        public static ulong AvailableVirtualMemory()
        {
            return ci.AvailableVirtualMemory / 1024 / 1024;
        }

        /// <summary>
        /// 占用物理内存
        /// </summary>
        /// <returns></returns>
        public static long PrivatePhysicalMemory()
        {
            return currentProcess.PrivateMemorySize64 / 1024 / 1024;
        }

        /// <summary>
        /// 启动时间
        /// </summary>
        /// <returns></returns>
        public static string StartTime()
        {
            return currentProcess.StartTime.ToString("yyyy-MM-dd HH:ss:mm");
        }

        public static string OSarch()
        {
            return Environment.Is64BitOperatingSystem ? "amd64" : "x86";
        }

        public static string OSname()
        {
            return Environment.MachineName;
        }

        public static string OSversion()
        {
            return Environment.OSVersion.ToString();
        }
    }

}
