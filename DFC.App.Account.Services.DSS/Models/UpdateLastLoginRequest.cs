using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace DFC.App.Account.Services.DSS.Models
{
    class UpdateLastLoginRequest
    {
        public DateTime LastLoggedInDateTime { get; set; }
    }
}
