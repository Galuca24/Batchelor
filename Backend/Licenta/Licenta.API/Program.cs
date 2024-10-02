using Ergo.Application.Contracts.Interfaces;
using Hangfire;
using Hangfire.PostgreSql;
using Licenta.API.Services;
using Licenta.API.Utility;
using Licenta.Application;
using Licenta.Application.Models;
using Licenta.Application.Persistence;
using Licenta.Identity;
using Licenta.Identity.Services;
using Licenta.Infrastructure;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructureToDI(builder.Configuration);
builder.Services.AddInfrastrutureIdentityToDI(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddHttpClient();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
builder.Services.AddHttpClient<IGoogleBooksService, GoogleBooksService>();
builder.Services.AddHttpClient<ILibrivoxService, LibrivoxService>();
builder.Services.AddScoped<IUserManager, ApplicationUserManager>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder =>
        builder.WithOrigins("http://localhost:5173") // Specific? originile permise
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials()); // Permiterea creden?ialelor este important? pentru SignalR
});

var spotifyClientId = builder.Configuration["Spotify:ClientId"];
var spotifyClientSecret = builder.Configuration["Spotify:ClientSecret"];
builder.Services.AddSingleton<ISpotifyService>(new SpotifyService(spotifyClientId, spotifyClientSecret));

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Ergo API",

    });

    c.OperationFilter<FileResultContentTypeOperationFilter>();
});

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("LicentaConnection"), new PostgreSqlStorageOptions
    {
        InvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.FromSeconds(15),
        DistributedLockTimeout = TimeSpan.FromMinutes(10)
    }));

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Open");

app.UseAuthorization();

app.MapControllers();

var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
recurringJobManager.AddOrUpdate<MembershipExpirationService>(
    "check-and-expire-memberships",
    service => service.CheckAndExpireMemberships(),
    Cron.Daily);

app.Run();
