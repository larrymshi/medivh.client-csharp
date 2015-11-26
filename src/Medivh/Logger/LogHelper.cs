﻿using System;
using Microsoft.VisualBasic;

namespace Medivh.Logger
{
    internal class LogHelper
    {
        public static Action<string> Logger { get; set; }
        private static uint LogLevel { get; set; }
        public static void Init(Action<string> action, uint logLevel)
        {
            Logger = action;
            LogLevel = logLevel;
        }


        public static void Info(string msg)
        {
            msg = "[INFO] " + msg;
            if (Logger != null)
            {
                Logger(msg);
            }
            else
            {
                Console.WriteLine(msg);
            }
        }

        public static void Error(string msg)
        {
            msg = "[Error] " + msg;
            if (Logger != null)
            {
                Logger(msg);
            }
            else
            {
                Console.WriteLine(msg);
            }
        }

        public static void Debug(string msg)
        {
            if (LogLevel == 0)
            {
                return;
            }
            msg = "[Debug] " + msg;
            if (Logger != null)
            {
                Logger(msg);
            }
            else
            {
                Console.WriteLine(msg);
            }

        }

        public static void Error(Exception ex)
        {
            if (ex != null)
            {
                var msg = ex.Message + "  -  " + ex.StackTrace;
                Error(msg);
            }
        }
    }
}