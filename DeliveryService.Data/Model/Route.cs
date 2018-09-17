using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Data.Model
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        [Required]
        public ICollection<Path> Paths { get; set; }
    }
}
