﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.Account.Application.Common.Models
{
    public class MarketingPreferences
    {
        public bool OptOutOfMarketing { get; set; }
        public bool MarketingOptIn
        {
            get
            {
                return !OptOutOfMarketing;
            }
            set
            {
                OptOutOfMarketing = !value;
            }
        }

        public bool OptOutOfMarketResearch { get; set; }
        public bool MarketResearchOptIn
        {
            get
            {
                return !OptOutOfMarketResearch;
            }
            set
            {
                OptOutOfMarketResearch = !value;
            }
        }
    }
}
