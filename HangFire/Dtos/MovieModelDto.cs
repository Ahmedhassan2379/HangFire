using System.ComponentModel.DataAnnotations;

namespace HangFire.Dtos
{
    public class MovieModelDto
    {
        [MaxLength(500)]
        public string? Tiltle { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string? StoreLine { get; set; }
        public int CategoryId { get; set; }
        public IFormFile Poster { get; set; }
    }
}
