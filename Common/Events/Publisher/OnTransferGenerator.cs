using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events.Publisher
{
    public class OnTransferGenerator
    {
        public delegate void TransferEventHandler(object sender, OnTransferArgs e);

        public event TransferEventHandler TransferStartedEvent;
        public event TransferEventHandler TransferCompletedEvent;
        public event TransferEventHandler SampleRecievedEvent;

        public void ProcessTransfer(TransferType type) {
            switch (type) { 
                case TransferType.Start:
                    TransferStarted();
                    break;
                case TransferType.Complete:
                    TransferCompleted();
                    break;
                case TransferType.Recieved:
                    SampleRecieved();
                    break;
            }
        }
        private void TransferStarted() {
            if ( TransferStartedEvent != null) {
                TransferStartedEvent(this, new OnTransferArgs("Transfer Started"));
            }
        }

        private void TransferCompleted()
        {
            if (TransferCompletedEvent != null)
            {
                TransferCompletedEvent(this, new OnTransferArgs("Transfer Complete"));
            }
        }

        private void SampleRecieved() {
            if (SampleRecievedEvent != null) {
                SampleRecievedEvent(this, new OnTransferArgs("Sample Recieved"));
            }
        }

    }
}
