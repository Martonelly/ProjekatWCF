using Client.Validations;
using Common.Contracts;
using Common.Helpers;
using Common.Interfaces;
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
            ILogger logger = new Logger();
            ChannelFactory<ISessionService> factory = new ChannelFactory<ISessionService>("SessionService");
            //TODO read from CSV file (externally) whole client workflow here
            ISessionService proxy = factory.CreateChannel();
            Console.WriteLine("Client starting!");
            Console.ReadKey();
            PvMeta test = new PvMeta("FileName", 100, "Schema", 200);
            ValidateLine validation = new ValidateLine();
            //TODO start session befor try catch
            try
            {
                //The meta values are set up here
                //Test for read sample --> the same as one line in the CSV file
                PvSample sample1 = new PvSample("0,2023335,00:05:00,0.1,3277.0,424.0,37.0,482.0,478.5,483.0,0.0,0.0,0.0,279.3,276.7,277.2,60.0,60.0,60.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                //For test and to see how the validation and the logger works
                validation.checkValidity(sample1);
                if (validation.IsValid)
                {
                    proxy.StartSession(test);
                    logger.Info(validation.Messsage);
                }
                else {
                    //TODO write the lines in the rejected_client.CSV
                    logger.Error(validation.Messsage);
                }

                //TODO Reading the CSV file --> foreach loop sending one by one line to Service (PushSample function), the validation works the same as above
                //
            }
            catch (Exception e){
                logger.Error(e.ToString());
            }
            //TODO EndSession() --> after the foreach loop
            Console.ReadLine();
        }
    }
}
