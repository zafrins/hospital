namespace Hospital.ViewModels
{
    public class DoctorAppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PatientEmail { get; set; }
        public string Description { get; set; }
        public string AppointmentNumber { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
       
    }
}