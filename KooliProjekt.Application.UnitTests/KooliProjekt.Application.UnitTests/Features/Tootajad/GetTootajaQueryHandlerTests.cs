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
        public async Task Handle_WhenRequestIsNull_ReturnsResultWithNullValue()
        {
            var repo = new Mock<ITootajaRepository>();
            var handler = new GetTootajaQueryHandler(repo.Object);

            var result = await handler.Handle(null, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Empty(result.Errors);
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
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal("Mati Maasikas", result.Value.Nimi);
            Assert.Equal("mati@mail.com", result.Value.Email);
            Assert.Equal("Mehaanik", result.Value.Roll);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task Handle_WhenTootajaDoesNotExist_ReturnsResultWithNullValue()
        {
            var repo = new Mock<ITootajaRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Töötaja)null);

            var handler = new GetTootajaQueryHandler(repo.Object);

            var result = await handler.Handle(new GetTootajaQuery { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Empty(result.Errors);
        }
    }
}