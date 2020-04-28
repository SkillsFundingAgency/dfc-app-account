using System.ComponentModel.DataAnnotations;

namespace DFC.App.Account.Services.DSS.Models
{
    public enum PreferredContactMethod 
    {
        Email,
        Text,
        Phone
    }

    public enum Title
    {
        Select,
        Dr,
        Miss,
        Mr,
        Mrs,
        Ms
    }

    public enum Gender
    {
        Select,
        [Display(Name = "Prefer not to say")]
        Prefernottosay,
        Male,
        Female
    }

}