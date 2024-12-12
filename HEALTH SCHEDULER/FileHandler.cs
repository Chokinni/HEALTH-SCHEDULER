using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace APPOINTMENTSCHEDULER
{
    // FileHandler class to handle saving appointments to file
    // FileHandler class to handle saving appointments to file
    public class FileHandler
    {
        //private const string directoryPath = @"C:\Users\user1\source\vsstudio\CPE261\AppointmentScheduler";
        private string filePath;
        const string directoryPath = @"D:\FILEHANDLINGuwu";

        public FileHandler() //store the file path provided when creating a FileHandler object
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
                filePath = Path.Combine(directoryPath, "APPOINTMENTS.txt");

                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath)) { }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"ERROR: File operation failed.");
            }
        }

        public void SaveAppointmentsToFile(List<IAppointment> appointments) // Saves a list of appointments to a file.
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var appointment in appointments)
                    {
                        if (appointment is BaseAppointment baseAppointment)
                        {
                            string data = $"{baseAppointment.PatientName},{baseAppointment.AppointmentType},{baseAppointment.Date:g},{baseAppointment.Provider},{baseAppointment.Status}";
                            writer.WriteLine(data);
                        }
                    }
                }
                Console.WriteLine("Appointments saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving to file: {ex.Message}");
            }
        }

        public List<IAppointment> LoadAppointmentsFromFile() // Loads appointments from a file.
                                                             // in charge of reading from the file and creating appointment objects.
        {
            List<IAppointment> loadedAppointments = new List<IAppointment>(); //This creates an empty list called loadedAppointments


            if (File.Exists(filePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length >= 4)
                            {
                                string patientName = parts[0];
                                string appointmentType = parts[1];
                                DateTime date = DateTime.Parse(parts[2]);
                                string provider = parts[3];
                                string status = parts[4];

                                IAppointment appointment;
                                appointment = new MedicalAppointment(patientName, date, provider, appointmentType, status);
                                loadedAppointments.Add(appointment);
                            }
                        }
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"An error occurred while loading appointments: {ex.Message}");
                }
            }
            return loadedAppointments;
        }


    }
}
