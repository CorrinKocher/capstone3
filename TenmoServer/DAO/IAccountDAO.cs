using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        ReturnUser GetReturnUser(string username);

        List<ReturnUser> ListOfUsers();

        List<Transfer> ListOfTransfers(int accountId);

        Transfer GetTransferDetails(int transferId);
    }
}
