using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [DataContract]
    public class PvSample
    {
        

        //Properties
        [DataMember]
        public int Day { get; set; }
        [DataMember]
        public string Hour { get; set; } 
        [DataMember]
        public double AcPwrt { get; set; }
        [DataMember]
        public double DcVolt { get; set; }
        [DataMember]
        public double Temper { get; set; }
        [DataMember]
        public double Vl1to2 { get; set; }
        [DataMember]
        public double Vl2to3 { get; set; }
        [DataMember]
        public double Vl3to1 { get; set; }
        [DataMember]
        public double AcCur1 { get; set; }
        [DataMember]
        public double AcVlt1 { get; set; }
        [DataMember]
        public int RowIndex { get; set; }

        public PvSample(int day, string hour, double acPwrt, double dcVolt, double temper, double vl1to2, double vl2to3, double vl3to1, double acCur1, double acVlt1, int rowIndex)
        {
            Day = day;
            Hour = hour;
            AcPwrt = acPwrt;
            DcVolt = dcVolt;
            Temper = temper;
            Vl1to2 = vl1to2;
            Vl2to3 = vl2to3;
            Vl3to1 = vl3to1;
            AcCur1 = acCur1;
            AcVlt1 = acVlt1;
            RowIndex = rowIndex;
        }

        public PvSample(string CSVLine) {
            string[] splited = CSVLine.Split(',');
            try
            {
                RowIndex = Int32.Parse(splited[0]);
                Day = Int32.Parse(splited[1]);
                Hour = splited[2];
                AcPwrt = Double.Parse(splited[3]);
                DcVolt = Double.Parse(splited[4]);
                Temper = Double.Parse(splited[5]);
                Vl1to2 = Double.Parse(splited[6]);
                Vl2to3 = Double.Parse(splited[7]);
                Vl3to1 = Double.Parse(splited[8]);
                AcCur1 = Double.Parse(splited[9]);
                AcVlt1 = Double.Parse(splited[10]);
            }
            catch { }
            
        }
    }
}
