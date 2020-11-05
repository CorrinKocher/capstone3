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
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO accountDAO;

        public AccountController(IAccountDAO accountDAO)
        {
            this.accountDAO = accountDAO;
        }

        [HttpGet("{username}")]
        public ActionResult<ReturnUser> GetBalance(string username)
        {
            ReturnUser user = new ReturnUser();

         
            user = this.accountDAO.GetBalance(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

    }
}
