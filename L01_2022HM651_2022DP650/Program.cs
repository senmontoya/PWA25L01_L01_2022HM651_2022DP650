using L01_2022HM651_2022DP650.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<restauranteContext>(options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("restauranteDB")
    )
);

//Inyeccion Dev
builder.Services.AddDbContext<restauranteContext>(options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("restauranteDamian")
    )
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
