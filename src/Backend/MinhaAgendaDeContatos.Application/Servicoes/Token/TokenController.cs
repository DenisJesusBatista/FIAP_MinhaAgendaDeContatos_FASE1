using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MinhaAgendaDeContatos.Application.Servicoes.Token;
public class TokenController
{
    private const string EmailAlias = "eml";
    private readonly double _tempoDeVidaDoTokenEmMinutos;
    private readonly string _chaveDeSeguranca;    


    public TokenController(double tempoDeVidaDoTokenEmMinutos, string chaveDeSeguranca)
    {
        _tempoDeVidaDoTokenEmMinutos = tempoDeVidaDoTokenEmMinutos;
        _chaveDeSeguranca = chaveDeSeguranca;        

    }


    public string GerarToken(string emailDoUsuario)
    {
        var claims = new List<Claim>()
        {
            new Claim(EmailAlias, emailDoUsuario),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescripton = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_tempoDeVidaDoTokenEmMinutos),

            /*Metodo usado para criptografar o token*/
            SigningCredentials = new SigningCredentials(SimetricKey(),SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescripton);

        return tokenHandler.WriteToken(securityToken);

    }


    public void ValidarToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var parametrosValidacao = new TokenValidationParameters
        {   
            RequireExpirationTime = true,
            IssuerSigningKey = SimetricKey(),
            ClockSkew = new TimeSpan(0),
            ValidateIssuer = false,
            ValidateAudience = false

        };

        tokenHandler.ValidateToken(token, parametrosValidacao, out _);
    }


    private SymmetricSecurityKey SimetricKey()
    {
        var symetricKey = Convert.FromBase64String(_chaveDeSeguranca);
        return new SymmetricSecurityKey(symetricKey);
    }
}
