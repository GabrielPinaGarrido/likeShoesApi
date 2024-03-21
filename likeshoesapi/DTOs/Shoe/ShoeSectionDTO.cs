using likeshoesapi.Models;
using System.ComponentModel.DataAnnotations;

namespace likeshoesapi.DTOs.Shoe
{
    public class ShoeSectionDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? SectionName { get; set; }

        public List<ShoeTypeDTO>? ShoeTypes { get; set; }
    }
}
