using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GameService.Utility;

public class JwtHelper(IConfiguration configuration)
{

    // private readonly string corpId = configuration["Jwt:CorpId"]!;
    // private readonly string secret = configuration["Jwt:Secret"]!;
    private readonly string? user = configuration["Jwt:Issuer"];
    private readonly string? audience = configuration["Jwt:Audience"];
    private readonly byte[] secretKey = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!);


    public string CreateToken(long userId)
    {

        // 1. 定义需要使用到的Claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userId.ToString()), //HttpContext.User.Identity.Name
            new Claim(ClaimTypes.Role, "admin"), //HttpContext.User.IsInRole("r_admin")
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Username", "Admin")
        };

        // 2. 从 appsettings.json 中读取SecretKey
        var secretKey = new SymmetricSecurityKey(this.secretKey);

        // 3. 选择加密算法
        const string algorithm = SecurityAlgorithms.HmacSha256;

        // 4. 生成Credentials
        var signingCredentials = new SigningCredentials(secretKey, algorithm);

        // 5. 根据以上，生成token
        var jwtSecurityToken = new JwtSecurityToken(
            user,     //Issuer
            audience,   //Audience
            claims,                          //Claims,
            DateTime.Now,                    //notBefore
            DateTime.Now.AddSeconds(300),    //expires
            signingCredentials               //Credentials
        );

        // 6. 将token变为string
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return token;
    }
}