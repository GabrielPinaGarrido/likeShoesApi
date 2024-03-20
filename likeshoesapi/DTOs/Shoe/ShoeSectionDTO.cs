using likeshoesapi.Models;

namespace likeshoesapi.DTOs.Shoe
{
    public class ShoeSectionDTO : ShoeSection
    {
        public List<ShoeType>? ShoeTypes { get; set; }
    }
}
