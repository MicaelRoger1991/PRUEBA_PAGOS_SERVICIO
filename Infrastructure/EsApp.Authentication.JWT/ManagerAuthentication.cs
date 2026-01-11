using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EsApp.Authentication.JWT;

public class ManagerAuthentication : IManagerAuthentication
{
    private readonly string keyJwt;
    private readonly string _sectionKeyJwt = "secretKeyJWT";
    private readonly int _timeSession;
    private readonly string _sectionTimeSession = "timeSession";
    public ManagerAuthentication(IConfiguration configuration)
    {
        keyJwt = configuration.GetSection(_sectionKeyJwt).Value!;
        _timeSession = int.Parse(configuration.GetSection(_sectionTimeSession).Value!);
    }

    private string GenerarToken(AuthenticationRequest request)
    {
        var keyBytes = Encoding.ASCII.GetBytes(keyJwt);

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, request.user),
                new Claim("role", JsonSerializer.Serialize(request.role)),
                new Claim(ClaimTypes.Name, request.name)
            };
        var claimsIdentity = new ClaimsIdentity(claims, "Token");
        var credencialesToken = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256Signature
            );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(_timeSession),
            SigningCredentials = credencialesToken
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

        string tokenCreado = tokenHandler.WriteToken(tokenConfig);

        return tokenCreado;


    }
    private string GenerarRefreshToken()
    {

        var byteArray = new byte[64];
        var refreshToken = "";

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(byteArray);
            refreshToken = Convert.ToBase64String(byteArray);
        }
        return refreshToken;
    }
    public AuthenticationToken getToken(AuthenticationRequest request)
    {
        string tokenCreate = GenerarToken(request);
        string refreshTokenCreate = GenerarRefreshToken();

        return new AuthenticationToken(
            token: tokenCreate,
            refreshToken: refreshTokenCreate
        );
    }
    public bool getValidateTokenFromRefresh(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenExpiradoSupuestamente = tokenHandler.ReadJwtToken(token);

        return !(tokenExpiradoSupuestamente.ValidTo > DateTime.UtcNow);
    }
    public AuthenticationToken getRefreshToken(AuthenticationToken request, AuthenticationRequest requestLogin)
    {
        var refreshTokenCreado = GenerarRefreshToken();
        var tokenCreado = GenerarToken(requestLogin);

        return new AuthenticationToken(
            token: tokenCreado,
            refreshToken: refreshTokenCreado
        );
    }
}
