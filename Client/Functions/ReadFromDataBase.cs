using Client.Validations;
using Common.Contracts;
using Common.Helpers;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Functions
{
    public class ReadFromDataBase : IReadFromDataBase, IDisposable
    {
        private FileStream fs;
        private StreamReader sr;
        private bool disposedValue = false;
        private ILogger logger = new Logger();

        public ReadFromDataBase(string path)
        {
            fs = File.OpenRead(path);
            sr = new StreamReader(fs);
        }

        public string FReadFromDataBase()
        {
            string line;
            line = sr.ReadLine();
            return line;
        }


        // Implementing the Dispose pattern to release resources
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    sr.Dispose();
                    fs.Dispose();
                    logger.Info("Clients resources have been released");
                }
                disposedValue = true;
            }
        }

        ~ReadFromDataBase() {
            Dispose(false);
        }
    }
}
