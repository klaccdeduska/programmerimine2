using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class SaveOperatsiooniTyypCommandHandler :
        IRequestHandler<SaveOperatsiooniTyypCommand, OperationResult<OperatsiooniTyyp>>
    {
        private readonly IOperatsiooniTyypRepository _repo;

        public SaveOperatsiooniTyypCommandHandler(IOperatsiooniTyypRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<OperatsiooniTyyp>> Handle(SaveOperatsiooniTyypCommand request, CancellationToken ct)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Id < 0)
            {
                throw new ArgumentException("Id cannot be negative.", nameof(request.Id));
            }

            var result = new OperationResult<OperatsiooniTyyp>();
            OperatsiooniTyyp entity;

            if (request.Id == 0)
            {
                entity = new OperatsiooniTyyp();
                await _repo.AddAsync(entity);
            }
            else
            {
                entity = await _repo.GetByIdAsync(request.Id);

                if (entity == null)
                {
                    return result;
                }
            }

            entity.Nimi = request.Nimi;
            entity.Kirjeldus = request.Kirjeldus;

            await _repo.SaveChangesAsync();

            result.Value = entity;
            return result;
        }
    }
}