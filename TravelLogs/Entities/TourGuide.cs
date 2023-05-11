namespace travel_logs_api.TravelLogs.Entities;

public class TourGuide
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Experience { get; set; }
    public string Availability { get; set; }
    public string ContactInfo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}