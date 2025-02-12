using Microsoft.EntityFrameworkCore;
using Spg.Fachtheorie.Aufgabe2.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

string connectionString = "Data Source = C:\\Scratch\\Aufgabe3_RealApp.db";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Aufgabe2Database>(options => options.UseSqlite(connectionString));

// TODO: Services im DI-Container resgistrieren
// ...
builder.Services.AddScoped<ApplicationService>();


var app = builder.Build();


// ====================================================================
// Create DB Code First (don't touch) =================================
DbContextOptions options = new DbContextOptionsBuilder()
.UseSqlite(connectionString)
.Options;
Aufgabe2Database db = new Aufgabe2Database(options);
db.Database.EnsureDeleted();
db.Database.EnsureCreated();
db.Seed();
// Create DB Code First ===============================================
// ====================================================================


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program { }
