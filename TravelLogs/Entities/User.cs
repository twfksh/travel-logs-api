namespace travel_logs_api.TravelLogs.Entities;

#nullable disable

public class User
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string Role { get; init; } 
    public string CreatedAt { get; init; }
    public string UpdatedAt { get; init; }
}
