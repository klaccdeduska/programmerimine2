using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class OperatsiooniTyyp
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nimi { get; set; } // уникальный

        [MaxLength(255)]
        public string Kirjeldus { get; set; }
    }
}

