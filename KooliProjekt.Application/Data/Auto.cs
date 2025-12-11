using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Auto : Entity
    {
        [Required]
        [MaxLength(100)]
        public string Tootja { get; set; }

        [Required]
        [MaxLength(100)]
        public string Mudel { get; set; }

        [Required]
        [MaxLength(15)]
        public string Numbrimark { get; set; }
    }
}
