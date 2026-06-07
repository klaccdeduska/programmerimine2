using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.Autos;
using Moq;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Autos
{
    public class GetAutoQueryHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestIsNull_ReturnsResultWithNullValue()
        {
            var repo = new Mock<IAutoRepository>();
            var handler = new GetAutoQueryHandler(repo.Object);

            var result = await handler.Handle(null, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Empty(result.Errors);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenAutoExists_ReturnsAutoDto()
        {
            var repo = new Mock<IAutoRepository>();

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Auto
                {
                    Id = 1,
                    Tootja = "Toyota",
                    Mudel = "Corolla",
                    Numbrimark = "123ABC"
                });

            var handler = new GetAutoQueryHandler(repo.Object);

            var result = await handler.Handle(new GetAutoQuery { Id = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal("Toyota", result.Value.Tootja);
            Assert.Equal("Corolla", result.Value.Mudel);
            Assert.Equal("123ABC", result.Value.Numbrimark);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task Handle_WhenAutoDoesNotExist_ReturnsResultWithNullValue()
        {
            var repo = new Mock<IAutoRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Auto)null);

            var handler = new GetAutoQueryHandler(repo.Object);

            var result = await handler.Handle(new GetAutoQuery { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Empty(result.Errors);
        }
    }
}