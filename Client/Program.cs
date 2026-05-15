using Client.Functions;
using Client.Validations;
using Common.Contracts;
using Common.Helpers;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common.Test;
using System.Threading;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new Logger();
            ValidateLine validation = new ValidateLine();

            string csvPath = "FPV_Altamonte_FL_data.csv";

            // Initialiaze WCF channel factory for ISessionService
            ChannelFactory<ISessionService> factory = new ChannelFactory<ISessionService>("SessionService");
            ISessionService proxy = null;

            Console.WriteLine("\t\t\t--- Solar Phanels Client ---");
            Console.WriteLine("Preace any key to continiue...");
            Console.ReadKey(); 

            try
            {
                List<PvSample> data;

                //Reading data from CSV file
                using (ReadFromDataBase reader = new ReadFromDataBase(csvPath))
                {
                    //Reading first 100 lines from CSV file, if there are less than 100 lines, it will read all of them
                    data = reader.FReadFromDataBase(csvPath, 220);
                } // here the Dispose method of ReadFromDataBase will be called, closing the file stream and stream reader

                // Create a channel to the WCF service
                proxy = factory.CreateChannel();

                // Information about the session
                PvMeta meta = new PvMeta("FPV_Altamonte.csv", data.Count, "1.0", 300);

                SolarDataProcessor processor = new SolarDataProcessor(proxy, logger);
                processor.TransferData(data, meta);

                // Channel and factory cleanup
                ((IClientChannel)proxy).Close();
                factory.Close();
            }
            catch (Exception e)
            {
                logger.Error("Error: " + e.Message);

                // Dispose of the channel and factory in case of an exception to free up resources
                if (proxy != null)
                {
                    Console.WriteLine("Error has acured, Forcefuly freing up resources (Abort)");
                    ((IClientChannel)proxy).Abort();
                }
                factory.Abort();
            }

            Console.WriteLine("\nOperation has ended. Preace ENTER to finish");
            Console.ReadLine();
        }
    }
}