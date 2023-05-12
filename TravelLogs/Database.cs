using System.Globalization;

namespace travel_logs_api.TravelLogs;

#nullable disable

using MySqlConnector;
using Entities;

public class Database
{
    private readonly string _connectionString;

    public Database(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    private async Task<MySqlConnection> GetConnection()
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    private async Task<MySqlCommand> CreateCommand(string query)
    {
        await using var connection = await GetConnection();
        var command = new MySqlCommand(query, connection);
        return command;
    }

    private async Task<MySqlDataReader> ExecuteReader(MySqlCommand command)
    {
        await using var reader = await command.ExecuteReaderAsync();
        return reader;
    }

    private async Task<int> ExecuteNonQuery(MySqlCommand command)
    {
        return await command.ExecuteNonQueryAsync();
    }

    // retrieve user by id
    public async Task<User> GetUser(int id)
    {
        var query = $"SELECT * FROM users WHERE id = {id}";

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        var user = new User
        {
            Id = reader["id"].GetHashCode(),
            Name = reader["name"].ToString(),
            Email = reader["email"].ToString(),
            Password = reader["password"].ToString(),
            Role = reader["role"].ToString(),
            CreatedAt = reader["created_at"].ToString(),
            UpdatedAt = reader["updated_at"].ToString()
        };
            
        return user;
    }

    // retrieve users
    public async Task<IEnumerable<User>> GetUsers()
    {
        var query = "SELECT * FROM users";
        
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        var reader = await command.ExecuteReaderAsync();
        var users = new List<User>();
        
        while (await reader.ReadAsync())
        {
            var user = new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                Role = reader.GetString(4),
                CreatedAt = reader.GetDateTime(5).ToString(CultureInfo.InvariantCulture),
                UpdatedAt = reader.GetDateTime(6).ToString(CultureInfo.InvariantCulture)
            };
            users.Add(user);
        }

        return users;
    }

    // create user
    public async Task<int> CreateUser(User user)
    {
        var query = "INSERT INTO users (name, email, password, role, updated_at) VALUES (@name, @email, "
                    + "@password, @role, @updated_at)";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@password", user.Password);
        command.Parameters.AddWithValue("@role", user.Role);
        command.Parameters.AddWithValue("@updated_at", DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"));

        return await command.ExecuteNonQueryAsync();
    }

    // update user
    public async Task<int> UpdateUser(User user)
    {
        var query = "UPDATE users SET ";
        var parameters = new List<MySqlParameter>();

        // Add parameters for the columns that are being updated.
        if (user.Name != null) {
            query += "name = @name, ";
            parameters.Add(new MySqlParameter("@name", user.Name));
        }
        if (user.Email != null) {
            query += "email = @email, ";
            parameters.Add(new MySqlParameter("@email", user.Email));
        }
        if (user.Password != null) {
            query += "password = @password, ";
            parameters.Add(new MySqlParameter("@password", user.Password));
        }
        if (user.Role != null) {
            query += "role = @role, ";
            parameters.Add(new MySqlParameter("@role", user.Role));
        }
        if (user.UpdatedAt != null) {
            query += "updated_at = @updated_at, ";
            parameters.Add(new MySqlParameter("@updated_at", user.UpdatedAt));
        }

        // Remove the trailing comma.
        query = query.Substring(0, query.Length - 2);

        // Add the WHERE clause.
        query += " WHERE id = @id";
        parameters.Add(new MySqlParameter("@id", user.Id));

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
    
        await using var command = new MySqlCommand(query, connection);
        foreach (var parameter in parameters) {
            command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
        }

        return await command.ExecuteNonQueryAsync();
    }

    // delete user
    public async Task<int> DeleteUser(int id)
    {
        var query = $"DELETE FROM users WHERE id = {id}";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        return await command.ExecuteNonQueryAsync();
    }
    
    // retrieve tour details by id
    public async Task<Tour> GetTour(int id)
    {
        var query = $"SELECT * FROM tours WHERE id = {id}";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        var tour = new Tour
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Destination = reader.GetString(2),
            Date = reader.GetDateTime(3).ToString("yyyy-mm-dd hh:mm:ss"),
            Activities = reader.GetString(4),
            Price = reader.GetDecimal(5),
            Availability = reader.GetString(6),
            Capacity = reader.GetInt32(9),
            CreatedAt = reader.GetDateTime(7).ToString("yyyy-mm-dd hh:mm:ss"),
            UpdatedAt = reader.GetDateTime(8).ToString("yyyy-mm-dd hh:mm:ss")
        };

        return tour;
    }

