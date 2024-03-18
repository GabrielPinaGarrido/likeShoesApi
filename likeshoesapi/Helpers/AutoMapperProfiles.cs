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

            // Shoes
            CreateMap<ShoeSectionPostDTO, ShoeSection>()
                .ForMember(
                    shoeSection => shoeSection.ShoeSectionShoeType,
                    options => options.MapFrom(MapShoeSectionShoeType)
                );
        }

        private List<ShoeSectionShoeType> MapShoeSectionShoeType(
            ShoeSectionPostDTO shoeSectionPostDTO,
            ShoeSection shoeSection
        )
        {
            var result = new List<ShoeSectionShoeType>();

            if (shoeSectionPostDTO.ShoeTypeIds == null)
            {
                return result;
            }

            foreach (var shoeTypeId in shoeSectionPostDTO.ShoeTypeIds)
            {
                result.Add(new ShoeSectionShoeType() { ShoeTypeId = shoeTypeId, });
            }

            return result;
        }
    }
}
