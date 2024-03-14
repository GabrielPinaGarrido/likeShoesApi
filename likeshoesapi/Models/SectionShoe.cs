using System.ComponentModel.DataAnnotations;

namespace likeshoesapi.Models
{
    public class SectionShoe
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? Section {  get; set; }

        [Required]
        [StringLength(20)]
        
    }
}

//{
//nameButton: "Mujeres",
//      type_shoe:
//    [
//        {
//    nameSection: "Tenis",
//          sectionElements: ["Cerrados", "Sin cordon"],
//        },
//        {
//    nameSection: "Botas",
//          sectionElements: [],
//        },
//      ],
//    },
