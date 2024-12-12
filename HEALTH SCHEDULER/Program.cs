namespace APPOINTMENTSCHEDULER
{
    class Program
    {
        static void Main(string[] args)
        {
            DoctorOperation doctorOperation = new DoctorOperation();
            PatientOperation patientOperation = new PatientOperation();

            while (true)
            {
                Console.Clear(); // Clear the console for a fresh start
                Console.WriteLine("=========================================");
                Console.WriteLine(" Welcome to the Healthcare Appointment Scheduler! ");
                Console.WriteLine("=========================================");
                Console.WriteLine("1. Doctor");
                Console.WriteLine("2. Patient");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");
                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        doctorOperation.Menu();
                        break;
                    case 2:
                        patientOperation.Menu();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }

}
