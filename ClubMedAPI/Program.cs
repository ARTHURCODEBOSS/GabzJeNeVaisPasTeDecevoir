using ClubMedAPI.Models.EntityFramework;
using ClubMedAPI.Models.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ===== DbContext PostgreSQL =====
builder.Services.AddDbContext<ClubMedContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ClubMedContext")));

// ===== Repository Pattern (DI) =====
builder.Services.AddScoped<IDataRepository<Activite>, DataRepository<Activite>>();
builder.Services.AddScoped<IDataRepository<ActiviteAdulte>, DataRepository<ActiviteAdulte>>();
builder.Services.AddScoped<IDataRepository<ActiviteEnfant>, DataRepository<ActiviteEnfant>>();
builder.Services.AddScoped<IDataRepository<Adresse>, DataRepository<Adresse>>();
builder.Services.AddScoped<IDataRepository<AutreVoyageur>, DataRepository<AutreVoyageur>>();
builder.Services.AddScoped<IDataRepository<Avis>, DataRepository<Avis>>();
builder.Services.AddScoped<IDataRepository<CarteBancaire>, DataRepository<CarteBancaire>>();
builder.Services.AddScoped<IDataRepository<Categorie>, DataRepository<Categorie>>();
builder.Services.AddScoped<IDataRepository<Chambre>, DataRepository<Chambre>>();
builder.Services.AddScoped<IDataRepository<Client>, DataRepository<Client>>();
builder.Services.AddScoped<IDataRepository<Club>, DataRepository<Club>>();
builder.Services.AddScoped<IDataRepository<LieuRestauration>, DataRepository<LieuRestauration>>();
builder.Services.AddScoped<IDataRepository<Localisation>, DataRepository<Localisation>>();
builder.Services.AddScoped<IDataRepository<Partenaire>, DataRepository<Partenaire>>();
builder.Services.AddScoped<IDataRepository<Periode>, DataRepository<Periode>>();
builder.Services.AddScoped<IDataRepository<Photo>, DataRepository<Photo>>();
builder.Services.AddScoped<IDataRepository<Regroupement>, DataRepository<Regroupement>>();
builder.Services.AddScoped<IDataRepository<Reservation>, DataRepository<Reservation>>();
builder.Services.AddScoped<IDataRepository<SousLocalisation>, DataRepository<SousLocalisation>>();
builder.Services.AddScoped<IDataRepository<Station>, DataRepository<Station>>();
builder.Services.AddScoped<IDataRepository<TrancheAge>, DataRepository<TrancheAge>>();
builder.Services.AddScoped<IDataRepository<Transaction>, DataRepository<Transaction>>();
builder.Services.AddScoped<IDataRepository<Transport>, DataRepository<Transport>>();
builder.Services.AddScoped<IDataRepository<TypeActivite>, DataRepository<TypeActivite>>();
builder.Services.AddScoped<IDataRepository<TypeChambre>, DataRepository<TypeChambre>>();

// ===== 2FA Services =====
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ClubMedAPI.Services.ITwoFactorService, ClubMedAPI.Services.TwoFactorService>();

// ===== JWT Authentication =====
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ClubMedSuperSecretKeyForJwtToken2026!!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = "ClubMedAPI",
            ValidateAudience = true,
            ValidAudience = "ClubMedFrontend",
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

// ===== Authorization Policies =====
builder.Services.AddAuthorization();

// ===== Controllers + JSON =====
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.MaxDepth = 256;
        options.JsonSerializerOptions.PropertyNamingPolicy = new LowerCaseNamingPolicy();
    });

// ===== Swagger =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== CORS (autoriser le front Vue) =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:8045", "http://localhost:5173", "http://127.0.0.1:8045")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Middleware: read auth cookie and set Authorization header (like Laravel Sanctum)
app.Use(async (context, next) =>
{
    if (context.Request.Cookies.TryGetValue("auth_token", out var token)
        && !context.Request.Headers.ContainsKey("Authorization"))
    {
        context.Request.Headers.Append("Authorization", $"Bearer {token}");
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public class LowerCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToLowerInvariant();
}
