using System.ComponentModel.DataAnnotations;
using likeshoesapi.Models;

namespace likeshoesapi.DTOs.Shoe
{
    public class ShoeDTO
    {
        public int Id { get; set; }

        public string? ShoeName { get; set; }

        public string? ShoeDescription { get; set; }

        public int? ShoePrice { get; set; }

        public string? ShoeImage { get; set; }

        public int? ShoeSectionId { get; set; }
        public int? ShoeTypeId { get; set; }
    }
}
