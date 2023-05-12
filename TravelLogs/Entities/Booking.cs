namespace travel_logs_api.TravelLogs.Entities;

#nullable disable

public class Booking
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int TourId { get; init; }
    public int TourGuideId { get; init; }
    public int NumberOfPeople { get; init; }
    public string Status { get; init; }
    public string CreatedAt { get; init; }
    public string UpdatedAt { get; init; }
}