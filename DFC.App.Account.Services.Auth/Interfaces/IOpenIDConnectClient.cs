using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace DFC.App.Account.Services.AzureB2CAuth.Interfaces
{
    public interface IOpenIDConnectClient
    {
        Task<string> GetRegisterUrl();
        Task<string> GetSignInUrl();
        Task<JwtSecurityToken> ValidateToken(string token);        
    }
}
