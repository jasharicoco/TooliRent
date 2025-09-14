using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Infrastructure.Repositories;
using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.Mapping;
using TooliRent.Services.Services;
using TooliRent.Services.Services.Interfaces;
using TooliRent.Services.Validators;
using TooliRent.WebAPI.Auth;

namespace TooliRent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -------------------------------
            // CORS Policy
            // -------------------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:5173",  // i fall du kör React med HTTP
                        "https://localhost:5173"  // om frontend körs med HTTPS
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // -------------------------------
            // Controllers & JSON Options
            // -------------------------------
            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    // Serialisera enums som strängar
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddEndpointsApiExplorer();

            // -------------------------------
            // Swagger / OpenAPI
            // -------------------------------
            builder.Services.AddSwaggerGen(o =>
            {
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            // -------------------------------
            // DbContexts
            // -------------------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<AppUserDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // -------------------------------
            // Identity
            // -------------------------------
            builder.Services.AddIdentityCore<AppUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppUserDbContext>()
            .AddDefaultTokenProviders();

            // -------------------------------
            // JWT Authentication
            // -------------------------------
            var jwtConfig = builder.Configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig["Issuer"],
                        ValidAudience = jwtConfig["Audience"],
                        IssuerSigningKey = key,
                        ClockSkew = TimeSpan.FromMinutes(1) // För att hantera liten tidsskillnad
                    };
                });

            builder.Services.AddAuthorization();

            // -------------------------------
            // FluentValidation
            // -------------------------------
            builder.Services.AddValidatorsFromAssemblyContaining<ReviewValidator>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ReviewValidator>();
            // builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            // -------------------------------
            // AutoMapper (registrerar alla profiler i mapping-assemblyn)
            // -------------------------------
            builder.Services.AddAutoMapper(typeof(CustomerProfile).Assembly);

            // -------------------------------
            // Repositories & Services
            // -------------------------------
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IToolService, ToolService>();
            builder.Services.AddScoped<IToolCategoryService, ToolCategoryService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IRentalService, RentalService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IToolRepository, ToolRepository>();
            builder.Services.AddScoped<IToolCategoryRepository, ToolCategoryRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IRentalRepository, RentalRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

            builder.Services.AddHostedService<OverdueRentalService>();
            builder.Services.AddScoped<IEmailService, SmtpEmailService>();

            // -------------------------------
            // Build App
            // -------------------------------
            var app = builder.Build();

            // -------------------------------
            // Middleware Pipeline
            // -------------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
