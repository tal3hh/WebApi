using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.DTOs.User;
using ServiceLayer.Services;
using ServiceLayer.Services.Interfaces;
using ServiceLayer.Mapping;
using ServiceLayer.Validations.FluentValidation.User;
using RepositoryLayer.UniteOfWork;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation();

#region Context
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration["ConnectionStrings:Mssql"]);
});
#endregion

#region FluentValidation
builder.Services.AddScoped<IValidator<UserDto>, UserDtoValdiation>();
builder.Services.AddScoped<IValidator<UserCreateDto>, UserCreateDtoValidation>();
builder.Services.AddScoped<IValidator<UserUpdateDto>, UserUpdateDtoValidation>();
#endregion

#region Services
builder.Services.AddScoped<IUserService, UserService>();
#endregion

#region AutoMapper
var configuration = new MapperConfiguration(x =>
{
    x.AddProfile(new AutoMapperProfile());
});
var mapper = configuration.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion

builder.Services.AddScoped<IUow, Uow>();

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
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();


//Endpoints 
app.MapControllers();

app.Run();
