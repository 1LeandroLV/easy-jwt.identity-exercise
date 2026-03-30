using easy_jwt.identity_exercise.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace easy_jwt.identity_exercise.Services
// denna klassen ska skapa jwt token för en användare,
// den ska läsa jwt-inställning, hämta roller, skapa claims,
// skapa token, returnera token som string
{
    public class TokenService // skapa jwt tokens
    {
        // dependencies / ett verktyg eller en tjänst som klassen använder
        private readonly IConfiguration _configuration; // för inställningar från appsettings.json
        private readonly UserManager<AppUser> _userManager; // behövs för att hämta användarens roller

        // Detta gör att ASP.NET automatiskt skickar in dependencies via Dependency Injection.
        public TokenService(
            IConfiguration configuration,
            UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> CreateToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            // skapa claims-lista
            // claim = information om användaren som lagras i token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // skapa säkerhetsnyckeln för token
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            // skapa signingsuppgifter
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // här skapar du ett nytt objekt av typen JwtSecurityToken
            var token = new JwtSecurityToken(
                // sätter issuer: vem som har skapat tokenen
                issuer: _configuration["Jwt:Issuer"],

                // vad gör den: ungefär vilken mottagare ska använda tokenen
                // vilket system gäller den här tokenen för
                audience: _configuration["Jwt:Audience"],

                // här skickar du in claims-listan med användarens information
                claims: claims,

                // vad den gör: den bestämmer när tokenen slutar vara giltig
                // här säger du: tokenen ska gälla från nu och 2 timmar framåt
                expires: DateTime.UtcNow.AddHours(2),

                // vad den gör: här kopplar du in signeringen
                signingCredentials: credentials
            );

            // Nu är JwtSecurityToken-objektet färdigbyggt
            // det här är sista steget: gör om token till string och returnera den
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
