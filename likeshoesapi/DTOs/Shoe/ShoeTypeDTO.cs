using System.ComponentModel.DataAnnotations;

namespace likeshoesapi.DTOs.Shoe
{
    public class ShoeTypeDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? TypeName { get; set; }
    }
}
