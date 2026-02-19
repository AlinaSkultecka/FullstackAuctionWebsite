using Lab3_FullstackAuctionWebsite.Core.Interfaces;
using Lab3_FullstackAuctionWebsite.Core.Services;
using Lab3_FullstackAuctionWebsite.Data;
using Lab3_FullstackAuctionWebsite.Data.Interfaces;
using Lab3_FullstackAuctionWebsite.Data.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// -------------------- DATABASE --------------------

var connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Lab3_Auction_Db;Integrated Security=True;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connString)
);

// -------------------- REPOSITORIES --------------------

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IAuctionRepo, AuctionRepo>();
builder.Services.AddScoped<IBidRepo, BidRepo>();


// -------------------- SERVICES --------------------

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IBidService, BidService>();


// -------------------- AUTOMAPPER --------------------

builder.Services.AddAutoMapper(
    typeof(Lab3_FullstackAuctionWebsite.Core.Mapping.MappingProfile).Assembly
);


// -------------------- JWT AUTHENTICATION --------------------

var jwtSettings = builder.Configuration.GetSection("Jwt");

var secret = jwtSettings["Key"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();


// -------------------- SWAGGER --------------------

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auction API",
        Description = "Web API for Lab 3 Fullstack Auction"
    });

    // Enable JWT in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer {your token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
});


var app = builder.Build();


// -------------------- MIDDLEWARE --------------------

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
