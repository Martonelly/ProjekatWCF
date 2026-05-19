using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(SessionService));
            host.Open();
            Console.WriteLine("Service is open press any button!");
            Console.ReadKey();
            host.Close();
            Console.WriteLine("Service is closing, press any button");
        }
    }
}
