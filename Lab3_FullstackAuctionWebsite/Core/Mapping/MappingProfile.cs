using AutoMapper;
using Lab3_FullstackAuctionWebsite.Core.DTOs;
using Lab3_FullstackAuctionWebsite.Core.DTOs.Auction;
using Lab3_FullstackAuctionWebsite.Core.DTOs.Bid;
using Lab3_FullstackAuctionWebsite.Core.DTOs.User;
using Lab3_FullstackAuctionWebsite.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab3_FullstackAuctionWebsite.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // -------------------- USER --------------------

            CreateMap<User, UserResponseDto>().ReverseMap();

            CreateMap<RegisterUserDto, User>();


            // -------------------- AUCTION --------------------

            CreateMap<Auction, AuctionResponseDto>().ReverseMap();

            CreateMap<CreateAuctionDto, Auction>();

            CreateMap<UpdateAuctionDto, Auction>();


            // -------------------- BID --------------------

            CreateMap<Bid, BidResponseDto>()
              .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName));


            CreateMap<CreateBidDto, Bid>();
        }
    }
}

