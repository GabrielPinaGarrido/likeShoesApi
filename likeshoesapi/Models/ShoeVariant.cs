using System.ComponentModel.DataAnnotations;

namespace likeshoesapi.Models
{
    public class ShoeVariant
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? VariantName { get; set; }
    }
}
