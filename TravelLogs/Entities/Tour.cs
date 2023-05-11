namespace travel_logs_api.TravelLogs.Entities;

public class Tour
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Destination { get; set; }
    public DateTime Date { get; set; }
    public string Activities { get; set; }
    public string Price { get; set; }
    public string Availability { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}