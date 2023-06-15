using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied";
    opt.Cookie.SameSite = SameSiteMode.Strict;  //BU cookie sadece elaqeli domainde isliyir.
    opt.Cookie.HttpOnly = true;                 //Bu cookie'nin JSle paylasmasina imkan vermir. 
    opt.Cookie.Name = "WebApi";                 
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;    //Request ne ile gelse olar.(Mes: Http ile gelse Http, Https le gelse onla cvb verir.)
});

var app = builder.Build();


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
