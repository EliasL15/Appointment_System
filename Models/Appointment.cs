using System.ComponentModel.DataAnnotations;

public class Appointment
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [MaxLength(250)]
    public string Location { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}