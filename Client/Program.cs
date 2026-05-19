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
using System.Threading;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Configuration;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string car = "";
            while (!car.Equals("x"))
            {
                Console.WriteLine("Menu please enter your choice, you can input x to ecsape!");
                Console.WriteLine("1. Start normal function!");
                Console.WriteLine("2. Test exception!");
                car = Console.ReadLine();
                switch (car)
                {
                    case "1":
                        start();
                        break;
                    case "2":
                        simulation();
                        break;
                }
            }
            return;
            
        }
            private static void start() {
            ILogger logger = new Logger();
            ValidateLine validation = new ValidateLine();
            string csvPath = ConfigurationManager.AppSettings["FileName"];
            int totalRows = int.Parse(ConfigurationManager.AppSettings["TotalRows"]);
            string vesion = ConfigurationManager.AppSettings["SchemaVersion"];
            int readRows = int.Parse(ConfigurationManager.AppSettings["RowLimitN"]);
            int rows = 0;
            ReadFromDataBase reader = new ReadFromDataBase(csvPath);
            // Initialiaze WCF channel factory for ISessionService
            ChannelFactory<ISessionService> factory = new ChannelFactory<ISessionService>("SessionService");
            ISessionService proxy = factory.CreateChannel(); ;
            Console.WriteLine("\t\t\t--- Solar Phanels Client ---");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            try
            {
                PvMeta meta = new PvMeta(csvPath, totalRows, vesion, readRows);
                proxy.StartSession(meta);
                while (rows++ <= readRows)
                {
                    string line = reader.FReadFromDataBase();
                    if (line == null) break;

                    PvSample sample = new PvSample(line);

                    validation.checkValidity(sample);
                    // Simulate some delay for the first 200 lines to test the client handling of delayed responses
                    if (sample.RowIndex > 200) {
                        Thread.Sleep(500);
                    }

                    if (validation.IsValid)
                    {
                        proxy.PushSample(sample);
                        logger.Info($"Sucesfully sent line: {sample.RowIndex}");
                    }
                    else
                    {
                        // Log the error and save the rejected sample in a separate file with the error message for later analysis
                        string errorLog = $"Line: {sample.RowIndex} Rejected: {validation.Message}";
                        File.AppendAllText("rejected_client.CSV", $"{errorLog} | Raw: {sample.RowIndex},{sample.Day},{sample.Hour}..." + Environment.NewLine);
                        logger.Error(errorLog);
                    }

                }
                reader.Dispose();
                proxy.EndSession();

                ((IClientChannel)proxy).Close();
                factory.Close();
            }
            catch (Exception e)
            {
                logger.Error("Error: " + e.Message);

                // Dispose of the channel and factory in case of an exception to free up resources
                if (proxy != null)
                {
                    Console.WriteLine("Error has accured, Forcefully firing up resources (Abort)");
                    ((IClientChannel)proxy).Abort();
                }
                reader.Dispose();
                factory.Abort();
            }

            Console.WriteLine("\nOperation has ended, press any button to close the window");
            Console.ReadLine();
        }
        private static void simulation() {
            ILogger logger = new Logger();
            ValidateLine validation = new ValidateLine();

            string csvPath = ConfigurationManager.AppSettings["FileName"];
            int totalRows = int.Parse(ConfigurationManager.AppSettings["TotalRows"]);
            string vesion = ConfigurationManager.AppSettings["SchemaVersion"];
            int readRows = int.Parse(ConfigurationManager.AppSettings["RowLimitN"]);
            int rows = 0;
            ReadFromDataBase reader = new ReadFromDataBase(csvPath);
            // Initialiaze WCF channel factory for ISessionService
            ChannelFactory<ISessionService> factory = new ChannelFactory<ISessionService>("SessionService");
            ISessionService proxy = factory.CreateChannel(); ;
            Console.WriteLine("\t\t\t--- Solar Phanels Client ---");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            PvMeta meta = new PvMeta(csvPath, totalRows, vesion, readRows);
            proxy.StartSession(meta);
            try
            {
                while (rows++ <= readRows)
                {
                    string line = reader.FReadFromDataBase();
                    if (line == null) break;

                    PvSample sample = new PvSample(line);

                    validation.checkValidity(sample);
                    if (sample.RowIndex > 200){
                        Thread.Sleep(100); 
                    }

                    if (validation.IsValid)
                    {
                        proxy.PushSample(sample);
                        logger.Info($"Sucesfully sent line: {sample.RowIndex}");
                    }
                    else
                    {
                        // Log the error and save the rejected sample in a separate file with the error message for later analysis
                        string errorLog = $"Line: {sample.RowIndex} Rejected: {validation.Message}";
                        File.AppendAllText("rejected_client.CSV", $"{errorLog} | Raw: {sample.RowIndex},{sample.Day},{sample.Hour}..." + Environment.NewLine);
                        logger.Error(errorLog);
                    }

                    if (rows == 15) {
                        throw new Exception("Test exception");
                    }

                }
                reader.Dispose();
                proxy.EndSession();

                ((IClientChannel)proxy).Close();
                factory.Close();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);

                // Dispose of the channel and factory in case of an exception to free up resources
                if (proxy != null)
                {
                    logger.Info("Forcefully firing up resources (Abort)");
                    ((IClientChannel)proxy).Abort();
                }
                reader.Dispose();
                factory.Abort();
            }
        }

    }
}