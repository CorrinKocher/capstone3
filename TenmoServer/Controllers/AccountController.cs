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
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO accountDAO; 
        private readonly ITransferDAO transferDAO;

        public AccountController(IAccountDAO accountDAO)
        {
            this.accountDAO = accountDAO;
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

        [HttpPost]

        public ActionResult<Transfer> InsertTransfer(int sendingAccountId, int receivingAccountId, decimal amtToTransfer, int transferType = 1001, int transferStatus = 2001)
        {
            return this.transferDAO.TransferMoney(sendingAccountId, receivingAccountId, amtToTransfer, transferType, transferStatus);
        }
    }
}
