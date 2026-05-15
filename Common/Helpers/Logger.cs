using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class Logger : ILogger
    {
        public void Error(string message)
        {
            DateTime time = new DateTime();
            time = DateTime.Now;
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine($"[{LoggerTypes.ERROR}],\t\t{time},\t\t{message}!");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine();
        }

        public void Info(string message)
        {
            DateTime time = new DateTime();
            time = DateTime.Now;
            Console.WriteLine($"[{LoggerTypes.INFO}],\t\t{time},\t\t{message}!");
            Console.WriteLine();
        }

        public void Warning(string message)
        {
            DateTime time = new DateTime();
            time = DateTime.Now;
            Console.WriteLine($"[{LoggerTypes.WARNING}],\t{time},\t\t{message}!");
            Console.WriteLine();
        }
    }
}
