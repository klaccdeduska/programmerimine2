using System;

namespace KooliProjekt.Application.Dto
{
    public class OperatsioonDto
    {
        public int Id { get; set; }
        public int AutoId { get; set; }
        public int TöötajaId { get; set; }
        public int TüüpId { get; set; }
        public DateTime Kuupäev { get; set; }
        public string Staatus { get; set; }
        public decimal? Maksumus { get; set; }
    }
}