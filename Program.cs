using travel_logs_api.TravelLogs;
using travel_logs_api.TravelLogs.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database instance
var db = new Database("Server=localhost;Port=3306;Database=travel_logs;Uid=root;Pwd=6266");

// App instance
var app = builder.Build();

// API Endpoints

// users endpoints
// GET /users
app.MapGet("/users", async () =>
{
    var users = await db.GetUsers();

    return Results.Json(users);
}).WithTags("user");

// GET /users/{id}
app.MapGet("/users/{id}", async (int id) =>
{
    var user = await db.GetUser(id);
    if (user is null)
    {
        return Results.NotFound();
    }

    return Results.Json(user);
}).WithTags("user");

// POST /users
app.MapPost("/users", async (User user) =>
{
    await db.CreateUser(user);

    return Results.Created($"/users/{user.Id}", user);
}).WithTags("user");

// PUT /users/{id}
app.MapPut("/users/{id}", async (int id, User user) =>
{
    var existingUser = await db.GetUser(id);
    if (existingUser is null)
    {
        return Results.NotFound();
    }

    var updatedUser = new User
    {
        Id = existingUser.Id,
        Name = user.Name,
        Email = user.Email,
        Password = user.Password,
        Role = user.Role,
        CreatedAt = existingUser.CreatedAt,
        UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    };

    await db.UpdateUser(updatedUser);

    return Results.Ok("Successfully updated user");
}).WithTags("user");

// DELETE /users/{id}
app.MapDelete("/users/{id}", async (int id) =>
{
    var user = await db.GetUser(id);
    if (user is null)
    {
        return Results.NotFound();
    }

    await db.DeleteUser(id);

    return Results.Ok("Successfully deleted user");
}).WithTags("user");

// tours endpoints
// GET /tours
app.MapGet("/tours", async () =>
{
    var tours = await db.GetTours();
    return Results.Json(tours);
}).WithTags("tour");

// GET /tours/{id}
app.MapGet("/tours/{id}", async (int id) =>
{
    var tour = await db.GetTour(id);
    if (tour is null)
    {
        return Results.NotFound();
    }

    return Results.Json(tour);
}).WithTags("tour");

// POST /tours
app.MapPost("/tours", async (Tour tour) =>
{
    await db.CreateTour(tour);

    return Results.Created($"/tours/{tour.Id}", tour);
}).WithTags("tour");

// PUT /tours/{id}
app.MapPut("/tours/{id}", async (int id, Tour tour) =>
{
    var existingTour = await db.GetTour(id);
    if (existingTour is null)
    {
        return Results.NotFound();
    }

    var updatedTour = new Tour
    {
        Id = existingTour.Id,
        Name = tour.Name,
        Destination = tour.Destination,
        Date = tour.Date,
        Activities = tour.Activities,
        Price = tour.Price,
        Capacity = tour.Capacity,
        Availability = tour.Availability,
        CreatedAt = existingTour.CreatedAt,
        UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    };

    await db.UpdateTour(updatedTour);

    return Results.Ok("Successfully updated tour");
}).WithTags("tour");

// DELETE /tours/{id}
app.MapDelete("/tours/{id}", async (int id) =>
{
    var tour = await db.GetTour(id);
    if (tour is null)
    {
        return Results.NotFound();
    }

    await db.DeleteTour(id);

    return Results.Ok("Successfully deleted tour");
}).WithTags("tour");

// tour_guides endpoints
// GET /tour_guides
app.MapGet("/tour_guides", async () =>
{
    var tourGuides = await db.GetTourGuides();

    return Results.Json(tourGuides);
}).WithTags("tour_guide");

// GET /tour_guides/{id}
app.MapGet("/tour_guides/{id}", async (int id) =>
{
    var tourGuide = await db.GetTourGuide(id);
    if (tourGuide is null)
    {
        return Results.NotFound();
    }

    return Results.Json(tourGuide);
}).WithTags("tour_guide");

// POST /tour_guides
app.MapPost("/tour_guides", async (TourGuide tourGuide) =>
{
    await db.CreateTourGuide(tourGuide);

    return Results.Created($"/tour_guides/{tourGuide.Id}", tourGuide);
}).WithTags("tour_guide");

// PUT /tour_guides/{id}
app.MapPut("/tour_guides/{id}", async (int id, TourGuide tourGuide) =>
{
    var existingTourGuide = await db.GetTourGuide(id);
    if (existingTourGuide is null)
    {
        return Results.NotFound();
    }

    var updatedTourGuide = new TourGuide
    {
        Id = existingTourGuide.Id,
        Name = tourGuide.Name,
        Experience = tourGuide.Experience,
        Availability = tourGuide.Availability,
        ContactInfo = tourGuide.ContactInfo,
        CreatedAt = existingTourGuide.CreatedAt,
        UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    };

    await db.UpdateTourGuide(updatedTourGuide);

    return Results.Ok("Successfully updated tour guide");
}).WithTags("tour_guide");

// DELETE /tour_guides/{id}
app.MapDelete("/tour_guides/{id}", async (int id) =>
{
    var tourGuide = await db.GetTourGuide(id);
    if (tourGuide is null)
    {
        return Results.NotFound();
    }

    await db.DeleteTourGuide(id);

    return Results.Ok("Successfully deleted tour guide");
}).WithTags("tour_guide");

// bookings endpoints
// GET /bookings
app.MapGet("/bookings", async () =>
{
    var bookings = await db.GetBookings();

    return Results.Json(bookings);
}).WithTags("booking");

// GET /bookings/{id}
app.MapGet("/bookings/{id}", async (int id) =>
{
    var booking = await db.GetBooking(id);
    if (booking is null)
    {
        return Results.NotFound();
    }

    return Results.Json(booking);
}).WithTags("booking");

// POST /bookings
app.MapPost("/bookings", async (Booking booking) =>
{
    await db.CreateBooking(booking);

    return Results.Created($"/bookings/{booking.Id}", booking);
}).WithTags("booking");

// PUT /bookings/{id}
app.MapPut("/bookings/{id}", async (int id, Booking booking) =>
{
    var existingBooking = await db.GetBooking(id);
    if (existingBooking is null)
    {
        return Results.NotFound();
    }

    var updatedBooking = new Booking
    {
        Id = existingBooking.Id,
        UserId = booking.UserId,
        TourGuideId = booking.TourGuideId,
        TourId = booking.TourId,
        CreatedAt = existingBooking.CreatedAt,
        UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    };

    await db.UpdateBooking(updatedBooking);

    return Results.Ok("Successfully updated booking");
}).WithTags("booking");

// DELETE /bookings/{id}
app.MapDelete("/bookings/{id}", async (int id) =>
{
    var booking = await db.GetBooking(id);
    if (booking is null)
    {
        return Results.NotFound();
    }

    await db.DeleteBooking(id);

    return Results.Ok("Successfully deleted booking");
}).WithTags("booking");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();