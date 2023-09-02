using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HangFire.Models
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(500)]
        public string? Tiltle { get; set; }    
        public int Year { get; set; }    
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string? StoreLine { get; set; }    
        public byte[]? Poster { get; set; }    
        public int CategoryId { get; set; }    
        public Category Category { get; set; }
    }
}
