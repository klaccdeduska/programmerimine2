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
            var result = new OperationResult<bool>();

            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null)
            {
                result.Errors.Add("Operatsioon not found");
                return result;
            }

            _repo.Remove(entity);          // ← тут тоже ТОЛЬКО так
            await _repo.SaveChangesAsync();

            result.Value = true;
            return result;
        }
    }
}
