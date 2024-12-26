
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure PostgreSQL connection
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
connectionString = connectionString?.Replace("postgres://", string.Empty);
var pgUserPass = connectionString.Split("@")[0];
var pgHostDb = connectionString.Split("@")[1];
var pgHost = pgHostDb.Split("/")[0];
var pgDb = pgHostDb.Split("/")[1];
var pgUser = pgUserPass.Split(":")[0];
var pgPass = pgUserPass.Split(":")[1];
var pgPort = pgHost.Split(":")[1];
connectionString = $"Host={pgHost.Split(':')[0]};Port={pgPort};Username={pgUser};Password={pgPass};Database={pgDb};SslMode=Require;TrustServerCertificate=True";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
