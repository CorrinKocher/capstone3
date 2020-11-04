using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class API_Account
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Token { get; set; }

        public int AccountId { get; set; }

        public decimal AccountBalance { get; set; }
    }
}
