using Microsoft.AspNetCore.Identity;
using GamesLibrary.DataAccessLayer.Contacts;
using GamesLibrary.DataAccessLayer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using GamesLibrary.Services;
using GamesLibrary.DataAccessLayer.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json"); // Add this line to load appsettings.json

// Add services to the container.
builder.Services.AddDbContext<GamesLibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Customize the Identity options as needed
    options.User.AllowedUserNameCharacters = null;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<GamesLibraryDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOnly", policy =>
    {
        policy.RequireRole("Manager");
    });
});

builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PurchaseService>();

builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration")); // Register EmailConfiguration
builder.Services.AddTransient<IEmailSender, EmailSender>(); // Register EmailSender

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
