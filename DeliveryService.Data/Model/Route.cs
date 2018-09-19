using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Data.Model
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        [Required]
        public int Cost { get; set; }

        [Required]
        public int Time { get; set; }

        [Required]
        public int PathId { get; set; }

        [Required, ForeignKey("PathId")]
        public Path Path { get; set; }
    }
}
