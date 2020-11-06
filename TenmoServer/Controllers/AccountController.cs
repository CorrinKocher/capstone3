using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO accountDAO;
        private readonly ITransferDAO transferDAO;

        public AccountController(IAccountDAO accountDAO, ITransferDAO transferDAO)
        {
            this.accountDAO = accountDAO;
            this.transferDAO = transferDAO;
        }

        [HttpGet("{username}")]
        public ActionResult<ReturnUser> GetReturnUser(string username)
        {
            ReturnUser user = new ReturnUser();


            user = this.accountDAO.GetReturnUser(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        [HttpGet]

        public ActionResult<List<ReturnUser>> ListOfAccounts()
        {
            return this.accountDAO.ListOfUsers();

        }

        [HttpPost("maketransfer/{username}")]

        public void InsertTransfer(Transfer transfer)
        {
            this.transferDAO.TransferMoney(transfer.SendingAccount, transfer.ReceivingAccount, transfer.AmountToTransfer);
        }

        //Need http get for list of transfers
        [HttpGet("transferlist/{accountId}")] //accounts/accountId{Id}
        public ActionResult<List<Transfer>> ListTransfers(int accountId)
        {
            return this.accountDAO.ListOfTransfers(accountId);
        }

        // need http get for specific transfer
        [HttpGet("transfers/{transferId}")] //accounts/transferId{Id}
        public ActionResult<Transfer> GetTransferDetails(int transferId)
        {
            return this.accountDAO.GetTransferDetails(transferId);
        }

        
    }
}
