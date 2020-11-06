using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class API_Transfer
    {
        public API_Transfer(int sendingAccount, int receivingAccount, decimal amount)
        {
            this.account_from = sendingAccount;
            this.account_to = receivingAccount;
            this.amount = amount;
        }
        int account_from { get; set; }

        int account_to { get; set; }

        decimal amount { get; set; }

        int transfer_status_id { get; set; } = 2001;

        int transfer_type_id { get; set; } = 1001;

                            
    }
}
