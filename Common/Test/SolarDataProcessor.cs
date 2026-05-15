using Client.Validations;
using Common.Contracts;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace Common.Test
{
    public class SolarDataProcessor
    {
        private readonly ISessionService _proxy;
        private readonly ILogger _logger;

        public SolarDataProcessor(ISessionService proxy, ILogger logger)
        {
            _proxy = proxy;
            _logger = logger;
        }

        public void TransferData(List<PvSample> data, PvMeta meta)
        {
            ValidateLine validation = new ValidateLine();

            try
            {
                // Start Session
                _proxy.StartSession(meta);
                _logger.Info("Session is succesfuly started.");

                // Sending data line by line, if the line is not valid, it will be logged and saved in a separate file for rejected samples
                foreach (var sample in data)
                {
                    validation.checkValidity(sample);

                    if(sample.RowIndex > 200) // Simulate some delay for the first 200 lines to test the client handling of delayed responses
                    {
                        Thread.Sleep(500); // Sleep for 0.5 seconds
                    }

                    if (validation.IsValid)
                    {
                        _proxy.PushSample(sample);
                        Console.WriteLine($"Sucesfully sent line: {sample.RowIndex}");
                    }
                    else
                    {
                        // Log the error and save the rejected sample in a separate file with the error message for later analysis
                        string errorLog = $"Line: {sample.RowIndex} Rejected: {validation.Messsage}";
                        File.AppendAllText("rejected_client.CSV", $"{errorLog} | Raw: {sample.RowIndex},{sample.Day},{sample.Hour}..." + Environment.NewLine);
                        _logger.Error(errorLog);
                    }
                }

                // End Session
                _proxy.EndSession();
                _logger.Info("Data transfer is all done. Sessino has ended");
            }
            catch (Exception e)
            {
                // This ensures the error is logged and the WCF channel is killed properly
                _logger.Error("Error: " + e.Message);

                if (_proxy is IClientChannel channel)
                {
                    Console.WriteLine("Error has acured, Forcefuly freing up resources (Abort)");
                    channel.Abort();
                }

                throw; // Rethrow so the Unit Test knows the failure happened
            }
        }
    }
}