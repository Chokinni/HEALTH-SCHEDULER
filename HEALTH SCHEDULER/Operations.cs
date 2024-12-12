using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;


namespace APPOINTMENTSCHEDULER
{
    // Appointment classes
    // Interface for appointments, defining core appointment methods
    public interface IAppointment
    {
        void Schedule(string patientName, DateTime date, string provider, string appointmentType, string status);
        void View();
        string GetPatientName();

        string GetDoctorName();
        DateTime GetDate();
        void SetDate(DateTime newDate);

        void SetStatus (string status);
        string GetStatus();
    }

    public abstract class BaseAppointment : IAppointment /// Represents a base class for different types of appointments.
    {
        public string PatientName { get; set; } /// Gets or sets the patient name.
        public DateTime Date { get; set; } /// Gets or sets the date.
        public string Provider { get; set; } /// Gets or sets the Healthcare Provider.
        public string AppointmentType { get; set; } /// Gets or sets the appointment type.
        public string Status { get; set; }

        public BaseAppointment(string patientName, DateTime date, string provider, string appointmentType, string status)
        {
            PatientName = patientName;
            Date = date;
            Provider = provider;
            AppointmentType = appointmentType;
            Status = status;
        }

        public abstract void Schedule(string patientName, DateTime date, string provider, string appointmentType, string status);

        public virtual void View()  /// Displays the appointment details.
        {
            Console.WriteLine($"[{Status}] Appointment: {PatientName} on {Date:g} with {Provider} ({AppointmentType})");
        }

        public string GetPatientName() => PatientName;

        public string GetDoctorName() => Provider;
        public DateTime GetDate() => Date;

        public void SetDate(DateTime newDate) => Date = newDate; ///defines a method that updates the appointment date to the value passed in as newDate

        public void SetStatus(string status) => Status = status;
        public string GetStatus() => Status;
    }

    public class MedicalAppointment : BaseAppointment /// Represents a medical appointment.
    {
        public MedicalAppointment(string patientName, DateTime date, string provider, string appointmentType, string status)
            : base(patientName, date, provider, appointmentType, status) { }

        public override void Schedule(string patientName, DateTime date, string provider, string appointmentType, string status)
        {
            Console.WriteLine($"[{status}] {appointmentType} appointment scheduled for {patientName} on {date:g} with {provider}.");
        }
    }

    // AppointmentScheduler class managing appointments and using FileHandler
    public class AppointmentScheduler /// Manages appointment scheduling, modification, cancellation, and viewing
    {
        private List<IAppointment> appointments = new List<IAppointment>();
        private FileHandler fileHandler;

        public AppointmentScheduler()
        {
            fileHandler = new FileHandler();
            LoadAppointmentsFromFile(); // Load appointments when the scheduler is created
        }

        public void ScheduleAppointment(IAppointment appointment, string patientName, DateTime date, string provider, string appointmentType, string status) /// Schedules a new appointment and saves it to file.
        {
            appointment.Schedule(patientName, date, provider, appointmentType, status);
            appointments.Add(appointment);
            fileHandler.SaveAppointmentsToFile(appointments); // Save to file after adding
        }

        public void CheckAppointmentReminders(string patientName, string status)
        {
            DateTime currentTime = DateTime.Now;
            foreach (var appointment in appointments)
            
            {

                // Check if the appointment is within the next 24 hours
                if (appointment.GetStatus() == status && appointment.GetDate() > currentTime && appointment.GetDate() <= currentTime.AddHours(24))
                {
                    Console.WriteLine($"Reminder: You have an appointment with {appointment.GetDoctorName()} at {appointment.GetDate():g}.");
                }

            }
        }

        public void CheckAppointmentReminders(string doctorName)
        {
            DateTime currentTime = DateTime.Now;
            foreach (var appointment in appointments)

            {

                // Check if the appointment is within the next 24 hours
                if (appointment.GetStatus() == "Approved" && appointment.GetDoctorName() == doctorName && appointment.GetDate() > currentTime && appointment.GetDate() <= currentTime.AddHours(24))
                {
                    Console.WriteLine($"Reminder: You have an appointment with {appointment.GetPatientName()} at {appointment.GetDate():g}.");
                }

            }
        }

