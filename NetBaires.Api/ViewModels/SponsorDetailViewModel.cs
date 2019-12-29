﻿using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.ViewModels
{
    public class SponsorDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SiteUrl { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public class SponsorDetailViewModelProfile : Profile
        {
            public SponsorDetailViewModelProfile()
            {
                CreateMap<Sponsor, SponsorDetailViewModel>();
            }
        }
    }
}