using Microsoft.EntityFrameworkCore;
using ProjectLab.Models;
using ProjectLab.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LabContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(option => option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//LabContext lc = new LabContext();
//var sf = new ScriptFile(lc);
//var azureData = sf.ReadAzureData("piyush@gmail.com");

//foreach (var resource in azureData)
//{
//    Console.WriteLine($"Type: {resource.Type}, Location: {resource.Location}, Name: {resource.Name}");
//}

app.Run();