            public void ModifyAppointment(string patientName, DateTime oldDate, DateTime newDate, string status)  /// Modifies an existing appointment's date.
            {
                var appointment = appointments.Find(a => a.GetPatientName() == patientName && a.GetDate() == oldDate);
                if (appointment != null)
                {
                    appointment.SetDate(newDate);
                    appointment.SetStatus(status);
                    Console.WriteLine($"Appointment for {patientName} has been modified to {newDate:g}.");
                    fileHandler.SaveAppointmentsToFile(appointments); // Save updated appointments to file
                }
                else
                {
                    Console.WriteLine("Appointment not found.");
                }
            }
        

        public bool CancelAppointment(string patientName, DateTime date) /// Cancels an appointment and removes it from the file.
        {
            var appointment = appointments.Find(a => a.GetPatientName() == patientName && a.GetDate() == date);
            try
            {


                if (appointment != null)
                {
                    appointments.Remove(appointment);
                    Console.WriteLine($"Appointment for {patientName} on {date:g} has been canceled.");
                    fileHandler.SaveAppointmentsToFile(appointments); // Save updated appointments to file
                    return true;
                }
                else
                {
                    Console.WriteLine("Appointment not found.");
                    return false;

                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"An error occurred while canceling appointments: {ex.Message}");
                return false;
            }

        }

        public bool ViewAppointments(string patientName) /// Displays all scheduled appointments.
        {
            bool found= false;
            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments scheduled.");
                return found;
            }
            else
            {
                Console.WriteLine("Scheduled Appointments:");
                foreach (var appointment in appointments)
                {
                    if (appointment.GetPatientName() == patientName && appointments.Count>0)
                    {
                        if (appointment.GetStatus() == "Pending" )
                        {
                            Console.WriteLine("\nPENDING APPOINTMENTS");
                            appointment.View();
                        }

                        if (appointment.GetStatus() == "Approved")
                        {
                            Console.WriteLine("\nAPPROVED APPOINTMENTS");
                            appointment.View();
                        }

                        if (appointment.GetStatus() == "Cancelled")
                        {
                            Console.WriteLine("\nCANCELLED APPOINTMENTS");
                            appointment.View();
                        }
                        found = true;
                    }

                 
                }
                return found;

                
            }
        }

