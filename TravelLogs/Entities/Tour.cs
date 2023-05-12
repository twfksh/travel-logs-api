namespace travel_logs_api.TravelLogs.Entities;

#nullable disable

public class Tour
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Destination { get; init; }
    public string Date { get; init; }
    public string Activities { get; init; }
    public decimal Price { get; init; }
    public int Capacity { get; init; }
    public string Availability { get; init; }
    public string CreatedAt { get; init; }
    public string UpdatedAt { get; init; }
}