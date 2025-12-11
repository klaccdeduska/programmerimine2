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
            var result = new OperationResult<bool>();

            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null)
            {
                result.Errors.Add("Operatsiooni tüüp not found");
                return result;
            }

            _repo.Remove(entity);
            await _repo.SaveChangesAsync();

            result.Value = true;
            return result;
        }
    }
}
