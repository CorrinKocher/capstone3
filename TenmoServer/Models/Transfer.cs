using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {

        public int SendingAccount { get; set; }

        public int ReceivingAccount { get; set; }

        public decimal AmountToTransfer { get; set; }

        public int TransferStatus { get; set; } = 2001;

        public int TransferType { get; set; } = 1001;

        public int TransferID { get; set; }

        

      
    }
}
