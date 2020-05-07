using DFC.App.Account.Application.SkillsHealthCheck.Models;
using System.Collections.Generic;

namespace DFC.App.Account.Services.SHC.Interfaces
{
    public interface ISkillsHealthCheckService
    {
        List<ShcDocument> GetShcDocumentsForUser(string llaId);
    }
}
