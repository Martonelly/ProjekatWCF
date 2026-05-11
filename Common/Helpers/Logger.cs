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
            Console.WriteLine($"[{LoggerTypes.ERROR}],  {time},  {message}!");
        }

        public void Info(string message)
        {
            DateTime time = new DateTime();
            time = DateTime.Now;
            Console.WriteLine($"[{LoggerTypes.INFO}],  {time},  {message}!");
        }

        public void Warning(string message)
        {
            DateTime time = new DateTime();
            time = DateTime.Now;
            Console.WriteLine($"[{LoggerTypes.WARNING}],  {time},  {message}!");
        }
    }
}
