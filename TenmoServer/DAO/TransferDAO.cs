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
            

       public void TransferMoney(int sendingAccountId, int receivingAccountId, decimal amtToTransfer, int transferType = 1001, int transferStatus = 2001)
        {
            Transfer transfer = new Transfer();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount)VALUES(@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount)", conn);
                cmd.Parameters.AddWithValue("@transfer_type_id", transferType);
                cmd.Parameters.AddWithValue("@transfer_status_id",transferStatus);
                cmd.Parameters.AddWithValue("@account_from", sendingAccountId);
                cmd.Parameters.AddWithValue("@account_to", receivingAccountId);
                cmd.Parameters.AddWithValue("@amount", amtToTransfer);

                cmd.ExecuteNonQuery(); //Execute nonQuery


                SqlCommand cmd2 = new SqlCommand("UPDATE accounts SET balance = accounts.balance - @amount WHERE accounts.account_id = @accountid", conn);
                cmd2.Parameters.AddWithValue("@amount", amtToTransfer);
                cmd2.Parameters.AddWithValue("@accountid", sendingAccountId);

                cmd2.ExecuteNonQuery();//Execute nonquery

                SqlCommand cmd3 = new SqlCommand("UPDATE accounts SET balance = accounts.balance + @amount WHERE accounts.account_id = @accountid", conn);
                cmd3.Parameters.AddWithValue("@amount", amtToTransfer);
                cmd3.Parameters.AddWithValue("@accountid", receivingAccountId);

                cmd3.ExecuteNonQuery();//execute nonquery

                //TODO figure out how to roll this back if there is an error in one of these

                

            }
        }

    }
}
