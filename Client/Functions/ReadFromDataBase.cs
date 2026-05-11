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
            using (FileStream fs = File.OpenRead(path))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string headerLine = sr.ReadLine(); // Skip the header line
                    string line;
                    List<PvSample> samples = new List<PvSample>();
                    int rowIndex = 0;

                    while ((line = sr.ReadLine()) != null && rowIndex < limitN)
                    {
                        try
                        {
                            string[] parts = line.Split(',');
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

                            PvSample pvSample = new PvSample(Day, Hour, AcPwrt, DcVolt, Temper, Vl1to2, Vl2to3, Vl3to1, AcCur1, AcVlt1, int.Parse(parts[0]));
                            for (int i = 0; i < limitN; i++)
                            {
                                samples.Add(pvSample);
                            }
                        }
                        catch (Exception)
                        {
                            File.AppendAllText("rejected_client.CSV", line + Environment.NewLine);
                        }
                    }
                    return samples;
                }
            }
        }

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
