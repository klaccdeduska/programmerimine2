using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class DeleteOperatsiooniTyypCommandHandler :
        IRequestHandler<DeleteOperatsiooniTyypCommand, OperationResult<bool>>
    {
        private readonly IOperatsiooniTyypRepository _repo;

        public DeleteOperatsiooniTyypCommandHandler(IOperatsiooniTyypRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<bool>> Handle(DeleteOperatsiooniTyypCommand request, CancellationToken ct)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<bool>();

            if (request.Id <= 0)
            {
                return result;
            }

            var entity = await _repo.GetByIdAsync(request.Id);

            if (entity == null)
            {
                return result;
            }

            _repo.Remove(entity);
            await _repo.SaveChangesAsync();

            result.Value = true;
            return result;
        }
    }
}