using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace APPOINTMENTSCHEDULER
{
    abstract class UserAuthentication
    {
        protected string Username { get; set; }
        protected string Password { get; set; }

        protected string Firstname { get; set; }
        protected string Lastname { get; set; }

        protected AuthManager authManager = new AuthManager();
        protected InputFormat input = new InputFormat();
        protected Doctor doctor = new Doctor();
        protected Patient patient = new Patient();

        public virtual void Menu()
        {
            while (true)
            {
                Console.Clear(); // Clear the console for a fresh start
                Console.WriteLine("=========================================");
                Console.WriteLine(" Welcome to the Healthcare Appointment Scheduler! ");
                Console.WriteLine("=========================================");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");
                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.Clear();
                        Registration();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case 2:
                        Console.Clear();
                        Login();
                        return;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please ry again");
                        break;
                }
            }
        }

        public virtual void Registration()
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("               Register User             ");
            Console.WriteLine("=========================================");
            Console.Write("Enter firstname: ");
            Firstname = Console.ReadLine().Trim().ToLower();
            Console.Write("Enter lastname: ");
            Lastname = Console.ReadLine().Trim().ToLower();
            Console.Write("Enter username: ");
            Username = Console.ReadLine();
        }

        public virtual void Login()
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("               User Login                ");
            Console.WriteLine("=========================================");
            Console.Write("Enter username: ");
            Username = Console.ReadLine();
        }
    }

    class DoctorOperation: UserAuthentication
    {
        public override void Menu()
        {
            base.Menu();
        }

        public override void Registration()
        {
            while (true)
            {
                try
                {
                    base.Registration();
                    input.DoctorIDFormat(Username);
                    if (!authManager.CheckID(Username))
                    {
                        Console.Write("Enter password: ");
                        Password = input.HidePassword();


                        //Example rani chuck
                        Console.WriteLine("\nPediatrician");
                        Console.WriteLine("Surgeon");
                        Console.WriteLine("Gynecologist");
                        Console.WriteLine("Cardiologist");
                        Console.WriteLine("Hematologist");
                        Console.WriteLine("Dermatologist");
                        string specialization;
                        do
                        {
                            Console.Write("Enter Specialization: ");
                            specialization = Console.ReadLine().Trim().ToLower();
                        } while (specialization != "pediatrician" && specialization != "surgeon" && specialization != "gynecologist" && specialization != "cardiologist" && specialization != "hematologist" && specialization != "dermatologist");
                        authManager.Register(Firstname, Lastname, Username, Password, "doctor", specialization);
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Doctor ID already exists.");
                    }
                }

                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public override void Login()
        {
            while (true)
            {
                try
                {
                    base.Login();
                    input.DoctorIDFormat(Username);

                    Console.Write("Enter password: ");
                    Password = input.HidePassword();
                    if (authManager.Login(Username, Password))
                    {
                        doctor.Operations(Username);
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("        Invalid Login Credentials        ");
                        Console.WriteLine("=========================================");
                        Console.WriteLine("The username or password is incorrect. Please try again.");
                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    class PatientOperation: UserAuthentication
    {
        public override void Menu()
        {
            base.Menu();
        }

        public override void Registration()
        {
            while (true)
            {
                try
                {
                   
                    base.Registration();
                    input.PatientIDFormat(Username);
                    if (!authManager.CheckID(Username))
                    {
                        Console.Write("Enter password: ");
                        Password = input.HidePassword();
                        authManager.Register(Firstname, Lastname, Username, Password, "patient", "none");
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Patient ID already exists");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public override void Login()
        {
            while (true)
            {
                try
                {
                    base.Login();
                    input.PatientIDFormat(Username);

                    Console.Write("Enter password: ");
                    Password = input.HidePassword();
                    if (authManager.Login(Username, Password))
                    {
                        patient.Operations(Username);
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("        Invalid Login Credentials        ");
                        Console.WriteLine("=========================================");
                        Console.WriteLine("The username or password is incorrect. Please try again.");
                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
