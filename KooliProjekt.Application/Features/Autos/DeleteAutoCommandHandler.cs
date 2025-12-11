using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Autos
{
    public class DeleteAutoCommandHandler :
        IRequestHandler<DeleteAutoCommand, OperationResult<bool>>
    {
        private readonly IAutoRepository _repo;

        public DeleteAutoCommandHandler(IAutoRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<bool>> Handle(DeleteAutoCommand request, CancellationToken ct)
        {
            var result = new OperationResult<bool>();

            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null)
            {
                result.Errors.Add("Auto not found");
                return result;
            }

            _repo.Remove(entity);          // ВАЖНО: именно _repo.Remove
            await _repo.SaveChangesAsync();

            result.Value = true;
            return result;
        }
    }
}
