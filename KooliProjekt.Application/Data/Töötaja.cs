using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Töötaja
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nimi { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Roll { get; set; }
    }
}
