using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Validations;
using Common.Contracts;
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
        private int rowLimit;
        
        public void EndSession()
        {
            Console.WriteLine("Session has ended by request");
            Console.WriteLine($"Recived messages are {Counter.RowCount} and the total processed row Limit is {rowLimit}!");
            Console.WriteLine($"Recived data is {(((double)Counter.RowCount/rowLimit)*100)}% of the actual size!");
            this.Dispose();
            Console.WriteLine("All Data is saved on disc.");
            Console.WriteLine("Preace any key to quit...");
            Console.ReadKey();
        }

        public void PushSample(PvSample sample)
        {
            Counter.RowCount++;
        //TODO : add validation here
        bool valid = true; //TODO change this to the actual validation result
            ValidateLine isValid = new ValidateLine();
            isValid.checkValidity(sample);
            if (isValid.IsValid)
            {
                Console.WriteLine("Data is transfring...");
               // logger.Info(isValid.Messsage);
                _streamWriter?.WriteLine($"{sample.RowIndex},{sample.Day},{sample.Hour},{sample.AcPwrt},{sample.DcVolt},{sample.Temper},{sample.Vl1to2},{sample.Vl2to3},{sample.Vl3to1},{sample.AcCur1},{sample.AcVlt1}");
                _streamWriter?.Flush();
                Console.WriteLine("Data transfer finished...");
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
            Counter.RowCount = 0;
            
            Console.WriteLine("\t\t\t --- Starting Session ---");

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
