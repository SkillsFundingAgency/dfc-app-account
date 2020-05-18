using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common.Interfaces;

namespace DFC.App.Account.Services.AzureB2CAuth.Interfaces
{
    public interface IOpenIDConnectClient
    {
        Task<string> GetRegisterUrl();
        Task<string> GetSignInUrl();
        string GetAuthdUrl();
        Task<JwtSecurityToken> ValidateToken(string token);
        Task<IResult> VerifyPassword(string userName, string password);
    }
}
