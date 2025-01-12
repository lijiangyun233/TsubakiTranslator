﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TsubakiTranslator.BasicLibrary
{
    public class ProcessHelper
    {

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        /// <summary>
        /// 获得当前系统进程列表 形式：直接用于显示的字串和进程PID
        /// </summary>
        /// <returns></returns>
        public static LinkedList<string> GetProcessList()
        {
            LinkedList<string> list = new LinkedList<string>();

            //获取系统进程列表
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    string info = p.Id + " — " + p.ProcessName;
                    list.AddLast(info);
                }
            }
            return list;
        }


        /// <summary>
        /// 根据路径返回进程
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static Process GetProcessByPath(string gamePath)
        {
            Process[] ps = Process.GetProcesses();
            Process process = ps[0];

            foreach(Process p in ps)
            {
                try
                {
                    if (p.MainModule.FileName.Equals(gamePath))
                    {
                        process = p;
                        break;
                    }
                }
                catch(Exception )
                {
                    continue;
                }
            }

            return process;
        }

        public static bool IsWinX86(Process process)
        {

            IntPtr processHandle;
            bool retVal;

            try
            {
                processHandle = Process.GetProcessById(process.Id).Handle;
            }
            catch
            {
                return false;
            }
            return IsWow64Process(processHandle, out retVal) && retVal;

        }



    }
}
