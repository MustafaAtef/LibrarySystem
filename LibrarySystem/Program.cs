using System;
using Utilties;

namespace LibrarySystem {
    internal class Program {
        static User currentUser = null;
        static void Main(string[] args) {

            while(currentUser is null) {
                int status = Init();
                if (status == 3) return;
            }
            HandleUser();

            Console.ReadKey();
        }

        static void HandleUser() {
            if (currentUser is null) {
                Messages.PrintMessage("Something Went Wrong Try Again Later!", MessagesStatus.Error);
                return;
            }


            Console.WriteLine($"Hello, {currentUser.Name}!");

        }

        static int Init() {
            Messages.PrintMessage("Welcome Into The Library System", MessagesStatus.Intro);
            int choise = StartUpMenu();
            if (choise == 1) {
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
