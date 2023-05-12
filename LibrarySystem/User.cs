using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utilties;

namespace LibrarySystem {
    public class User {
        private const string USERS_FILE = @"E:\Programming\cs_learning\LibrarySystem\LibrarySystem\users.txt";

        public User(string id, string name, string email, string phone) {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
        }

        public string Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }

        public static User SignUp() {
            while(true) {
                bool error = false;
                Messages.PrintMessage("User Info");
                Console.Write(" - Username: ");
                string username = Convert.ToString(Console.ReadLine());
                Console.Write(" - Password: ");
                string password = Convert.ToString(Console.ReadLine());
                Console.Write(" - Name: ");
                string name = Convert.ToString(Console.ReadLine());
                Console.Write(" - Email: ");
                string email = Convert.ToString(Console.ReadLine());
                Console.Write(" - Phone: ");
                string phone = Convert.ToString(Console.ReadLine());
                if (!isUserInfoValid(username, password, name, email, phone)) {
                    Messages.PrintMessage("Invalid Data Is Entered!", MessagesStatus.Error);
                    error = true;
                } else if (isUserExists(username)) {
                    Messages.PrintMessage("This Username Is Already Exists!", MessagesStatus.Error);
                    error = true;
                }
                if (error) {
                    int choise = -1;
                    while(choise < 1 || choise > 2) {
                        Console.WriteLine("Options:-");
                        Console.WriteLine("\t1-Try Again");
                        Console.WriteLine("\t2-Back To Main Menu");
                        Console.Write("Enter Your Option: ");
                        if (!int.TryParse(Console.ReadLine(), out choise) || choise < 1 || choise > 2) {
                            Messages.PrintMessage("Enter Valid Option!", MessagesStatus.Error);
                        } else break;
                    }
                    if (choise == 1) continue;
                    else return null;
                }
                if(saveUserInfo(username, password, name, email, phone)) {
                    Messages.PrintMessage("Signed Up Successfully!", MessagesStatus.Success);
                    return new User(username, name, email,phone);
                } else {
                    Messages.PrintMessage("Something Went Wrong! Try Again Later", MessagesStatus.Error);
                    return null;
                }
            }
        }

        private static bool saveUserInfo(string username, string password, string name, string email, string phone) {
            using var usersFileWriter = new StreamWriter(USERS_FILE, true);
            usersFileWriter.WriteLine($"{username},{password},{name},{email},{phone},0");
            return true;
        }

        private static bool isUserExists(string username) {
            using var usersFileReader = new StreamReader(USERS_FILE);
            while(!usersFileReader.EndOfStream) {
                string user = usersFileReader.ReadLine() ?? "";
                if (user.Length > 0) {
                    string[] userInfo = user.Split(",");
                    if (username == userInfo[0]) return true;
                } else 
                    return true;
            }
            return false;
        }

        private static bool isUserInfoValid(string username, string password, string name, string email, string phone) {
            if (username.Length > 0 && password.Length > 0 && name.Length > 0 && email.Length > 0 && phone.Length > 0) return true;
            return false;
        }
    }
}
