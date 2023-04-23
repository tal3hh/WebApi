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
using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation();


#region Ientity
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredLength = 1;
    opt.Password.RequireDigit = false;

    opt.User.RequireUniqueEmail = true;

    opt.SignIn.RequireConfirmedEmail = true;
    opt.SignIn.RequireConfirmedAccount = false;

    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    opt.Lockout.MaxFailedAccessAttempts = 5;

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
#endregion

#region Configure
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.Cookie.Name = "AshionIdentity";
    opt.LoginPath = new PathString("/Account/Login");
    opt.AccessDeniedPath = new PathString("/Account/AccessDenied");

});
#endregion

#region JWTBearer
builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audince"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
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

#region Context
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration["ConnectionStrings:Mssql"]);
});
#endregion

builder.Services.AddTransient<ITokenService, TokenService>();
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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


//Endpoints 
app.MapControllers();

app.Run();
