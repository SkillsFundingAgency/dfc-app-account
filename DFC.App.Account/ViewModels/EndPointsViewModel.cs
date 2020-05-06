using System.Collections.Generic;
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
