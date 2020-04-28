using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.Account.Models;

namespace DFC.App.Account.ViewModels
{
    public class EndPointsViewModel
    {
        public List<EndPoint> EndPoints { get; set; }

        public EndPointsViewModel()
        {
            EndPoints = new List<EndPoint>();
        }
    }
}
