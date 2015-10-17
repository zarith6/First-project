using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security;

namespace SampleDBandAuth
{
    class Program
    {
        

        static void Main(string[] args)
        {
            
            
            int userSelection = 0;
            

            do
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1) Register Account");
                Console.WriteLine("2) Sign in");
                Console.WriteLine("3) Exit\n");
            
                userSelection = Convert.ToInt32(Console.ReadLine());
                

                if (userSelection == 1)
                {
                    createUser();
                }
                
                else if (userSelection == 2)
                {
                    if (signInUser())
                    {
                        signInSuccess();
                    }
                }
            } while (userSelection != 3);

            
        }

        public static void createUser()
        {
            using (ConsoleDbContext db = new ConsoleDbContext())
            {
                //Initialize variables
                ConsoleKeyInfo temp;
                string pwdBuilder = null;

                Console.WriteLine("Enter a username");
                string userName = Console.ReadLine();

                //query to make sure username isnt taken
                var query = from users in db.Users
                            where userName == users.Username
                            select users;

                if (query.Count() == 0)
                {
                    Console.WriteLine("Enter a password");
                    do
                    {
                        temp = Console.ReadKey(true);
                        pwdBuilder += temp.KeyChar;
                    } while (temp.Key != ConsoleKey.Enter);
                    Console.WriteLine("Generating salt...");
                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    Console.WriteLine("Hashing password...");
                    string hashedPwd = BCrypt.Net.BCrypt.HashPassword(pwdBuilder, salt);
                    Console.WriteLine("Creating account...");
                    var user = new User { Username = userName, HashedPwd = hashedPwd, Salt = salt };
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Username already taken");
                    Console.WriteLine("Returning to main menu");
                }

                
            }
        }

        public static bool signInUser()
        {
            ConsoleKeyInfo temp;
            string pwdBuilder = null;

            using (ConsoleDbContext db = new ConsoleDbContext())
            {
                Console.WriteLine("Enter your username:");
                string userName = Console.ReadLine();
                Console.WriteLine("Enter your password");
                do
                {
                    temp = Console.ReadKey(true);
                    pwdBuilder += temp.KeyChar;
                } while (temp.Key != ConsoleKey.Enter);

                
                  //Pull user from db using entered user name
                

                var query = from users in db.Users
                            where users.Username == userName
                            select users;

                User[] tempUser = query.ToArray();

                //Hash password
                Console.WriteLine("Hashing password...");
                string tempHash = BCrypt.Net.BCrypt.HashPassword(pwdBuilder, tempUser[0].Salt);

                Console.WriteLine("Verifying that password is correct...");
                if (tempHash == tempUser[0].HashedPwd)
                {
                    Console.WriteLine("Log in success!");
                    return true;
                }
                else
                {
                    Console.WriteLine("Log in failed");
                    return false;
                }
                
            }
        }

        public static void signInSuccess()
        {
            List<User> userList = new List<User>();
            int userSelection = 0;
            do
            {
                Console.WriteLine("Welcome! What would you like to do?");
                Console.WriteLine("1) Print all users");
                Console.WriteLine("2) Sign out");
                userSelection = Convert.ToInt32(Console.ReadLine());

                if (userSelection == 1)
                {
                    using (ConsoleDbContext db = new ConsoleDbContext())
                    {
                        var query = from users in db.Users
                                    select users;

                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Username);
                        }

                    }
                }

            } while (userSelection != 2);
        }

    }
}
