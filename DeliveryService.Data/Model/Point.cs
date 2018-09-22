using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Data.Model
{
    public class Point
    {
        [Key]
        public int PointId { get; set; }

        [Required, MaxLength(1)]
        public string Name { get; set; }
    }
}
