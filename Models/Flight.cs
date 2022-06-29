using System.ComponentModel.DataAnnotations;

namespace AirportApplication.Models
{
    public class Flight
    { 
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Origin { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Destination { get; set; }
    }
}
