using Business.Services;
using Data;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataServices();

builder.Services.AddGrpc();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ApplicationUserService>();

var jwtSettings = builder.Configuration.GetSection("JWTSettings");
string key = jwtSettings["Key"] ?? "defaultKeyValue";
string issuer = jwtSettings["Issuer"] ?? "defaultIssuer";
string audience = jwtSettings["Audience"] ?? "defaultAudience";
int durationInMinutes = jwtSettings.GetValue<int>("DurationInMinutes", 30);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Home/ViewLogin";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.Cookie.Name = "jwt";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapGrpcService<UserService>();
    endpoints.MapGrpcService<ApplicationUserService>();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=ViewLogin}/{id?}");
});

app.Run();
