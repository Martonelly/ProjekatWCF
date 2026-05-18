using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Client.Validations
{
    //Validation should be checked on service side, pleace move it there
    public class ValidateLine
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        static int PreviousRow { get; set; } 

        public void checkValidity(PvSample sample) {
            //Validation return false if not valid
            //TODO log files add row index checker 
            IsValid = false;
            if (sample.AcCur1 < 0 || sample.AcVlt1 < 0 || sample.Day < 0 || sample.DcVolt < 0 || sample.Vl1to2<0 || sample.Vl2to3<0 || sample.Vl3to1 <0) {
                Message = "The values are either negative or not real";
                return;
            }
            if (sample.AcPwrt <= 0) {
                Message = "The AcPwrt field is not valid";
                return;
            }
            //Sentinel treat it like null
            
            if (sample.DcVolt == 32767.0) {
                Message = "Volt value is sentinel";
                return;
            }
            if (sample.DcVolt == 32767.0)
            {
                Message = "DcVolt value is sentinel";
                return;
            }

            if (sample.AcCur1 == 32767.0)
            {
                Message = "AcCur1 value is sentinel";
                return;
            }

            if (sample.AcPwrt == 32767.0)
            {
                Message = "AcPwrt value is sentinel";
                return;
            }

            if (sample.AcVlt1 == 32767.0)
            {
                Message = "AcVlt1 value is sentinel";
                return;
            }

            if (sample.Temper == 32767.0)
            {
                Message = "Temper value is sentinel";
                return;
            }

            if (sample.Vl1to2 == 32767.0)
            {
                Message = "Vl1to2 value is sentinel";
                return;
            }

            if (sample.Vl2to3 == 32767.0)
            {
                Message = "Vl2to3 value is sentinel";
                return;
            }

            if (sample.Vl3to1 == 32767.0)
            {
                Message = "Vl3to1 value is sentinel";
                return;
            }

            try
            {
                if (sample.RowIndex <= PreviousRow)
                {
                    Message = "Row Index is not monoton";
                    return;
                }
            }
            catch { }
            PreviousRow = sample.RowIndex;
            Message = "Everything is ok";
            IsValid = true;

        }
        public ValidateLine() {
            PreviousRow = -1;
        }
    }
}
