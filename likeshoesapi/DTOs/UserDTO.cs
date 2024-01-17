using System.ComponentModel.DataAnnotations;

namespace likeshoesapi.DTOs
{
    public class UserDTO
    {
        [Required]
        [StringLength(60)]
        public string? Email { get; set; }

        [Required]
        [StringLength(100)]
        public string? Full_name { get; set; }
    }
}
