using System.ComponentModel.DataAnnotations;

namespace HangFire.Dtos
{
    public class MovieModelDto
    {
        [MaxLength(500)]
        public string? Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string? StoreLine { get; set; }
        public int CategoryId { get; set; }
        public byte[]? Poster { get; set; } = null;
        public string CategoryName { get; set; }
    }
}
