using Hangfire;
using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.Repository;
using System.Web.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository, EfRepository>();


var connetionString = builder.Configuration.GetConnectionString("Defaultsql");
builder.Services.AddDbContext<RuxGymDBcontext>(options =>
{
    options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString));
    //options.UseSqlServer(connetionString);
    options.EnableSensitiveDataLogging(true);
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
