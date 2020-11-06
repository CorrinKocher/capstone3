using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    public class AccountService
    {
        private readonly string BASE_URL;
        private readonly RestClient client;

        public AccountService()
        {
            this.BASE_URL = AuthService.API_BASE_URL + "account";

            this.client = new RestClient();
        }

        public void UpdateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                this.client.Authenticator = null;
            }
            else
            {
                this.client.Authenticator = new JwtAuthenticator(token);
            }
        }

        public API_Account GetBalance(string username)
        {
            RestRequest request = new RestRequest(BASE_URL + "/" + username);

            var response = client.Get<API_Account>(request);


            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occurred fetching the balance");

                return null;
            }
        }

        public List<API_Account> GetListOfUsers()
        {

            RestRequest request = new RestRequest(BASE_URL);
            IRestResponse<List<API_Account>> response = client.Get<List<API_Account>>(request);


            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occurred fetching the balance");

                return null;
            }
        }

        public bool RequestToTransferToAnotherAccount(string username, decimal amtToTransfer, API_Transfer transfer)
        {
            bool isSufficient;
            API_Account account = GetBalance(username);
            if (account == null)
            {
                isSufficient = false;
            }
            else if (account.AccountBalance < amtToTransfer)
            {
                Console.WriteLine($"You do not have sufficient funds for the transfer, your current balance is  ${account.AccountBalance}.");
                isSufficient = false;
            }
            else
            {
                Console.WriteLine("The Transfer was successful.");
                RestRequest request = new RestRequest(BASE_URL + "/" + username);
                API_Transfer thisTransfer = transfer;
                request.AddJsonBody(transfer);
                var response = client.Post<API_Account>(request);

                isSufficient = true;
            }
            return isSufficient;
        }


    }
}
