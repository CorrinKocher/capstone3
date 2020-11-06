using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferDAO : ITransferDAO
    {
        private readonly string connectionString;

        
        public TransferDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;

        }
            

       public Transfer TransferMoney(int sendingAccountId, int receivingAccountId, decimal amtToTransfer, int transferType = 1001, int transferStatus = 2001)
        {
            Transfer transfer = new Transfer();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount)VALUES(@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount)");
                cmd.Parameters.AddWithValue("@transfer_type_id", transferType);
                cmd.Parameters.AddWithValue("@transfer_status_id",transferStatus);
                cmd.Parameters.AddWithValue("@account_from", sendingAccountId);
                cmd.Parameters.AddWithValue("@account_to", receivingAccountId);
                cmd.Parameters.AddWithValue("@amount", amtToTransfer);


                SqlCommand cmd2 = new SqlCommand("UPDATE accounts SET balance = accounts.balance - @amount WHERE accounts.account_id = @accounts.account_id");
                cmd2.Parameters.AddWithValue("@amount", amtToTransfer);
                cmd2.Parameters.AddWithValue("@accounts.account_id", sendingAccountId);

                SqlCommand cmd3 = new SqlCommand("UPDATE accounts SET balance = accounts.balance + @amount WHERE accounts.account_id = @accounts.account_id");
                cmd2.Parameters.AddWithValue("@amount", amtToTransfer);
                cmd2.Parameters.AddWithValue("@accounts.account_id", receivingAccountId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    this.
                }

            }
        }

    }
}
