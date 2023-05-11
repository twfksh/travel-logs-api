namespace travel_logs_api.TravelLogs.Entities;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TourId { get; set; }
    public int TourGuideId { get; set; }
    public int NumberOfPeople { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}