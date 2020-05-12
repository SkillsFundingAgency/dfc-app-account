using System.Collections.Generic;
using System.Linq;
using DFC.App.Account.Application.Common.Models;

namespace DFC.App.Account.Models
{
    public class GetAddressIoResult
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Postcode { get; set; }
        public IList<GetAddressIOAddressModel> Addresses { get; set; }

        public IList<PostalAddressModel> MapToPostalAddressModel()
        {
            return Addresses.Select(x => new PostalAddressModel
            {
                Line1 = x.Line1,
                Line2 = x.Line2,
                Line3 = x.Line3,
                Line4 = x.Line4,
                City = x.City,
                PostalCode = Postcode,
                Text = string.Join(string.Empty, x.FormattedAddress)
            }).ToList();
        }
    }
}
