using Client.Validations;
using Common.Contracts;
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

        public ReadFromDataBase(string path)
        {
            fs = File.OpenRead(path);
            sr = new StreamReader(fs);
        }

        public List<PvSample> FReadFromDataBase(string path, int limitN)
        {
            ReadFromDataBase reader = new ReadFromDataBase(path);

            string headerLine = sr.ReadLine(); // Skip the header line
            string line;
            List<PvSample> samples = new List<PvSample>();
            int rowIndex = 0;

            while ((line = sr.ReadLine()) != null && rowIndex < limitN)
            {
                try
                {
                    // Split the line into parts and parse the values
                    string[] parts = line.Split(',');
                    // Parse the necessary fields to create a PvSample object
                    int Day = int.Parse(parts[1]);
                    string Hour = parts[2];
                    double AcPwrt = double.Parse(parts[3]);
                    double DcVolt = double.Parse(parts[4]);
                    double Temper = double.Parse(parts[6]);
                    double Vl1to2 = double.Parse(parts[7]);
                    double Vl2to3 = double.Parse(parts[8]);
                    double Vl3to1 = double.Parse(parts[9]);
                    double AcCur1 = double.Parse(parts[10]);
                    double AcVlt1 = double.Parse(parts[13]);

                    // Create a PvSample object and add it to the list
                    PvSample pvSample = new PvSample(Day, Hour, AcPwrt, DcVolt, Temper, Vl1to2, Vl2to3, Vl3to1, AcCur1, AcVlt1, int.Parse(parts[0]));
                    samples.Add(pvSample);
                    rowIndex++;
                }
                catch (Exception)
                {
                    // If there's an error parsing the line, log it to the rejected_client.CSV file
                    File.AppendAllText("rejected_client.CSV", line + Environment.NewLine);
                }
            }
            return samples;
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
                    Console.WriteLine("Clients resources has being releaced");
                }
                disposedValue = true;
            }
        }
    }
}
