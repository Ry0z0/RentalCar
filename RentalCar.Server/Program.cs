using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RentalCar.Auth;
using RentalCar.Common.AutoMapper;
using RentalCar.Entity;
using RentalCar.Entity.Contexts;
using RentalCar.Repository.Infrastructures;
using System.Text;
using RentalCar.Repository.Repositories.BookingRepository;
using RentalCar.Repository.Repositories.CarOwnerRepository;
using RentalCar.Repository.Repositories.CarRepositoryRepository;
using RentalCar.Repository.Repositories.CustomerRepository;
using RentalCar.Repository.Repositories.AddressRepository;
using RentalCar.Repository.Repositories.FeedbackRepository;
using RentalCar.Server.Controllers;

namespace RentalCar.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            IServiceCollection services = builder.Services;

            builder.Services.AddControllers();
            //Add cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            //Add SQL context
            builder.Services.AddDbContext<RentalCarDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("MyCnn"),
                    b => b.MigrationsAssembly("RentalCar.Entity")));


            // Configure JWT Authentication
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            // Configure Authorization with Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));
                options.AddPolicy("CarOwnerPolicy", policy => policy.RequireRole("CarOwner"));
            });
            //services.AddSingleton<EncryptionService>(new EncryptionService("cEQ0qgzWCJY6hbB9hv97lrPZfFADjzlo"));

            // Register JWT Token Service
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            // Register the UnitOfWork
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            //Register the repositories
            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<ICarOwnerRepository, CarOwnerRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IAddressCityRepository, AddressCityRepository>();
            services.AddTransient<IAddressDistrictRepository, AddressDistrictRepository>();
            services.AddTransient<IAddressWardRepository, AddressWardRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();

            //Register the services
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<ICarService, CarService>();
            services.AddTransient<ICarOwnerService, CarOwnerService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IAddressCityService, AddressCityService>();
            services.AddTransient<IAddressDistrictService, AddressDistrictService>();
            services.AddTransient<IAddressWardService, AddressWardService>();
            services.AddTransient<IFeedbackService, FeedbackService>();


            // Register the AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });

            });
            services.AddHostedService<TokenCleanupService>();

            // Add distributed memory cache to store session data
            services.AddDistributedMemoryCache();

            // Add session services
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAllOrigins");
            app.MapControllers();
            app.UseSession();
            // Initialize the seed data
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                await SeedData.Initialize(serviceProvider);
            }

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
