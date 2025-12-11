using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class OperatsiooniTyyp : Entity
    {
        [Required]
        [MaxLength(100)]
        public string Nimi { get; set; }

        [MaxLength(255)]
        public string Kirjeldus { get; set; }
    }
}
