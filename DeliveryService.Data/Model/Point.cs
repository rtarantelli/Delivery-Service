using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Data.Model
{
    public class Point
    {
        [Key]
        public int PointId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
