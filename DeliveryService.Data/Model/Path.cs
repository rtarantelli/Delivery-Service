using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Data.Model
{
    public class Path
    {
        [Key]
        public int PathId { get; set; }

        [Required]
        [ForeignKey("OriginId")]
        public Point Origin { get; set; }
        public int OriginId { get; set; }

        [Required]
        [ForeignKey("DestinyId")]
        public Point Destiny { get; set; }
        public int DestinyId { get; set; }
    }
}
