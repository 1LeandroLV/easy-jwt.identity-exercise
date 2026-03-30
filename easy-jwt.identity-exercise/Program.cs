using easy_jwt.identity_exercise.Services;
using easy_jwt.identity_exercise.Data;
using easy_jwt.identity_exercise.Models;
// NYTT: behövs för JWT authentication
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // behövs för token validation
using System.Text; // NYTT: behövs för swagger JWT authorize-knapp

namespace easy_jwt.identity_exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // detta registrerar Appdbcontext i dependency injection /databas
            builder.Services.AddDbContext<AppDbContext>(options =>
            // säger vilken databas vi använder
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<AppUser, IdentityRole>()
            // Identity ska använda databasen via APPdbContext
            .AddEntityFrameworkStores<AppDbContext>()
            // detta lägger till standardfunktioner som identity använder internt bra att ha.
            .AddDefaultTokenProviders();

            builder.Services.AddScoped<TokenService>();
            //JWT Authentication konfiguration
            // appens authentication ska använda JWT bearer som standard
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // här börjar konfigurationen för hur JWT ska läsas och valideras
            .AddJwtBearer(options =>
            {
                // så här ska appen kontrollera om token är giltig
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // kontrollera att token kommer från rätt issuer
                    ValidateIssuer = true,

                    // kontrollerar att token är avsedd för rätt mottagare
                    ValidateAudience = true,

                    // kontrollerar att token inte gått ut
                    ValidateLifetime = true,

                    // kontrollerar att signaturen är giltig och att token inte manipulerats
                    ValidateIssuerSigningKey = true,

                    // här hämtar appen rätt issuer från appsettings.json
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],

                    // här hämtar appen rätt audience från appsettings.json
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    // här sätter vi den hemliga nyckeln som används för att verifiera token
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

            builder.Services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer();

            // swagger konfiguration för JWT authorize-knappen
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //  detta gör att API:t kan läsa JWT token från Authorization header
            app.UseAuthentication();

            // kontrollerar om användaren har rätt behörighet
            app.UseAuthorization();
            //
            app.MapControllers();
            using (var scope = app.Services.CreateScope()) 
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                DbSeeder.SeedRolesAndAdminAsync(roleManager, userManager).Wait();
            }

            app.Run();
        }
    }
}