        public bool ViewPendingAppointments(string doctorName)
        {
            bool found = false;
            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments scheduled.");
                return found;
            }
            else
            {
                Console.WriteLine("Scheduled Appointments:");

                foreach (var appointment in appointments)
                {
                    if (appointment.GetDoctorName() == doctorName && appointments.Count>0)
                    {
                        if (appointment.GetStatus() == "Pending")
                        {
                            Console.WriteLine("\nPENDING APPOINTMENTS");
                            appointment.View();

                        }
                        found= true;
                    }
                }
                return found;
            }
        }

        public bool ViewApprovedAppointment(string doctorName)
        {
            bool found = false;
            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments scheduled.");
                return found;
            }
            else
            {
                Console.WriteLine("Scheduled Appointments:");
                foreach (var appointment in appointments)
                {
                    if (appointment.GetDoctorName() == doctorName && appointments.Count>0)
                    {
                        if (appointment.GetStatus() == "Approved")
                        {
                            
                            appointment.View();
                        }
                        found= true;

                    }
                }
                return found;
            }
        }
        private void LoadAppointmentsFromFile() // Loads saved appointments from the file.
        {                                       //responsible for managing the appointments. It schedules, modifies, and manages the list of appointments in memory.
            var loadedAppointments = fileHandler.LoadAppointmentsFromFile();
            foreach (var appointment in loadedAppointments)
            {
                appointments.Add(appointment);
            }
        }

        public bool ApproveAppointment(string patientName,DateTime date, string doctorName, string status)  /// Modifies an existing appointment's date.
        {
            
            var appointment = appointments.Find(a => a.GetPatientName() == patientName && a.GetDate()==date && a.GetDoctorName() == doctorName && a.GetStatus() == "Pending");
            if (appointment != null)
            {
                appointment.SetStatus(status);
                Console.WriteLine($"Appointment for {patientName} has been modified to Approved.");
                fileHandler.SaveAppointmentsToFile(appointments); // Save updated appointments to file
                return true;
            }
            else
            {
                Console.WriteLine("Appointment not found.");
                return false;
            }
        }

        public bool CancelAppointment(string patientName, DateTime date, string doctorName, string status)  /// Modifies an existing appointment's date.
        {

            var appointment = appointments.Find(a => a.GetPatientName() == patientName && a.GetDate() == date && a.GetDoctorName() == doctorName && a.GetStatus() == "Pending");
            if (appointment != null)
            {
                appointment.SetStatus(status);
                Console.WriteLine($"Appointment for {patientName} has been cancelled.");
                fileHandler.SaveAppointmentsToFile(appointments); // Save updated appointments to file
                return true;
            }
            else
            {
                Console.WriteLine("Appointment not found.");
                return false;
            }
        }
        public void Stats(string doctorName)
        {
            while (true) // Loop to allow returning to the Doctor's Menu
            {
                // Step 1: Prompt user for the desired doctor
                

                // Step 2: Prompt user for the desired month
                int month = AnsiConsole.Prompt(
                    new TextPrompt<int>("Enter the [green]month (1-12)[/]:")
                        .PromptStyle("blue")
                        .Validate(month => month >= 1 && month <= 12 ? ValidationResult.Success() : ValidationResult.Error("[red]Invalid month! Please enter a value between 1 and 12.[/]")));

                // Step 3: Filter approved appointments for the selected doctor and month
                var approvedAppointments = appointments
                    .Where(appointment => appointment.GetDoctorName() == doctorName &&
                                          appointment.GetStatus() == "Approved" &&
                                          appointment.GetDate().Month == month)
                    .ToList();

                if (approvedAppointments.Count == 0)
                {
                    Console.WriteLine($"No approved appointments found for Dr. {doctorName} in month {month}.");
                }
                else
                {
                    // Step 4: Group appointments by week
                    int[] weeklyBookings = new int[4]; // Assuming 4 weeks in a month
                    foreach (var appointment in approvedAppointments)
                    {
                        int weekNumber = (appointment.GetDate().Day - 1) / 7; // Determine the week (0-indexed)
                        weeklyBookings[weekNumber]++;
                    }

                    // Step 5: Build and display the bar chart
                    var barChart = new BarChart()
                        .Width(60)
                        .Label($"[green bold underline]Weekly Approved Appointments for Dr. {doctorName} in Month {month}[/]")
                        .CenterLabel();

                    for (int i = 0; i < weeklyBookings.Length; i++)
                    {
                        barChart.AddItem($"Week {i + 1}", weeklyBookings[i], Color.Green); // Use Green for Approved appointments
                    }

                    AnsiConsole.Write(barChart);
                }

                break;
               
            }
            
        }





    }




    class Patient
    {
        public void Operations(string username)
        {
            AuthManager authManager = new AuthManager();
            AppointmentScheduler scheduler = new AppointmentScheduler();

            (string pfirstname, string plastname) = authManager.Name(username);
            string patientName = pfirstname + " " + plastname;

            string specialization;
            string chosenDoctor = null;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine("          Appointment Management         ");
                Console.WriteLine("=========================================");
                Console.WriteLine("1. Schedule Appointment");
                Console.WriteLine("2. Modify Appointment");
                Console.WriteLine("3. Cancel Appointment");
                Console.WriteLine("4. View Appointments");
                Console.WriteLine("5. View Upcoming Schedule");
                Console.WriteLine("6. Logout");
                Console.Write("Choose an option: ");
                string subOption = Console.ReadLine();

                switch (subOption)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("          Schedule Appointment           ");
                        Console.WriteLine("=========================================");
                        Console.WriteLine("Pediatrician");
                        Console.WriteLine("Surgeon");
                        Console.WriteLine("Gynecologist");
                        Console.WriteLine("Cardiologist");
                        Console.WriteLine("Hematologist");
                        Console.WriteLine("Dermatologist");

                        do
                        {
                            Console.Write("Enter Specialization: ");
                            specialization = Console.ReadLine().Trim().ToLower();
                        } while (specialization != "pediatrician" && specialization != "surgeon" && specialization != "gynecologist" && specialization != "cardiologist" && specialization != "hematologist" && specialization != "dermatologist");

                        Console.WriteLine("Available Doctors");
                        
                        while (!authManager.AvailableDoctors(specialization))
                        {
                            Console.Write("Enter Doctor ID [D00-000-000]: ");
                            chosenDoctor = Console.ReadLine();
                            if (authManager.CheckDoctor(chosenDoctor,specialization))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("No available doctor.");
                            }
                        }

                        Console.Write("Enter appointment date and time (yyyy-mm-dd hh:mm): ");
                        DateTime date;
                        while (!DateTime.TryParse(Console.ReadLine(), out date))
                        {
                            Console.Write("Invalid format. Please enter again (yyyy-mm-dd hh:mm): ");
                        }

                        (string dfirstname, string dlastname) = authManager.Name(chosenDoctor);
                        string provider = dfirstname + " " + dlastname;
                        IAppointment appointment;
                        appointment = new MedicalAppointment(patientName, date, provider, specialization,"Pending");

                        scheduler.ScheduleAppointment(appointment, patientName, date, provider, specialization, "Pending");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("          Modify Appointment             ");
                        Console.WriteLine("=========================================");

                        if (scheduler.ViewAppointments(patientName))
                        {

                            Console.Write("Enter old appointment date (yyyy-mm-dd hh:mm): ");
                            DateTime oldDate;
                            while (!DateTime.TryParse(Console.ReadLine(), out oldDate))
                            {
                                Console.Write("Invalid format. Please enter again (yyyy-mm-dd hh:mm): ");
                            }

                            Console.Write("Enter new appointment date (yyyy-mm-dd hh:mm): ");
                            DateTime newDate;
                            while (!DateTime.TryParse(Console.ReadLine(), out newDate))
                            {
                                Console.Write("Invalid format. Please enter again (yyyy-mm-dd hh:mm): ");
                            }

                            scheduler.ModifyAppointment(patientName, oldDate, newDate, "Pending");
                        }
                        else
                        {
                            Console.WriteLine("NO APPOINTMENTS");
                        }
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("          Cancel Appointment             ");
                        Console.WriteLine("=========================================");

                        if (scheduler.ViewAppointments(patientName))
                        {

                            Console.Write("Enter appointment date (yyyy-mm-dd hh:mm): ");
                            DateTime dateToCancel;
                            while (!DateTime.TryParse(Console.ReadLine(), out dateToCancel))
                            {
                                Console.Write("Invalid format. Please enter again (yyyy-mm-dd hh:mm): ");
                            }
                            Console.Write("Confirm appointment? (y/n): ");
                            string status = Console.ReadLine().Trim().ToLower();
                            if (status == "y")

                            {

                                if (scheduler.CancelAppointment(patientName, dateToCancel))
                                {
                                    Console.WriteLine($"Appointment for {patientName} Cancelled.");

                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("NO APPOINTMENTS");
                        }
                        
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("          View Appointments              ");
                        Console.WriteLine("=========================================");
                        scheduler.ViewAppointments(patientName);
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("        Upcoming Appointments            ");
                        Console.WriteLine("=========================================");
                        scheduler.CheckAppointmentReminders(patientName, "Approved");

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("               Logout                    ");
                        Console.WriteLine("=========================================");
                        Console.WriteLine("Logged out successfully.");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                }

                if (subOption == "6") return; // Exit to main menu
            }
        }
    }

   
    class Doctor
    {
        public void Operations(string username)
        {
            AuthManager authManager = new AuthManager();
            AppointmentScheduler scheduler = new AppointmentScheduler();

            (string dfirstname, string dlastname) = authManager.Name(username);
            string doctorName = dfirstname + " " + dlastname;

            string patientName = null;
            string status = null;

            while (true)
            {
                Console.Clear();
                
                Console.WriteLine("=========================================");
                Console.WriteLine("          Appointment Management         ");
                Console.WriteLine("=========================================");
                Console.WriteLine("1. Accept Appointment");  
                Console.WriteLine("2. Cancel Appointments");
                Console.WriteLine("3. View Appointments");
                Console.WriteLine("4. View Upcoming Schedule");
                Console.WriteLine("5. View Weekly Appointments");
                Console.WriteLine("6. Logout");
                Console.Write("Choose an option: ");
                string subOption = Console.ReadLine();

                switch (subOption)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("          Accept Appointment           ");
                        Console.WriteLine("=========================================");

                        if (scheduler.ViewPendingAppointments(doctorName))
                        {




                            Console.Write("Enter patient name: ");
                            patientName = Console.ReadLine();
                            Console.Write("Enter appointment date and time (yyyy-mm-dd hh:mm): ");
                            DateTime date;
                            while (!DateTime.TryParse(Console.ReadLine(), out date))
                            {
                                Console.Write("Invalid format. Please enter again (yyyy-mm-dd hh:mm): ");
                            }


                            Console.Write("Confirm appointment? (y/n): ");
                            status = Console.ReadLine().Trim().ToLower();
                            if (status == "y")

                            {

                                if (scheduler.ApproveAppointment(patientName, date, doctorName, "Approved"))
                                {
                                    Console.WriteLine($"Appointment for {patientName} approved.");
                                    break;
                                }




                            }
                        }
                        else
                        {
                            Console.WriteLine("NO PENDING APPOINTMENTS");
                        }
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("          CANCEL Appointment             ");
                        Console.WriteLine("=========================================");

                        if (scheduler.ViewPendingAppointments(doctorName))
                        {




                            Console.Write("Enter patient name: ");
                            patientName = Console.ReadLine();
                            Console.Write("Enter appointment date and time (yyyy-mm-dd hh:mm): ");
                            DateTime date;
                            while (!DateTime.TryParse(Console.ReadLine(), out date))
                            {
                                Console.Write("Invalid format. Please enter again (yyyy-mm-dd hh:mm): ");
                            }


                            Console.Write("Cancel appointment? (y/n): ");
                            status = Console.ReadLine().Trim().ToLower();
                            if (status == "y")

                            {

                                if (scheduler.CancelAppointment(patientName, date, doctorName, "Cancelled"))
                                {
                                    Console.WriteLine($"Appointment for {patientName} cancelled.");
                                    break;
                                }




                            }
                        }
                        else
                        {
                            Console.WriteLine("NO PENDING APPOINTMENTS TO CANCEL.");
                        }
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;




                    case "3":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("          View Appointments              ");
                        Console.WriteLine("=========================================");
                        scheduler.ViewApprovedAppointment(doctorName);
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("        Upcoming Appointments            ");
                        Console.WriteLine("=========================================");
                        scheduler.CheckAppointmentReminders(doctorName);
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("          VIEW WEEKLY APPOINTMENTS       ");
                        Console.WriteLine("=========================================");
                        scheduler.Stats(doctorName);
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("=========================================");
                        Console.WriteLine("               Logout                    ");
                        Console.WriteLine("=========================================");
                        Console.WriteLine("Logged out successfully.");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                }

                if (subOption == "6") return; // Exit to main menu
            }
        }
    }
}
