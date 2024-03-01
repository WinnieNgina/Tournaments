using API.DTO;
using API.Interfaces;
using API.Options;
using API.Repository;
using API.Services;
using API.Validators;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ModelsLibrary.DataAccess;
using ModelsLibrary.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddControllers();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ICoachRepository, CoachRepository>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<IOrganizerRepository, OrganizerRepository>();
builder.Services.AddScoped<IOrganizerService, OrganizerService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRegistrationValidator, RegistrationValidator>();
builder.Services.AddScoped<ICoachRegistrationValidator, CoachRegistrationValidator>();
builder.Services.AddScoped<IOrganizerRegistrationValidator, OrganizerRegistrationValidator>();
builder.Services.AddScoped<PlayerLoginValidator>();
builder.Services.AddScoped<CoachLoginValidator>();
builder.Services.AddScoped<OrganizerLoginValidator>();
builder.Services.AddScoped<ILoginValidatorFactory, LoginValidatorFactory>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tournament Tracker API",
        Description = "Tournament Tracker web application API",
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter the token into the field as 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("API"));
});
// For Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Password policy
    options.Password.RequiredLength = 8;  // Increased minimum required length to 8 characters
    options.Password.RequireDigit = true;  // Requires at least one digit
    options.Password.RequireNonAlphanumeric = true;  // Require at least one special character
    options.Password.RequireUppercase = true;  // Requires at least one uppercase letter
    options.Password.RequireLowercase = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 10;

    // User requirements
    options.User.RequireUniqueEmail = true;

    // Sign-in requirements
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = true; // Enable phone number confirmation
    options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultEmailProvider;
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))

    };
});
// Add Email Options Configuration
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<SmsOptions>(builder.Configuration.GetSection("AfricasTalking"));
// Configure the API key
var apiKey = builder.Configuration.GetValue<string>("AfricasTalking:ApiKey");
// Check if the API key is null or empty
if (string.IsNullOrEmpty(apiKey))
{
    // Handle the case where the API key is not found
    // For example, you might want to log an error or throw an exception
    // Here, we're throwing an exception to halt the application startup
    throw new InvalidOperationException("The AfricaTalking API key is not set in the configuration.");
}

builder.Services.AddSingleton(apiKey);
// Register SmsService
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IEmailService, EmailService>();
// Add SmtpClient as a transient service
builder.Services.AddTransient<SmtpClient>();
builder.Services.AddMemoryCache();
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
