using System.ComponentModel.DataAnnotations;

namespace likeshoesapi.Models
{
    public class ShoeType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? TypeName { get; set; }

        public List<ShoeSectionShoeType>? ShoeSectionShoeType { get; set; }
    }
}
