using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountDAO : IAccountDAO
    {
        private readonly string connectionString;

        public AccountDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public ReturnUser GetBalance(string username)
        {
            

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT users.user_id, users.username, accounts.account_id, accounts.balance FROM users JOIN accounts ON users.user_id = accounts.user_id  WHERE users.username = @username", conn);
                cmd.Parameters.AddWithValue("@username", username);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    return GetUserFromReader(reader);
                   
                }


            }
            return null;
        }

        private ReturnUser GetUserFromReader(SqlDataReader reader)
        {
            return new ReturnUser()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                Username = Convert.ToString(reader["username"]),
                AccountId = Convert.ToInt32(reader["account_id"]),
                AccountBalance = Convert.ToDecimal(reader["balance"])
            };
        }
    }
}
