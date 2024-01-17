using System.ComponentModel.DataAnnotations;

namespace likeshoesapi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        public string? Email { get; set; }

        [Required]
        [StringLength(100)]
        public string? Full_name { get; set; }

        [Required]
        [StringLength(20)]
        public string? Password { get; set; }
    }
}
