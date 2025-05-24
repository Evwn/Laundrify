using System.ComponentModel.DataAnnotations;

namespace Laundrify.Data
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string UnitType { get; set; } // e.g., 'kg', 'item'
    }
} 