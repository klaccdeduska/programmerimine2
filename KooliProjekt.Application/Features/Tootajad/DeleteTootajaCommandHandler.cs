using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class DeleteTootajaCommandHandler :
        IRequestHandler<DeleteTootajaCommand, OperationResult<bool>>
    {
        private readonly ITootajaRepository _repo;

        public DeleteTootajaCommandHandler(ITootajaRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<bool>> Handle(DeleteTootajaCommand request, CancellationToken ct)
        {
            var result = new OperationResult<bool>();

            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null)
            {
                result.Errors.Add("Töötaja not found");
                return result;
            }

            _repo.Remove(entity);
            await _repo.SaveChangesAsync();

            result.Value = true;
            return result;
        }
    }
}
