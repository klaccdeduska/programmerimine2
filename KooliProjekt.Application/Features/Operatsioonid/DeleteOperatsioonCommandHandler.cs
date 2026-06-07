using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class DeleteOperatsioonCommandHandler :
        IRequestHandler<DeleteOperatsioonCommand, OperationResult<bool>>
    {
        private readonly IOperatsioonRepository _repo;

        public DeleteOperatsioonCommandHandler(IOperatsioonRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<bool>> Handle(DeleteOperatsioonCommand request, CancellationToken ct)
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