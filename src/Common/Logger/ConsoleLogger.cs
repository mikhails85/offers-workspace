using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logger
{
    public class ConsoleLogger : ILogger
    {
        public void LogError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;            
            Console.WriteLine($"ERROR ({DateTime.Now.ToString("dd/MM HH:mm:ss.fff")}):{error}"); 
            Console.ResetColor();
        }

        public void LogError(Exception error)
        {
            this.LogError(error); 
        }

        public void LogInfo(string info)
        {
            Console.ForegroundColor = ConsoleColor.Blue;            
            Console.WriteLine($"INFO ({DateTime.Now.ToString("dd/MM HH:mm:ss.fff")}):{info}");
            Console.ResetColor();
        }
    }
}