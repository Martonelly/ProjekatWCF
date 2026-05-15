using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Client.Validations;
using Common.Analitics;
using Common.Contracts;
using Common.Enums;
using Common.Events.Publisher;
using Common.Events.Subscriber;
using Common.Helpers;
using Common.Interfaces;

namespace Service
{
    public class SessionService : ISessionService, IDisposable
    {
        private StreamWriter _streamWriter;
        private StreamWriter _rejectStreamWriter;
        private bool _isDisposed = false;
        private ILogger logger = new Logger();
        private int rowCounter;
        private int rowLimit;
        private Flatline flatline = new Flatline();
        private Spike spike = new Spike();
        public void EndSession()
        {
            Listner TransferComplete = new Listner();

            OnTransferGenerator generator = new OnTransferGenerator();

            generator.TransferCompletedEvent += TransferComplete.LogInfo;

            Console.WriteLine("Session has ended by request");
            generator.ProcessTransfer(TransferType.Complete);

            Console.WriteLine($"Recived messages are {rowCounter} and the total processed row Limit is {rowLimit}!");
            Console.WriteLine($"Recived data is {Math.Round(((double)rowCounter/rowLimit)*100),2}% of the actual size!");
            this.Dispose();
            Console.WriteLine("All Data is saved on disc.");
            Console.WriteLine("Preace any key to quit...");
            Console.ReadKey();
        }

        public void PushSample(PvSample sample)
        {
            rowCounter++;
            //Listeners that subscribe to the event
            Listner sampleListner = new Listner();
            //Transfer generator that has the two events (Publisher) --> we manually activate these events bellow
            OnTransferGenerator transferGenerator = new OnTransferGenerator();

            //Subscription 
            transferGenerator.SampleRecievedEvent += sampleListner.LogInfo;

            //Raise event
            transferGenerator.ProcessTransfer(TransferType.Recieved);

            ValidateLine isValid = new ValidateLine();
            isValid.checkValidity(sample);

            //ANALITICS PART
            flatline.FlatlineCheck(sample);
            spike.SpikeCheck(sample);
            //TODO ADD temp and Voltage Check (same style of warnings diffenert messages)
            Thread.Sleep(500); // Simulate some processing time for each sample to test the client handling of delayed responses
            if (isValid.IsValid)
            {
                _streamWriter?.WriteLine($"{sample.RowIndex},{sample.Day},{sample.Hour},{sample.AcPwrt},{sample.DcVolt},{sample.Temper},{sample.Vl1to2},{sample.Vl2to3},{sample.Vl3to1},{sample.AcCur1},{sample.AcVlt1}");
                _streamWriter?.Flush();
            }
            else
            {
                logger.Error(isValid.Messsage);
                //_rejectStreamWriter.WriteLine("RowIndex,Day,Hour,AcPwrt,DcVolt,Temper,Vl1to2,Vl2to3,Vl3to1,AcCur1,AcVlt1,ErrMsg");
                _rejectStreamWriter?.WriteLine($"{sample.RowIndex},{sample.Day},{sample.Hour},{sample.AcPwrt},{sample.DcVolt},{sample.Temper},{sample.Vl1to2},{sample.Vl2to3},{sample.Vl3to1},{sample.AcCur1},{sample.AcVlt1}");
                // TODO: Add Error Message to the rejected samples header
            }
        }

        public void StartSession(PvMeta meta)
        {
            Console.WriteLine("\t\t\t --- Starting Session ---");
            
            Listner transferListner = new Listner();

            OnTransferGenerator generator = new OnTransferGenerator();

            generator.TransferStartedEvent += transferListner.LogInfo;

            generator.ProcessTransfer(TransferType.Start);
            rowCounter = 0;

            // Creating a directory for the session, using the file name as PlantId and the current date, and creating a file for the session data
            string PlantId = meta.FileName.Replace(".csv", "");
            string directoryPath = $"Data/{PlantId}/{DateTime.Now:yyyy-MM-dd}/Session.csv";

            if (!Directory.Exists(Path.GetDirectoryName(directoryPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(directoryPath));

            _streamWriter = new StreamWriter(directoryPath);
            _rejectStreamWriter = new StreamWriter("Rejected_Samples.csv");

            // If the file is new, write the header line
            if (new FileInfo(directoryPath).Length == 0)
            {
                _streamWriter.WriteLine("RowIndex,Day,Hour,AcPwrt,DcVolt,Temper,Vl1to2,Vl2to3,Vl3to1,AcCur1,AcVlt1");
            }
            if(new FileInfo("Rejected_Samples.csv").Length == 0)
            {
                _rejectStreamWriter.WriteLine("RowIndex,Day,Hour,AcPwrt,DcVolt,Temper,Vl1to2,Vl2to3,Vl3to1,AcCur1,AcVlt1,ErrMsg");
            }

            rowLimit = meta.RowLimitN;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _streamWriter.Close();
                    _rejectStreamWriter.Close();
                    _streamWriter.Dispose();
                    _rejectStreamWriter.Dispose();
                    Console.WriteLine("Clients resources has being releaced");
                }
                _isDisposed = true;
            }
        }

    }
}
