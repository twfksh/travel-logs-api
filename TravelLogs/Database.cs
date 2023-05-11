using MySqlConnector;
using travel_logs_api.TravelLogs.Entities;

namespace travel_logs_api.TravelLogs;

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
    public async Task<User?> GetUser(int id)
    {
        var query = $"SELECT * FROM users WHERE id = {id}";
        await using var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", id);

        await using var reader = await ExecuteReader(command);
        if (!await reader.ReadAsync())
        {
            return null;
        }

        var user = new User
        {
            Id = reader["id"].GetInt32(),
            Name = reader["name"].GetString(),
            Email = reader["email"].GetString(),
            Password = reader["password"].GetString(),
            Role = reader["role"].GetString(),
            CreatedAt = reader["created_at"].GetDateTime(),
            UpdatedAt = reader["updated_at"].GetDateTime()
        };

        return user;
    }

    // retrieve users
    public async Task<IEnumerable<User>> GetUsers()
    {
        var query = "SELECT * FROM users";
        await using var command = await CreateCommand(query);

        await using var reader = await ExecuteReader(command);
        var users = new List<User>();

        while (await reader.ReadAsync())
        {
            var user = new User
            {
                Id = reader["id"].GetInt32(),
                Name = reader["name"].GetString(),
                Email = reader["email"].GetString(),
                Password = reader["password"].GetString(),
                Role = reader["role"].GetString(),
                CreatedAt = reader["created_at"].GetDateTime(),
                UpdatedAt = reader["updated_at"].GetDateTime()
            };
            users.Add(user);
        }

        return users;
    }

    // create user
    public async Task<int> CreateUser(User user)
    {
        var query = "INSERT INTO users (name, email, password, role, created_at, updated_at) VALUES (@name, @email, "
                    + "@password, @role, @created_at, @updated_at)";
        await using var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@password", user.Password);
        command.Parameters.AddWithValue("@role", user.Role);
        command.Parameters.AddWithValue("@created_at", user.CreatedAt);
        command.Parameters.AddWithValue("@updated_at", user.UpdatedAt);

        return await ExecuteNonQuery(command);
    }

    // update user
    public async Task<int> UpdateUser(User user)
    {
        var query = "UPDATE users SET name = @name, email = @email,"
                    + " password = @password, role = @role, updated_at = @updated_at WHERE id = @id";
        await using var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", user.Id);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@password", user.Password);
        command.Parameters.AddWithValue("@role", user.Role);
        command.Parameters.AddWithValue("@updated_at", user.UpdatedAt);

        return await ExecuteNonQuery(command);
    }

    // delete user
    public async Task<int> DeleteUser(int id)
    {
        var query = $"DELETE FROM users WHERE id = {id}";
        await using var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", id);

        return await ExecuteNonQuery(command);
    }
    
    // retrieve tour details by id
    public async Task<Tour?> GetTour(int id)
    {
        var query = $"SELECT * FROM tours WHERE id = {id}";
        await using var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", id);

        await using var reader = await ExecuteReader(command);
        if (!await reader.ReadAsync())
        {
            return null;
        }

        var tour = new Tour
        {
            Id = reader["id"].GetInt32(),
            Name = reader["name"].GetString(),
            Destination = reader["destination"].GetString(),
            Date = reader["date"].GetDateTime(),
            Activities = reader["activities"].GetString(),
            Price = reader["price"].GetString(),
            Availability = reader["availability"].GetString(),
            CreatedAt = reader["created_at"].GetDateTime(),
            UpdatedAt = reader["updated_at"].GetDateTime()
        };

        return tour;
    }

    // retrieve tours details
    public async Task<IEnumerable<Tour>> GetTours()
    {
        var query = "SELECT * FROM tours";
        await using var command = await CreateCommand(query);

        await using var reader = await ExecuteReader(command);
        var tours = new List<Tour>();

        while (await reader.ReadAsync())
        {
            var tour = new Tour
            {
                Id = reader["id"].GetInt32(),
                Name = reader["name"].GetString(),
                Destination = reader["destination"].GetString(),
                Date = reader["date"].GetDateTime(),
                Activities = reader["activities"].GetString(),
                Price = reader["price"].GetString(),
                Availability = reader["availability"].GetString(),
                CreatedAt = reader["created_at"].GetDateTime(),
                UpdatedAt = reader["updated_at"].GetDateTime()
            };
            tours.Add(tour);
        }

        return tours;
    }

    // create tour
    public async Task<int> CreateTour(Tour tour)
    {
        var query =
            "INSERT INTO tours (name, destination, date, activities, price, availability, created_at, updated_at) VALUES (@name, @destination, "
            + "@date, @activities, @price, @availability, @created_at, @updated_at)";
        await using var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@name", tour.Name);
        command.Parameters.AddWithValue("@destination", tour.Destination);
        command.Parameters.AddWithValue("@date", tour.Date);
        command.Parameters.AddWithValue("@activities", tour.Activities);
        command.Parameters.AddWithValue("@price", tour.Price);
        command.Parameters.AddWithValue("@availability", tour.Availability);
        command.Parameters.AddWithValue("@created_at", tour.CreatedAt);
        command.Parameters.AddWithValue("@updated_at", tour.UpdatedAt);

        return await ExecuteNonQuery(command);
    }

    // update tour
    public async Task<int> UpdateTour(Tour tour)
    {
        var query = "UPDATE tours SET name = @name, destination = @destination,"
                    + " date = @date, activities = @activities, price = @price, availability = @availability, updated_at = @updated_at WHERE id = @id";
        var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", tour.Id);
        command.Parameters.AddWithValue("@name", tour.Name);
        command.Parameters.AddWithValue("@destination", tour.Destination);
        command.Parameters.AddWithValue("@date", tour.Date);
        command.Parameters.AddWithValue("@activities", tour.Activities);
        command.Parameters.AddWithValue("@price", tour.Price);
        command.Parameters.AddWithValue("@availability", tour.Availability);
        command.Parameters.AddWithValue("@updated_at", tour.UpdatedAt);

        return await ExecuteNonQuery(command);
    }

    // delete tour
    public async Task<int> DeleteTour(int id)
    {
        var query = $"DELETE FROM tours WHERE id = {id}";
        await using var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", id);

        return await ExecuteNonQuery(command);
    }
    
    // retrieve tour_guide details by id
    public async Task<TourGuide?> GetTourGuide(int id)
    {
        var query = $"SELECT * FROM tour_guides WHERE id = {id}";
        await using var command = await CreateCommand(query);
        
        await using var reader = await ExecuteReader(command);
        if (!await reader.ReadAsync())
        {
            return null;
        }
        
        var tourGuide = new TourGuide
        {
            Id = reader["id"].GetInt32(),
            Name = reader["name"].GetString(),
            Experience = reader["experience"].GetString(),
            Availability = reader["availability"].GetString(),
            ContactInfo = reader["contact_info"].GetString(),
            CreatedAt = reader["created_at"].GetDateTime(),
            UpdatedAt = reader["updated_at"].GetDateTime()
        };
        
        return tourGuide;
    }
    
    // retrieve tour_guides details
    public async Task<IEnumerable<TourGuide>> GetTourGuides()
    {
        var query = "SELECT * FROM tour_guides";
        await using var command = await CreateCommand(query);
        
        await using var reader = await ExecuteReader(command);
        var tourGuides = new List<TourGuide>();
        
        while (await reader.ReadAsync())
        {
            var tourGuide = new TourGuide
            {
                Id = reader["id"].GetInt32(),
                Name = reader["name"].GetString(),
                Experience = reader["experience"].GetString(),
                Availability = reader["availability"].GetString(),
                ContactInfo = reader["contact_info"].GetString(),
                CreatedAt = reader["created_at"].GetDateTime(),
                UpdatedAt = reader["updated_at"].GetDateTime()
            };
            tourGuides.Add(tourGuide);
        }
        
        return tourGuides;
    }

    // create tour_guide
    public async Task<int> CreateTourGuide(TourGuide tourGuide)
    {
        var query =
            "INSERT INTO tour_guides (id, name, experience, availability, contact_info, created_at, updated_at) VALUES (@id, @name, @experience, @availability, @contact_info, @created_at, @updated_at)";
        var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", tourGuide.Id);
        command.Parameters.AddWithValue("@name", tourGuide.Name);
        command.Parameters.AddWithValue("@experience", tourGuide.Experience);
        command.Parameters.AddWithValue("@availability", tourGuide.Availability);
        command.Parameters.AddWithValue("@contact_info", tourGuide.ContactInfo);
        command.Parameters.AddWithValue("@created_at", tourGuide.CreatedAt);
        command.Parameters.AddWithValue("@updated_at", tourGuide.UpdatedAt);
        
        return await ExecuteNonQuery(command);
    }
    
    // update tour_guide
    public async Task<int> UpdateTourGuide(TourGuide tourGuide)
    {
        var query = "UPDATE tour_guides SET name = @name, experience = @experience,"
                    + " availability = @availability, contact_info = @contact_info, updated_at = @updated_at WHERE id = @id";
        var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", tourGuide.Id);
        command.Parameters.AddWithValue("@name", tourGuide.Name);
        command.Parameters.AddWithValue("@experience", tourGuide.Experience);
        command.Parameters.AddWithValue("@availability", tourGuide.Availability);
        command.Parameters.AddWithValue("@contact_info", tourGuide.ContactInfo);
        command.Parameters.AddWithValue("@updated_at", tourGuide.UpdatedAt);
        
        return await ExecuteNonQuery(command);
    }
    
    // delete tour_guide
    public async Task<int> DeleteTourGuide(int id)
    {
        var query = $"DELETE FROM tour_guides WHERE id = {id}";
        await using var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", id);

        return await ExecuteNonQuery(command);
    }
    
    // retrieve booking details by id
    public async Task<Booking?> GetBooking(int id)
    {
        var query = $"SELECT * FROM bookings WHERE id = {id}";
        await using var command = await CreateCommand(query);
        
        await using var reader = await ExecuteReader(command);
        if (!await reader.ReadAsync())
        {
            return null;
        }
        
        var booking = new Booking
        {
            Id = reader["id"].GetInt32(),
            UserId = reader["user_id"].GetInt32(),
            TourId = reader["tour_id"].GetInt32(),
            TourGuideId = reader["tour_guide_id"].GetInt32(),
            NumberOfPeople = reader["number_of_people"].GetInt32(),
            Status = reader["status"].GetString(),
            CreatedAt = reader["created_at"].GetDateTime(),
            UpdatedAt = reader["updated_at"].GetDateTime()
        };
        
        return booking;
    }
    
    // retrieve bookings details
    public async Task<IEnumerable<Booking>> GetBookings()
    {
        var query = "SELECT * FROM bookings";
        await using var command = await CreateCommand(query);
        
        await using var reader = await ExecuteReader(command);
        var bookings = new List<Booking>();
        
        while (await reader.ReadAsync())
        {
            var booking = new Booking
            {
                Id = reader["id"].GetInt32(),
                UserId = reader["user_id"].GetInt32(),
                TourId = reader["tour_id"].GetInt32(),
                TourGuideId = reader["tour_guide_id"].GetInt32(),
                NumberOfPeople = reader["number_of_people"].GetInt32(),
                Status = reader["status"].GetString(),
                CreatedAt = reader["created_at"].GetDateTime(),
                UpdatedAt = reader["updated_at"].GetDateTime()
            };
            bookings.Add(booking);
        }
        
        return bookings;
    }
    
    // create booking
    public async Task<int> CreateBooking(Booking booking)
    {
        var query =
            "INSERT INTO bookings (id, user_id, tour_id, tour_guide_id, number_of_people, status, created_at, updated_at) VALUES (@id, @user_id, @tour_id, @tour_guide_id, @number_of_people, @status, @created_at, @updated_at)";
        var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", booking.Id);
        command.Parameters.AddWithValue("@user_id", booking.UserId);
        command.Parameters.AddWithValue("@tour_id", booking.TourId);
        command.Parameters.AddWithValue("@tour_guide_id", booking.TourGuideId);
        command.Parameters.AddWithValue("@number_of_people", booking.NumberOfPeople);
        command.Parameters.AddWithValue("@status", booking.Status);
        command.Parameters.AddWithValue("@created_at", booking.CreatedAt);
        command.Parameters.AddWithValue("@updated_at", booking.UpdatedAt);
        
        return await ExecuteNonQuery(command);
    }
    
    // update booking
    public async Task<int> UpdateBooking(Booking booking)
    {
        var query = "UPDATE bookings SET user_id = @user_id, tour_id = @tour_id,"
                    + " tour_guide_id = @tour_guide_id, number_of_people = @number_of_people, status = @status, updated_at = @updated_at WHERE id = @id";
        var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", booking.Id);
        command.Parameters.AddWithValue("@user_id", booking.UserId);
        command.Parameters.AddWithValue("@tour_id", booking.TourId);
        command.Parameters.AddWithValue("@tour_guide_id", booking.TourGuideId);
        command.Parameters.AddWithValue("@number_of_people", booking.NumberOfPeople);
        command.Parameters.AddWithValue("@status", booking.Status);
        command.Parameters.AddWithValue("@updated_at", booking.UpdatedAt);
        
        return await ExecuteNonQuery(command);
    }
    
    // delete booking
    public async Task<int> DeleteBooking(int id)
    {
        var query = $"DELETE FROM bookings WHERE id = {id}";
        await using var command = await CreateCommand(query);
        command.Parameters.AddWithValue("@id", id);
        
        return await ExecuteNonQuery(command);
    }
}