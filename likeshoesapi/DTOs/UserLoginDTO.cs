using System.ComponentModel.DataAnnotations;

namespace likeshoesapi.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        [StringLength(60)]
        public string? Email { get; set; }

        [Required]
        [StringLength(20)]
        public string? Password { get; set; }
    }
}
