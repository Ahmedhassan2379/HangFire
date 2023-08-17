using System.ComponentModel.DataAnnotations;

namespace HangFire.Dtos
{
    public class MovieModelDto
    {
        public int Id { get; set; }
        [MaxLength(500)]
        public string? Tiltle { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string? StoreLine { get; set; }
        public int CategoryId { get; set; }
        public byte[] Poster { get; set; }
        public string CategoryName { get; set; }
    }
}
