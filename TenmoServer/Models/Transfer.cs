using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {

        int SendingAccount { get; set; }

        int ReceivingAccount { get; set; }

        decimal AmountToTransfer { get; set; }

        bool TransferStatus { get; set; } 

        int SendingUserId { get; set; }

        int ReceivingUserId { get; set; }
    }
}
