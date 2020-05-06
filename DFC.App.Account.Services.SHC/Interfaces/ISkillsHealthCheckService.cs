using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.Account.Services.SHC.Models;

namespace DFC.App.Account.Services.SHC.Interfaces
{
    public interface ISkillsHealthCheckService
    {
        List<SkillsDocument> GetSHCDocumentsForUser(string llaId);
    }
}
