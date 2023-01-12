using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using UTP_Web_API.Database;
using UTP_Web_API.Repository;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services;
using UTP_Web_API.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UtpContext>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IComplainRepository, ComplainRepository>();
builder.Services.AddScoped<IComplainAdapter, ComplainAdapter>();
builder.Services.AddScoped<IInvestigatorRepository, InvestigatorRepository>();
builder.Services.AddScoped<IInvestigatorAdapter, InvestigatorAdapter>();
builder.Services.AddScoped<IConclusionRepository, ConclusionRepository>();
builder.Services.AddScoped<IConclusionAdapter, ConclusionAdapter>();
builder.Services.AddScoped<IInvestigationStagesRepository, InvestigationStageRepository>();
builder.Services.AddScoped<IInvestigatorComplainsRepository, InvestigatorComplainsRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyAdapter, CompanyAdapter>();
builder.Services.AddScoped<IInvestigationRepository, InvestigationRepository>();
builder.Services.AddScoped<IInvestigationAdapter, InvestigationAdapter>();
builder.Services.AddScoped<IAdministrativeInspectionRepository, AdministrativeInspectionRepository>();
builder.Services.AddScoped<IAdministrativeInspectionAdapter, AdministrativeInspectionAdapter>();



builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });


builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddJsonOptions(option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    option.IncludeXmlComments(xmlPath);

    // This is added to show JWT UI part in Swagger
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description =
            "JWT Authorization header is using Bearer scheme. \r\n\r\n" +
            "Enter 'Bearer' and token separated by a space. \r\n\r\n" +
            "Example: \"Bearer d5f41g85d1f52a\"",
        Name = "Authorization", // Header key name
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
});

builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddCors(p => p.AddPolicy("corsforUtp", builder =>
{
    builder.WithOrigins("*")
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsforUtp");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
