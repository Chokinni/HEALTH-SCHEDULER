using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Media;

namespace APPOINTMENTSCHEDULER
{
    class InputFormat
    {
        string pattern;

        //Method to check the format of Teacher ID. returns ID if the format is correct and throws an Exception if not.
        public string DoctorIDFormat(string id)
        {
            pattern = @"^D\d{2}-\d{3}-\d{3}$";
            while (true)
            {
                if (Regex.IsMatch(id, pattern))
                {
                    return id;
                }
                else
                {
                    throw new FormatException("Invalid ID. Follow this format: D00-000-000");
                }
            }
        }

        //Method to check the format of Student ID. returns ID if the format is correct and throws an Exception if not.
        public string PatientIDFormat(string id)
        {
            pattern = @"^P\d{2}-\d{3}-\d{3}$";
            while (true)
            {
                if (Regex.IsMatch(id, pattern))
                {
                    return id;
                }
                else
                {
                    throw new FormatException("Invalid ID. Follow this format: P00-000-000");
                }
            }
        }

        //Method to hide input (password) 
        public string HidePassword()
        {
            string password = string.Empty;
            while (true)
            {
                var passwordKey = Console.ReadKey(intercept: true);
                if (passwordKey.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (passwordKey.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Remove(password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password += passwordKey.KeyChar;
                    Console.Write("*");
                }
            }
            return password;
        }
    }
}
