using System.ComponentModel.DataAnnotations;

namespace likeshoesapi.Models
{
    public class Shoe
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? ShoeName { get; set; }

        [Required]
        [StringLength(50)]
        public string? ShoeDescription { get; set; }

        [Required]
        [StringLength(8)]
        public int? ShoePrice { get; set; }

        [Required]
        [StringLength(500)]
        public string? ShoeImage { get; set; }

        public int? ShoeSectionId { get; set; }
        public int? ShoeTypeId { get; set; }
    }
}
