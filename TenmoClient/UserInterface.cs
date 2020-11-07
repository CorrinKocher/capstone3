using System;
using System.Collections.Generic;
using System.Dynamic;
using TenmoClient.APIClients;
using TenmoClient.Data;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly AccountService accountService = new AccountService();

        private bool shouldExit = false;

        public void Start()
        {
            while (!shouldExit)
            {
                while (!authService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
            }
            else if (loginRegister == 2)
            {
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");// not required
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");// not required!!!
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    switch (menuSelection)
                    {
                        case 1:
                            GetBalance(UserService.Username);

                            break;
                        case 2:
                            GetListOfTransfers();

                            break;
                        case 3:
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;
                        case 4:
                            GetListOfUsers();
                            TransferMoney(UserService.Username);


                            break;
                        case 5:
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;
                        case 6:
                            Console.WriteLine();
                            UserService.SetLogin(new API_User()); //wipe out previous login info
                            return;
                        default:
                            Console.WriteLine("Goodbye!");
                            shouldExit = true;
                            return;
                    }
                }
            }
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();
                API_User user = authService.Login(loginUser);
                if (user != null)
                {
                    UserService.SetLogin(user);
                    accountService.UpdateToken(user.Token);
                }
            }
        }

        private void GetBalance(string username)
        {
            API_Account account = accountService.GetBalance(username);


            if (account != null)
            {
                string accountString = $"Your account balance is: ${account.AccountBalance}";
                Console.WriteLine(accountString);
            }
            else
            {
                Console.WriteLine("The account number is not found.");
            }
        }

        private void GetListOfUsers()
        {
            List<API_Account> accounts = accountService.GetListOfUsers();
            foreach (API_Account account in accounts)
            {
                if (account.Username != UserService.Username)
                {
                    string accountNameAndAccountId = $"{account.AccountId} ) Username: {account.Username}";
                    Console.WriteLine(accountNameAndAccountId);
                }
            }
        }

        private int SelectUserToReceiveTransfer()
        {
            Console.WriteLine("Please enter the ID of the user you would like to transfer to (ex: 4000)");
            int userIdSelected = int.Parse(Console.ReadLine());
            return userIdSelected;
        }

        private decimal SelectAmtToTransfer()
        {
            Console.WriteLine("Please enter the decimal amount of how much money you would like to transfer (ex: 12.00)");
            decimal amtToTransfer = decimal.Parse(Console.ReadLine());
            return amtToTransfer;
        }

        private void TransferMoney(string username)
        {
            bool isSufficient;
            int receivingUser = SelectUserToReceiveTransfer();
            decimal amtToTransfer = SelectAmtToTransfer();
            API_Account account = accountService.GetBalance(username);
            API_Transfer transfer = new API_Transfer();
            transfer.SendingAccount = account.AccountId;
            transfer.ReceivingAccount = receivingUser;
            transfer.AmountToTransfer = amtToTransfer;

            isSufficient = accountService.RequestToTransferToAnotherAccount(username, amtToTransfer, transfer);
            while (!isSufficient)
            {
                SelectAmtToTransfer();
                Console.WriteLine($"You do not have sufficient funds for the transfer, your current balance is  ${account.AccountBalance}.");
            }

        }

        private void GetListOfTransfers()
        {
            API_Account Sender = accountService.GetBalance(UserService.Username);
            List<API_Transfer> transfers = accountService.GetListOfTransfers(Sender.AccountId);
            foreach (API_Transfer transfer in transfers)
            {

                string transferDetailsString = $"{transfer.TransferId} ) Transfer To: {transfer.ReceivingAccountName} Transfer amount: ${transfer.AmountToTransfer}";
                Console.WriteLine(transferDetailsString);

            }
            Console.WriteLine("Please enter the transfer ID to view the details of the transfer");
            int selectedTransfer = int.Parse(Console.ReadLine());
            API_Transfer transferToDisplay = accountService.GetTransferDetails(selectedTransfer);

            string transferDetails = $" ID: {transferToDisplay.TransferId} \n FROM: {UserService.Username} " +
                $"\n TO:{transferToDisplay.ReceivingAccount} \n Type: {transferToDisplay.typeName} \n Status: {transferToDisplay.StatusName} " +
                $"\n Amount: ${transferToDisplay.AmountToTransfer}  ";
            Console.WriteLine(transferDetails);
        }

        //private void GetTransferDetails()
        //{
        //    Console.WriteLine("Please enter the transfer ID to view the details of the transfer");

        //    API_Transfer transfer = new API_Transfer();

        //    transfer.TransferId = int.Parse(Console.ReadLine());


        //    accountService.GetTransferDetails(transfer.TransferId);


        //}

    }
}
