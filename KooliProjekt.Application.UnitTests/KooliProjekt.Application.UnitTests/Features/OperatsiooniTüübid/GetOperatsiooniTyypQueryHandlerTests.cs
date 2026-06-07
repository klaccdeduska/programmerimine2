using System;
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
        public async Task Handle_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new GetOperatsiooniTyypQueryHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_WhenRequestIdIsZeroOrLess_ReturnsNullAndDoesNotQueryRepository(int id)
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new GetOperatsiooniTyypQueryHandler(repo.Object);

            var result = await handler.Handle(new GetOperatsiooniTyypQuery { Id = id }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);

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
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal("Õlivahetus", result.Value.Nimi);
            Assert.Equal("Mootoriõli vahetus", result.Value.Kirjeldus);

            repo.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenTypeDoesNotExist_ReturnsNullValue()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((OperatsiooniTyyp)null);

            var handler = new GetOperatsiooniTyypQueryHandler(repo.Object);

            var result = await handler.Handle(new GetOperatsiooniTyypQuery { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);

            repo.Verify(x => x.GetByIdAsync(99), Times.Once);
        }
    }
}