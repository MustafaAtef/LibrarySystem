using System;
using Utilties;

namespace LibrarySystem {
    internal class Program {
        static User currentUser = null;
        static void Main(string[] args) {
            while(true) {
                Console.Clear();
               int status = init();
                if (status == 3) return;
                else if (currentUser is not null)
                    routeHandling();
            }
        }

        static void routeHandling() {
            if (currentUser is null) {
                Messages.PrintMessage("Something Went Wrong Try Again Later!", MessagesStatus.Error);
                return;
            }
            Console.Clear();
            if (currentUser.Role == 1)
                adminHandling();
            else
                regUserHandling();
            currentUser = null;
        }

        private static void regUserHandling() {
            Messages.PrintMessage($"Welcome User, {currentUser.Name}", MessagesStatus.Intro);

        }

        private static void adminHandling() {
            while(true) {
                int choise = adminMenu(currentUser.Role);
                if (choise == 5) {
                    User.ShowAllUsers();
                    Console.Clear();
                } else if (choise == 8) return;
            }


        }

        private static int adminMenu(int role) {
            int choise = -1;
            Messages.PrintMessage($"Welcome Admin, {currentUser.Name}", MessagesStatus.Intro);
            while (choise < 1 || choise > 8) {
                int i = 1;
                Console.WriteLine($"  {i++}-Add Book");
                Console.WriteLine($"  {i++}-Show All Books");
                Console.WriteLine($"  {i++}-Show Books By Name");
                Console.WriteLine($"  {i++}-Show Books For a User");
                Console.WriteLine($"  {i++}-Show All Users");
                Console.WriteLine($"  {i++}-Show All Users' Books");
                Console.WriteLine($"  {i++}-Show Users By Name");
                Console.WriteLine($"  {i++}-Sign Out");
                Console.Write("\nEnter Your Choise: ");
                if (!int.TryParse(Console.ReadLine(), out choise) || choise < 1 || choise > 8) {
                    Messages.PrintMessage("Enter Valid Choise!", MessagesStatus.Error);
                }
            }
            return choise;
        }

        static int init() {
            Messages.PrintMessage("Welcome Into The Library System", MessagesStatus.Intro);
            int choise = StartUpMenu();
            if (choise == 1) {
                currentUser = User.Login();
                return 1; 
            } else if (choise == 2) {
               currentUser = User.SignUp();
                return 2;
            } else {
                Messages.PrintMessage("Bye Bye!", MessagesStatus.Exit);
                return 3;
            }
            
        }
        static int StartUpMenu() {
            int choise = -1;
            while (choise < 1 || choise > 3) {
                Console.WriteLine("Authentication Gate");
                Console.WriteLine("  1-Login");
                Console.WriteLine("  2-Signup");
                Console.WriteLine("  3-Exit");
                Console.Write("\nEnter Your Choise: ");
                if (!int.TryParse(Console.ReadLine(), out choise) || choise < 1 || choise > 3) {
                    Messages.PrintMessage("Enter Valid Choise!", MessagesStatus.Error);
                }
            }
            return choise;
        }
    }
}
