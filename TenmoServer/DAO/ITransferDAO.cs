using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        void TransferMoney(int sendingAccountId, int receivingAccountId, decimal amtToTransfer, int transferType = 1001, int transferStatus = 2001);
    }
}
