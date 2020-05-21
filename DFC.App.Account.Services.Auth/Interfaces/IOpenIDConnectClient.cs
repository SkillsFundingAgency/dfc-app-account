using System.Threading.Tasks;
using DFC.App.Account.Application.Common.Interfaces;

namespace DFC.App.Account.Services.Auth.Interfaces
{
    public interface IOpenIDConnectClient
    {
        Task<IResult> VerifyPassword(string userName, string password);
    }
}
