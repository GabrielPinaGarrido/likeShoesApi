using System.ComponentModel.DataAnnotations;

namespace likeshoesapi.Models
{
    public class ShoeSection
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? SectionName { get; set; }

        public List<ShoeSectionShoeType>? ShoeSectionShoeType { get; set; }
    }
}
