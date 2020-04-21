using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Account.Models
{
    public class CompositeSettings
    {
        // Path that is registered for this application in the Cosmos 'paths' collection
        public string Path { get; set; }
        public string CDN { get; set; }
    }
}
