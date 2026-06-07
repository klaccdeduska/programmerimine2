using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.OperatsiooniTüübid;
using Moq;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.OperatsiooniTüübid
{
    public class GetOperatsiooniTyypQueryHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestIsNull_ReturnsResultWithNullValue()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new GetOperatsiooniTyypQueryHandler(repo.Object);

            var result = await handler.Handle(null, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Empty(result.Errors);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenTypeExists_ReturnsOperatsiooniTyypDto()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new OperatsiooniTyyp
                {
                    Id = 1,
                    Nimi = "Õlivahetus",
                    Kirjeldus = "Mootoriõli vahetus"
                });

            var handler = new GetOperatsiooniTyypQueryHandler(repo.Object);

            var result = await handler.Handle(new GetOperatsiooniTyypQuery { Id = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal("Õlivahetus", result.Value.Nimi);
            Assert.Equal("Mootoriõli vahetus", result.Value.Kirjeldus);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task Handle_WhenTypeDoesNotExist_ReturnsResultWithNullValue()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((OperatsiooniTyyp)null);

            var handler = new GetOperatsiooniTyypQueryHandler(repo.Object);

            var result = await handler.Handle(new GetOperatsiooniTyypQuery { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Empty(result.Errors);
        }
    }
}