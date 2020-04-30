using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DFC.App.Account.Application.Common.Enums
{
    public class CommonEnums
    {
        public enum Channel
        {
            [Display(Name = "Email")]
            Email = 0,
            [Display(Name = "Text")]
            Text = 1,
            [Display(Name = "Phone")]
            Phone = 2
        }
        public enum Gender
        {
            [Display(Name = "Select", Order = 0)]
            NotKnown = 0,
            [Display(Name = "Female", Order = 1)]
            Female = 3,
            [Display(Name = "Male", Order = 2)]
            Male = 2,
            [Display(Name = "Prefer not to say", Order = 3)]
            NotApplicable = 1
        }
        public enum Title
        {
            [Display(Name = "Select", Order = 0)]
            NotKnown = 0,
            [Display(Name = "Miss", Order = 1)]
            Miss = 2,
            [Display(Name = "Mr", Order = 2)]
            Mr = 3,
            [Display(Name = "Mrs", Order = 3)]
            Mrs = 4,
            [Display(Name = "Ms", Order = 4)]
            Ms = 5,
            [Display(Name = "Dr", Order = 5)]
            Dr = 1
        }

        public enum FormTitle
        {
            [Display(Name = "Mrs", Order = 0)]
            Mrs = 0,
            [Display(Name = "Mr", Order = 1)]
            Mr = 1,
            [Display(Name = "Miss", Order = 2)]
            Miss = 2,
            [Display(Name = "Ms", Order = 3)]
            Ms = 3,
            [Display(Name = "Other", Order = 4)]
            Other = 4
        }
    }
}
