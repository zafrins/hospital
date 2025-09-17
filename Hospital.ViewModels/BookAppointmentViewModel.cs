using System.ComponentModel.DataAnnotations;

public class BookAppointmentViewModel
{
    public string DoctorId { get; set; }

    public string DoctorName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public string Reason { get; set; }
}
