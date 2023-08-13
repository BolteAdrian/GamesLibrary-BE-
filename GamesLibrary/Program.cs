using Microsoft.AspNetCore.Identity;
using GamesLibrary.Repository.Contacts;
using GamesLibrary.Repository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using GamesLibrary.Services;
using GamesLibrary.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json"); // Add this line to load appsettings.json

// Add services to the container.
IServiceCollection serviceCollection = builder.Services.AddDbContext<GamesLibraryDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));   //use this for SQL connection
options.UseInMemoryDatabase("GamesLibraryInMemoryDatabase"));

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
builder.Services.AddScoped<ReviewService>();

builder.Services.Configure<EmailConfigurationDto>(builder.Configuration.GetSection("EmailConfiguration")); // Register EmailConfiguration
builder.Services.AddTransient<IEmailSenderDto, EmailSender>(); // Register EmailSender

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JwtOptionsDto>(builder.Configuration.GetSection("Jwt"));

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

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GamesLibraryDbContext>();
    dbContext.Database.EnsureCreated();
    DatabaseSeeder.SeedDatabase(dbContext);
}

app.Run();
