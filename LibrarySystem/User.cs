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

        public User(string id, string name, string email, string phone, int role) {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            Role = role;
        }

        public string Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }

        public int Role { get; } = 0;

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
                    return new User(username, name, email,phone);
                } else {
                    Messages.PrintMessage("Something Went Wrong! Try Again Later", MessagesStatus.Error);
                    return null;
                }
            }
        }

        public static User Login() {
            while (true) {
                bool error = false;
                Messages.PrintMessage("Login Credentials");
                Console.Write(" - Username: ");
                string username = Convert.ToString(Console.ReadLine());
                Console.Write(" - Password: ");
                string password = Convert.ToString(Console.ReadLine());
            
                if (!isUserInfoValid(username, password)) {
                    Messages.PrintMessage("Invalid Data Is Entered!", MessagesStatus.Error);
                    error = true;
                } else {
                    User loggedIn = AuthenticateUser(username, password);
                    if (loggedIn is null) {
                        Messages.PrintMessage("Username Or Password Is Incorrect!", MessagesStatus.Error);
                        error = true;
                    }
                    else {
                        Messages.PrintMessage("Logged In Successfully!", MessagesStatus.Success);
                        return loggedIn;
                    }
                }
                if (error) {
                    int choise = -1;
                    while (choise < 1 || choise > 2) {
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
            }
        }

        public static void ShowAllUsers() {
            Console.Clear();
            Messages.PrintMessage("Users Data", MessagesStatus.Intro);
            List<User> users = new List<User>();
            using var usersFileReader = new StreamReader(USERS_FILE);
            while(!usersFileReader.EndOfStream) {
                string userInfo = usersFileReader.ReadLine() ?? "";
                if (userInfo.Length <= 0) break;
                var curUser = covertStringToUser(userInfo);
                if (curUser is null) continue;
                users.Add(curUser);
            }
            int usernameMaxLength = int.MinValue, nameMaxLength = int.MinValue, emailMaxLength = int.MinValue, phoneMaxLength = int.MinValue;
            foreach (var user in users) {
                usernameMaxLength = Math.Max(usernameMaxLength, user.Id.Length);
                nameMaxLength = Math.Max(nameMaxLength, user.Name.Length);
                emailMaxLength = Math.Max(emailMaxLength, user.Email.Length);
                phoneMaxLength = Math.Max(phoneMaxLength, user.Phone.Length);
            }

            for (int i = 0; i < usernameMaxLength + nameMaxLength+ emailMaxLength + phoneMaxLength + 17; i++)
                Console.Write("-");
            Console.WriteLine();
            Console.WriteLine($"| {"Username".PadRight(usernameMaxLength+2, ' ')}| {"Name".PadRight(nameMaxLength + 2, ' ')}| {"Email".PadRight(emailMaxLength + 2, ' ')}| {"Phone".PadRight(phoneMaxLength + 2, ' ')}|");
            for (int i = 0; i < usernameMaxLength + nameMaxLength + emailMaxLength + phoneMaxLength + 17; i++)
                Console.Write("-");
            Console.WriteLine();
            foreach (var u in users) {
                Console.WriteLine($"| {u.Id.PadRight(usernameMaxLength +2, ' ')}| {u.Name.PadRight(nameMaxLength +2, ' ')}| {u.Email.PadRight(emailMaxLength +2, ' ')}| {u.Phone.PadRight(phoneMaxLength +2, ' ')}|");
                for (int i = 0; i < usernameMaxLength + nameMaxLength + emailMaxLength + phoneMaxLength + 17; i++)
                    Console.Write("-");
                Console.WriteLine();
            }

            int choise = -1;
            while(choise != 1) {
                Console.Write("Enter (1) To Return To The Main Menu: ");
                int.TryParse(Console.ReadLine(), out choise);
                if (choise != 1) Messages.PrintMessage("Invalid Option!", MessagesStatus.Error);
            }
            return;

        }

        private static User covertStringToUser(string userInfoString) {
            string[] userInfo = userInfoString.Split(",");
            if (userInfo[0] == "admin") return null;
            return new User(userInfo[0], userInfo[2], userInfo[3], userInfo[4]);
        }

        private static User AuthenticateUser(string username, string password) {
            using var userFileReader = new StreamReader(USERS_FILE);
            while(!userFileReader.EndOfStream) {
                string user = userFileReader.ReadLine() ?? "";
                if (!(user.Length > 0)) return null;
                string[] userInfo = user.Split(",");
                int role = 0;
                int.TryParse(userInfo[5], out role);
                if (userInfo[0] == username && userInfo[1] == password) return new User(userInfo[0], userInfo[2], userInfo[3], userInfo[4], role);
            }
            return null;
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

        private static bool isUserInfoValid(params string[] info) {
            bool valid = true;
            foreach (var i in info) {
                if (!(i.Length > 0)) {
                    valid = false;
                    break;
                }
            }
            return valid;
        }
    }
}
