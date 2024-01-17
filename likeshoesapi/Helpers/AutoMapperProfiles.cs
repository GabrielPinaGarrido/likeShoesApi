using AutoMapper;
using likeshoesapi.DTOs;
using likeshoesapi.Models;

namespace likeshoesapi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserPostDTO, User>();
            CreateMap<UserLoginDTO, UserDTO>();
        }
    }
}
