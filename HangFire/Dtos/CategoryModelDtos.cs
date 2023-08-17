using System.ComponentModel.DataAnnotations;

namespace HangFire.Dtos
{
    public class CategoryModelDtos
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
