using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class SaveOperatsioonCommand : IRequest<OperationResult<Operatsioon>>
    {
        public int Id { get; set; }
        public int AutoId { get; set; }
        public int TüüpId { get; set; }
        public int TöötajaId { get; set; }
        public DateTime Kuupäev { get; set; }
        public string Staatus { get; set; }
        public decimal Maksumus { get; set; }
    }
}
