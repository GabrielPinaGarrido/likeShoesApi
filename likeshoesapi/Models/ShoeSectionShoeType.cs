namespace likeshoesapi.Models
{
    public class ShoeSectionShoeType
    {
        public int ShoeTypeId { get; set; }
        public int ShoeSectionId { get; set; }

        // Propiedades de navegación
        public ShoeType? ShoeType { get; set; }
        public ShoeSection? ShoeSection { get; set; }
    }
}
