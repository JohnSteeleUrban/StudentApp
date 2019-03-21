using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using StudentWebApi.Data;
using StudentWebApi.Models;
using StudentWebApi.Repository;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace StudentWebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost:5000",
                    ValidAudience = "http://localhost:5000",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Authorization:Secret").Value))
                };
            });
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Pagination"));
            });
        }
        public static void ConfigureStudentRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Settings>(options =>
            {
                options.ConnectionString = configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = configuration.GetSection("MongoConnection:Database").Value;
                options.SuperSecretDontTellNobody = configuration.GetSection("Authorization:Secret").Value;
            });

            services.AddSingleton<IMongoClient, MongoClient>(
                _ => new MongoClient(configuration.GetSection("MongoConnection:ConnectionString").Value));

            services.AddTransient<IStudentContext, StudentContext>();
            services.AddTransient<IStudentRepository, StudentRepository>();
        }

        public static void ConfigureSwaggerGeneration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Student API", Version = "v1" });
                var security = new Dictionary<string, IEnumerable<string>>
               {
                   {"Bearer", new string[] { }},
               };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                });
                c.AddSecurityRequirement(security);
            });
        }
    }
}
