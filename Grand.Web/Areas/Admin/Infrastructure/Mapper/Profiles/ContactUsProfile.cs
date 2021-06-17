﻿using AutoMapper;
using Grand.Domain.Messages;
using Grand.Core.Mapper;
using Grand.Web.Areas.Admin.Models.Messages;

namespace Grand.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class ContactUsProfile : Profile, IAutoMapperProfile
    {
        public ContactUsProfile()
        {
            CreateMap<ContactUs, ContactFormModel>()
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}