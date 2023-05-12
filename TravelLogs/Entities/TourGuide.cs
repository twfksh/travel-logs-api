namespace travel_logs_api.TravelLogs.Entities;

#nullable disable

public class TourGuide
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Experience { get; init; }
    public string Availability { get; init; }
    public string ContactInfo { get; init; }
    public string CreatedAt { get; init; }
    public string UpdatedAt { get; init; }
}