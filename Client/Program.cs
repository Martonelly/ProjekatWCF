using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<ISessionService> factory = new ChannelFactory<ISessionService>("SessionService");
            //TODO read from CSV file (externally) whole client workflow here
               ISessionService proxy = factory.CreateChannel();
            Console.WriteLine("Client starting!");
            Console.ReadKey();
            try
            {
                PvMeta test = new PvMeta("FileName", 100, "Schema", 200);
                proxy.StartSession(test);
            }
            catch {
            
            }
        }
    }
}
