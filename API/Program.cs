using API.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repo;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IHistoryRepo, HistoryRepo>();
builder.Services.AddScoped<IChatRepo, ChatRepo>();
builder.Services.AddControllers();

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Configure Swagger to use Bearer token authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});
var serviceProvider = builder.Services.BuildServiceProvider();
var userRepository = serviceProvider.GetRequiredService<IAccountRepo>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(async o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(await userRepository.GetSecretKeyAsync()),
            ClockSkew = TimeSpan.Zero, // No clock skew tolerance
            // There is a concept called Clock skew, this allows the comparingson between server and the token-
            // experation time to be a little off. by default there is a built in clockskew tolerance, in the validation process
            // However by setting the TimeSpan.Zero, it removes the allowance for tolerance in the server and token time difference
            IssuerSigningKeyResolver = (token, secutiryToken, kid, validationParameters) =>
            {
                var issuerSigningKey = GetIssuerSigningKeyAsync().GetAwaiter().GetResult();
                return new List<SecurityKey> { issuerSigningKey };
            }



        };
    });
async Task<SymmetricSecurityKey> GetIssuerSigningKeyAsync()
{
    return new SymmetricSecurityKey(await userRepository.GetSecretKeyAsync());
}

builder.Services.AddAuthorization(); // Line added

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Line added
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");
app.Run();
