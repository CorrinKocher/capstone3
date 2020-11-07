using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class API_Transfer
    {
        public int SendingAccount { get; set; }

        public int ReceivingAccount { get; set; }

        public decimal AmountToTransfer { get; set; }

        public int TransferStatus { get; set; }

        public int SenderUserId { get; set; }

        public int ReceiverUserId { get; set; }

        public int TransferId { get; set; }

        public int TransferType { get; set; }

        public string typeName { get; set; }

        public string StatusName { get; set; }

        public string ReceivingAccountName { get; set; }





    }
}
