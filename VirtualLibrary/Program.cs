
using VirtualLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using VirtualLibrary.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IBookBorrowingsService, BookBorrowingsService>();
builder.Services.AddScoped<IReviewService, ReviewService>();



builder.Services.AddCors(options =>{
        options.AddPolicy("corspolicy", policy =>{
            policy.WithOrigins(new []{"http://localhost:3000"}).AllowCredentials().AllowAnyMethod().AllowAnyHeader();
        });
});

builder.Services.AddDbContext<VirtualLibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VirtualLibraryContext") ?? throw new InvalidOperationException("Connection string 'VirtualLibraryContext' not found.")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{   
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();