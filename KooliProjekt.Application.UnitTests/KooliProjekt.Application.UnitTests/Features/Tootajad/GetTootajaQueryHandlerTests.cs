using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.Tootajad;
using Moq;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Tootajad
{
    public class GetTootajaQueryHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<ITootajaRepository>();
            var handler = new GetTootajaQueryHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_WhenRequestIdIsZeroOrLess_ReturnsNullAndDoesNotQueryRepository(int id)
        {
            var repo = new Mock<ITootajaRepository>();
            var handler = new GetTootajaQueryHandler(repo.Object);

            var result = await handler.Handle(new GetTootajaQuery { Id = id }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);

            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenTootajaExists_ReturnsTootajaDto()
        {
            var repo = new Mock<ITootajaRepository>();

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Töötaja
                {
                    Id = 1,
                    Nimi = "Mati Maasikas",
                    Email = "mati@mail.com",
                    Roll = "Mehaanik"
                });

            var handler = new GetTootajaQueryHandler(repo.Object);

            var result = await handler.Handle(new GetTootajaQuery { Id = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal("Mati Maasikas", result.Value.Nimi);
            Assert.Equal("mati@mail.com", result.Value.Email);
            Assert.Equal("Mehaanik", result.Value.Roll);

            repo.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenTootajaDoesNotExist_ReturnsNullValue()
        {
            var repo = new Mock<ITootajaRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Töötaja)null);

            var handler = new GetTootajaQueryHandler(repo.Object);

            var result = await handler.Handle(new GetTootajaQuery { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);

            repo.Verify(x => x.GetByIdAsync(99), Times.Once);
        }
    }
}