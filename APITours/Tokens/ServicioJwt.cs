using APITours.Modelos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APITours.Tokens
{
    public class ServicioJwt
    {
        private readonly IConfiguration _configuration;
        public ServicioJwt(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerarToken(UsuariosModel usuario)
        {
            var jwt = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwt["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, usuario.NombreUsuario),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.UniqueName, usuario.NombreUsuario),
                new ("rol", "Administrador") // Puedes cambiar el rol según tu lógica
            };
            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
