using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public ReturnUser GetReturnUser(string username)
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

        public List<ReturnUser> ListOfUsers()
        {

            List<ReturnUser> returnUsers = new List<ReturnUser>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT users.user_id, users.username, accounts.account_id, accounts.balance FROM users JOIN accounts ON users.user_id = accounts.user_id", conn);
                // cmd.Parameters.AddWithValue("@user_id", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    ReturnUser user = GetUserFromReader(reader);

                    returnUsers.Add(user);

                }

            }
            return returnUsers;
        }

        // check balance is greater than 0, deduct transfer amount from sender, deposit trasnfer amt to receiver

        public bool BalanceIsSufficient(string username, decimal amtToTransfer)
        {
            ReturnUser user = GetReturnUser(username);
            return user.AccountBalance >= amtToTransfer;

        }

        private Transfer GetListOfTransfers(SqlDataReader reader)
        {
            return new Transfer()
            {
                SendingAccount = Convert.ToInt32(reader["account_from"]),
                ReceivingAccount = Convert.ToInt32(reader["account_to"]),
                AmountToTransfer = Convert.ToDecimal(reader["amount"]),
                TransferStatus = Convert.ToInt32(reader["transfer_status_id"]),
                TransferType = Convert.ToInt32(reader["transfer_type_id"]),
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                typeName = Convert.ToString(reader["typeName"]),
                StatusName = Convert.ToString(reader["transferName"]),
                ReceivingAccountName = Convert.ToString(reader["receivingName"])
            };

        }

        public List<Transfer> ListOfTransfers(int accountId)
        {
            List<Transfer> transferList = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT transfers.transfer_id, transfers.transfer_type_id, transfers.transfer_status_id, " +
                    "transfers.account_from, transfers.account_to, transfers.amount, users.username AS receivingName, transfer_types.transfer_type_desc AS typeName, " +
                    "transfer_statuses.transfer_status_desc AS transferName FROM transfers JOIN transfer_statuses ON transfers.transfer_status_id = " +
                    "transfer_statuses.transfer_status_id JOIN transfer_types ON Transfers.transfer_type_id = transfer_types.transfer_type_id " +
                    "JOIN accounts on transfers.account_to = accounts.account_id JOIN users ON accounts.user_id = users.user_id " +
                    "WHERE transfers.account_from = @accountId; ", conn);
                    cmd.Parameters.AddWithValue("@accountId", accountId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    
                    Transfer transfer = GetListOfTransfers(reader);
                   
                    transferList.Add(transfer);
                }
                

            }
            return transferList;
        }

        public Transfer GetTransferDetails(int transferId)
        {
            Transfer transfer = new Transfer();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT transfers.transfer_id, transfers.transfer_type_id, transfers.transfer_status_id, " +
                   "transfers.account_from, transfers.account_to, transfers.amount, users.username AS receivingName, transfer_types.transfer_type_desc AS typeName, " +
                   "transfer_statuses.transfer_status_desc AS transferName FROM transfers JOIN transfer_statuses ON transfers.transfer_status_id = " +
                   "transfer_statuses.transfer_status_id JOIN transfer_types ON Transfers.transfer_type_id = transfer_types.transfer_type_id " +
                   "JOIN accounts on transfers.account_to = accounts.account_id JOIN users ON accounts.user_id = users.user_id " +
                                               " WHERE transfers.transfer_id = @transferId ", conn);
                cmd.Parameters.AddWithValue("@transferId", transferId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();


                    return GetListOfTransfers(reader);
                }
                return null;
            }
        }
    }
}

