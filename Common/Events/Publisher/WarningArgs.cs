using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events.Publisher
{
    public class WarningArgs : EventArgs
    {
        public string Message { get; set; }

        public WarningArgs(string message){
            Message = message;
        }
    }
}
