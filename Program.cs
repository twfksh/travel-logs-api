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