using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Autos
{
    public class SaveAutoCommandHandler :
        IRequestHandler<SaveAutoCommand, OperationResult<Auto>>
    {
        private readonly IAutoRepository _repo;

        public SaveAutoCommandHandler(IAutoRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<Auto>> Handle(SaveAutoCommand request, CancellationToken ct)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Id < 0)
            {
                throw new ArgumentException("Id cannot be negative.", nameof(request.Id));
            }

            var result = new OperationResult<Auto>();
            Auto entity;

            if (request.Id == 0)
            {
                entity = new Auto();
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

            entity.Tootja = request.Tootja;
            entity.Mudel = request.Mudel;
            entity.Numbrimark = request.Numbrimark;

            await _repo.SaveChangesAsync();

            result.Value = entity;
            return result;
        }
    }
}