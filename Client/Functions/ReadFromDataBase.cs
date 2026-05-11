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
    public class ReadFromDataBase : IReadFromDataBase
    {
        public void FReadFromDataBase(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
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
                    }
                }
            }
        }
    }
}
