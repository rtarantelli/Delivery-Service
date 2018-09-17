using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Data.Model
{
    public class Path
    {
        [Key]
        public int PathId { get; set; }

        [NotMapped]
        public int OriginId { get; set; }

        [Required]
        [ForeignKey("OriginId")]
        public Point Origin { get; set; }

        [NotMapped]
        public int DestinyId { get; set; }

        [Required]
        [ForeignKey("DestinyId")]
        public Point Destiny { get; set; }

        [Required]
        public int Cost { get; set; }

        [Required]
        public int Time { get; set; }
    }
}
