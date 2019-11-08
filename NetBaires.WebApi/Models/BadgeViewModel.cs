﻿using NetBaires.Data;

namespace NetBaires.Api.Models
{
    public class BadgeViewModel
    {
        public BadgeViewModel(Badge badge, string badgeUrl)
        {
            Badge = badge;
            BadgeUrl = badgeUrl;
        }

        public Badge Badge { get; set; }
        public string BadgeUrl { get; set; }

    }
}