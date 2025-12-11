using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class Operatsioon
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Auto))]
        public int AutoId { get; set; }

        [Required]
        [ForeignKey(nameof(Tüüp))]
        public int TüüpId { get; set; }

        [Required]
        [ForeignKey(nameof(Töötaja))]
        public int TöötajaId { get; set; }

        [Required]
        public DateTime Kuupäev { get; set; } // проверка “не будущая” будет в FluentValidation

        [Required]
        [MaxLength(20)]
        public string Staatus { get; set; }

        [Precision(18, 2)]
        [Range(0, double.MaxValue)]
        public decimal Maksumus { get; set; }

        public Auto Auto { get; set; }
        public OperatsiooniTyyp Tüüp { get; set; }
        public Töötaja Töötaja { get; set; }
    }
}
