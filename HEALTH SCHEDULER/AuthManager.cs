using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Media;

namespace APPOINTMENTSCHEDULER
{
    public class AuthManager
    {
        //private const string directoryPath = @"C:\Users\user1\source\vsstudio\CPE261\AppointmentScheduler";
        private string userFilePath;
        const string directoryPath = @"D:\FILEHANDLINGuwu";

        public AuthManager()
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
                userFilePath = Path.Combine(directoryPath, "AuthManager.txt");

                if (!File.Exists(userFilePath))
                {
                    using (FileStream fs = File.Create(userFilePath)) { }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"ERROR: File operation failed.");
            }
        }

        public bool Register(string firstname, string lastname, string username, string password, string role, string specialty) /// Registers a new user by saving their information to the user file.
        {
            // Check if username already exists
            if (File.Exists(userFilePath))
            {
                using (StreamReader reader = new StreamReader(userFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(',');
                        if (parts[0] == username)
                        {
                            Console.WriteLine("Username already exists. Please try another.");
                            return false;
                        }
                    }
                }
            }

            // Register the user
            using (StreamWriter writer = new StreamWriter(userFilePath, true))
            {
                writer.WriteLine($"{firstname},{lastname},{username},{password},{role},{specialty}");
            }
            Console.WriteLine("\nUser registered successfully.");
            return true;
        }

        public bool Login(string username, string password) /// Logs a user in by verifying their username and password.
        {
            if (!File.Exists(userFilePath))
            {
                Console.WriteLine("No users registered yet.");
                return false;
            }

            using (StreamReader reader = new StreamReader(userFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts[2] == username && parts[3] == password)
                    {
                        Console.WriteLine("Login successful.");
                        return true;
                    }
                }
            }

            Console.WriteLine("Invalid username or password.");
            return false;
        }

        public bool AvailableDoctors(string specialty)
        {
            try
            {
                if (!File.Exists(userFilePath))
                {
                    Console.WriteLine("ERROR: User file not found.");
                    return false;
                }

                bool found = false;
                using (StreamReader read = new StreamReader(userFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 6)
                        {
                            string storedFirstname = parts[0];
                            string storedLastname = parts[1];
                            string Username = parts[2];
                            string Password = parts[3];
                            string role = parts[4];
                            string specialization = parts[5];
                            storedFirstname = char.ToUpper(storedFirstname[0]) + storedFirstname.Substring(1).ToLower();
                            storedLastname = char.ToUpper(storedLastname[0]) + storedLastname.Substring(1).ToLower();
                            if (role == "doctor" && specialization == specialty)
                            {
                                Console.WriteLine($"{Username} - Dr. {storedFirstname} {storedLastname}");
                            }
                        }
                    }
                }
                return found;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File not read");
                return false;
            }
        }

        public bool CheckDoctor (string doctor, string specialty)
        {
            try
            {
                if (!File.Exists(userFilePath))
                {
                    Console.WriteLine("ERROR: User file not found.");
                    return false;
                }

                bool found = false;
                using (StreamReader read = new StreamReader(userFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 6)
                        {
                            string Username = parts[2];
                            string specialization = parts[5];
                            if (specialization == specialty && Username == doctor)
                            {
                                found = true;
                            }
                        }
                    }
                }
                return found;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File not read");
                return false;
            }
        }

        public (string,string) Name (string username)
        {
            try
            {
                using (StreamReader read = new StreamReader(userFilePath))
                {
                    string line;

                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 6)
                        {
                            string Firstname = parts[0];
                            string Lastname = parts[1];
                            string Username = parts[2];
                            Firstname = char.ToUpper(Firstname[0]) + Firstname.Substring(1).ToLower();
                            Lastname = char.ToUpper(Lastname[0]) + Lastname.Substring(1).ToLower();
                            if (Username == username)
                            {
                                return (Firstname, Lastname);
                            }
                        }
                    }
                }
                return (null, null);
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File not read");
                return (null,null);
            }
        }
        public bool CheckID(string id)
        {
            try
            {
                if (!File.Exists(userFilePath))
                {
                    Console.WriteLine("ERROR: User file not found.");
                    return false;
                }

                using (StreamReader read = new StreamReader(userFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] credentials = line.Split(',');
                        if (credentials.Length == 6)
                        {
                            string storedID = credentials[2].Trim();
                            if (storedID == id)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Trouble checking ID.");
            }
            return false;
        }
    }
}
