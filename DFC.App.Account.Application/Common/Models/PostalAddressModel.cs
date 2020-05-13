namespace DFC.App.Account.Application.Common.Models
{
    public class PostalAddressModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int? Error { get; set; }
        public string Description { get; set; }
        public string Cause { get; set; }
        public string Resolution { get; set; }
    }

}