    // retrieve tours details
    public async Task<IEnumerable<Tour>> GetTours()
    {
        var query = "SELECT * FROM tours";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        var reader = await command.ExecuteReaderAsync();
        var tours = new List<Tour>();

        while (await reader.ReadAsync())
        {
            var tour = new Tour
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Destination = reader.GetString(2),
                Date = reader.GetDateTime(3).ToString("yyyy-mm-dd hh:mm:ss"),
                Activities = reader.GetString(4),
                Price = reader.GetDecimal(5),
                Availability = reader.GetString(6),
                Capacity = reader.GetInt32(9),
                CreatedAt = reader.GetDateTime(7).ToString("yyyy-mm-dd hh:mm:ss"),
                UpdatedAt = reader.GetDateTime(8).ToString("yyyy-mm-dd hh:mm:ss")
            };
            tours.Add(tour);
        }

        return tours;
    }

    // create tour
    public async Task<int> CreateTour(Tour tour)
    {
        var query =
            "INSERT INTO tours (name, destination, date, activities, price, availability, capacity, updated_at) VALUES (@name, @destination, "
            + "@date, @activities, @price, @availability, @capacity, @updated_at)";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", tour.Name);
        command.Parameters.AddWithValue("@destination", tour.Destination);
        command.Parameters.AddWithValue("@date", tour.Date);
        command.Parameters.AddWithValue("@activities", tour.Activities);
        command.Parameters.AddWithValue("@price", tour.Price);
        command.Parameters.AddWithValue("@availability", tour.Availability);
        command.Parameters.AddWithValue("@capacity", tour.Capacity);
        command.Parameters.AddWithValue("@updated_at", DateTime.Now);

        return await command.ExecuteNonQueryAsync();
    }

    // update tour
    public async Task<int> UpdateTour(Tour tour)
    {
        var query = "UPDATE tours SET ";
        var parameters = new List<MySqlParameter>();

        if (tour.Name != null)
        {
            query += "name = @name, ";
            parameters.Add(new MySqlParameter("@name", tour.Name));
        }
        if (tour.Destination != null)
        {
            query += "destination = @destination, ";
            parameters.Add(new MySqlParameter("@destination", tour.Destination));
        }
        if (tour.Date != null)
        {
            query += "date = @date, ";
            parameters.Add(new MySqlParameter("@date", tour.Date));
        }
        if (tour.Activities != null)
        {
            query += "activities = @activities, ";
            parameters.Add(new MySqlParameter("@activities", tour.Activities));
        }
        if (tour.Price != 0)
        {
            query += "price = @price, ";
            parameters.Add(new MySqlParameter("@price", tour.Price));
        }
        if (tour.Availability != null)
        {
            query += "availability = @availability, ";
            parameters.Add(new MySqlParameter("@availability", tour.Availability));
        }
        if (tour.Capacity != 0)
        {
            query += "capacity = @capacity, ";
            parameters.Add(new MySqlParameter("@capacity", tour.Capacity));
        }
        query += "updated_at = @updated_at";
        parameters.Add(new MySqlParameter("@updated_at", DateTime.Now));

        // Add the WHERE clause.
        query += " WHERE id = @id";
        parameters.Add(new MySqlParameter("@id", tour.Id));

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new MySqlCommand(query, connection);
        foreach (var parameter in parameters)
        {
            command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
        }

        return await command.ExecuteNonQueryAsync();
    }

    // delete tour
    public async Task<int> DeleteTour(int id)
    {
        var query = $"DELETE FROM tours WHERE id = {id}";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        return await command.ExecuteNonQueryAsync();
    }
    
    // retrieve tour_guide details by id
    public async Task<TourGuide> GetTourGuide(int id)
    {
        var query = $"SELECT * FROM tour_guides WHERE id = {id}";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }
        
        var tourGuide = new TourGuide
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Experience = reader.GetString(2),
            Availability = reader.GetString(3),
            ContactInfo = reader.GetString(4),
            CreatedAt = reader.GetDateTime(5).ToString("yyyy-mm-dd hh:mm:ss"),
            UpdatedAt = reader.GetDateTime(6).ToString("yyyy-mm-dd hh:mm:ss")
        };
        
        return tourGuide;
    }
    
    // retrieve tour_guides details
    public async Task<IEnumerable<TourGuide>> GetTourGuides()
    {
        var query = "SELECT * FROM tour_guides";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();
        var tourGuides = new List<TourGuide>();
        
        while (await reader.ReadAsync())
        {
            var tourGuide = new TourGuide
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Experience = reader.GetString(2),
                Availability = reader.GetString(3),
                ContactInfo = reader.GetString(4),
                CreatedAt = reader.GetDateTime(5).ToString("yyyy-mm-dd hh:mm:ss"),
                UpdatedAt = reader.GetDateTime(6).ToString("yyyy-mm-dd hh:mm:ss")
            };
            tourGuides.Add(tourGuide);
        }
        
        return tourGuides;
    }

    // create tour_guide
    public async Task<int> CreateTourGuide(TourGuide tourGuide)
    {
        var query =
            "INSERT INTO tour_guides (name, experience, availability, contact_info, updated_at) VALUES (@name, @experience, @availability, @contact_info, @updated_at)";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", tourGuide.Name);
        command.Parameters.AddWithValue("@experience", tourGuide.Experience);
        command.Parameters.AddWithValue("@availability", tourGuide.Availability);
        command.Parameters.AddWithValue("@contact_info", tourGuide.ContactInfo);
        command.Parameters.AddWithValue("@updated_at", DateTime.Now);

        return await command.ExecuteNonQueryAsync();
    }
    
    // update tour_guide
    public async Task<int> UpdateTourGuide(TourGuide tourGuide)
    {
        var query = "UPDATE tour_guides SET name = @name, experience = @experience,"
                    + " availability = @availability, contact_info = @contact_info, updated_at = @updated_at WHERE id = @id";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", tourGuide.Name);
        command.Parameters.AddWithValue("@experience", tourGuide.Experience);
        command.Parameters.AddWithValue("@availability", tourGuide.Availability);
        command.Parameters.AddWithValue("@contact_info", tourGuide.ContactInfo);
        command.Parameters.AddWithValue("@updated_at", DateTime.Now);

        return await command.ExecuteNonQueryAsync();
    }
    
    // delete tour_guide
    public async Task<int> DeleteTourGuide(int id)
    {
        var query = $"DELETE FROM tour_guides WHERE id = {id}";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        return await command.ExecuteNonQueryAsync();
    }
    
    // retrieve booking details by id
    public async Task<Booking> GetBooking(int id)
    {
        var query = $"SELECT * FROM bookings WHERE id = {id}";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        
        await using var command = new MySqlCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }
        
        var booking = new Booking
        {
            Id = reader.GetInt32(0),
            UserId = reader.GetInt32(1),
            TourId = reader.GetInt32(2),
            TourGuideId = reader.GetInt32(8),
            NumberOfPeople = reader.GetInt32(4),
            Status = reader.GetString(5),
            Date = reader.GetDateTime(3).ToString("yyyy-mm-dd hh:mm:ss"),
            CreatedAt = reader.GetDateTime(6).ToString("yyyy-mm-dd hh:mm:ss"),
            UpdatedAt = reader.GetDateTime(7).ToString("yyyy-mm-dd hh:mm:ss")
        };
        
        return booking;
    }
    
    // retrieve bookings details
    public async Task<IEnumerable<Booking>> GetBookings()
    {
        var query = "SELECT * FROM bookings";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        
        await using var command = new MySqlCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();
        var bookings = new List<Booking>();
        
        while (await reader.ReadAsync())
        {
            var booking = new Booking
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                TourId = reader.GetInt32(2),
                TourGuideId = reader.GetInt32(8),
                NumberOfPeople = reader.GetInt32(4),
                Status = reader.GetString(5),
                Date = reader.GetDateTime(3).ToString("yyyy-mm-dd hh:mm:ss"),
                CreatedAt = reader.GetDateTime(6).ToString("yyyy-mm-dd hh:mm:ss"),
                UpdatedAt = reader.GetDateTime(7).ToString("yyyy-mm-dd hh:mm:ss")
            };
            bookings.Add(booking);
        }
        
        return bookings;
    }
    
    // create booking
    public async Task<int> CreateBooking(Booking booking)
    {
        var query =
            "INSERT INTO bookings (user_id, tour_id, tour_guide_id, number_of_people, status, updated_at) VALUES (@user_id, @tour_id, @tour_guide_id, @number_of_people, @status, @updated_at)";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@user_id", booking.UserId);
        command.Parameters.AddWithValue("@tour_id", booking.TourId);
        command.Parameters.AddWithValue("@tour_guide_id", booking.TourGuideId);
        command.Parameters.AddWithValue("@number_of_people", booking.NumberOfPeople);
        command.Parameters.AddWithValue("@status", booking.Status);
        command.Parameters.AddWithValue("@updated_at", DateTime.Now);

        return await ExecuteNonQuery(command);
    }
    
    // update booking
    public async Task<int> UpdateBooking(Booking booking)
    {
        var query = "UPDATE bookings SET user_id = @user_id, tour_id = @tour_id,"
                    + " tour_guide_id = @tour_guide_id, number_of_people = @number_of_people, status = @status WHERE id = @id";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@user_id", booking.UserId);
        command.Parameters.AddWithValue("@tour_id", booking.TourId);
        command.Parameters.AddWithValue("@tour_guide_id", booking.TourGuideId);
        command.Parameters.AddWithValue("@number_of_people", booking.NumberOfPeople);
        command.Parameters.AddWithValue("@status", booking.Status);

        return await ExecuteNonQuery(command);
    }
    
    // delete booking
    public async Task<int> DeleteBooking(int id)
    {
        var query = $"DELETE FROM bookings WHERE id = {id}";
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        
        return await ExecuteNonQuery(command);
    }
}