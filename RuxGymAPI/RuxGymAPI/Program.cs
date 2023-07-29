using Hangfire;
using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.Repository;
using RuxGymAPI.Repository.Tournament;
using System.Configuration;
using System.Web.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository, EfRepository>();
builder.Services.AddScoped<ITournament, EfTournament>();


var connetionString = builder.Configuration.GetConnectionString("Defaultsql");
builder.Services.AddDbContext<RuxGymDBcontext>(options =>
{
    options.UseMySql(connetionString, new MySqlServerVersion(new Version(8, 0, 26)));
    options.EnableSensitiveDataLogging(true);
    
    // Diðer yapýlandýrmalar
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
