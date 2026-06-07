using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.Autos;
using Moq;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Autos
{
    public class DeleteAutoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<IAutoRepository>();
            var handler = new DeleteAutoCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.Remove(It.IsAny<Auto>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_WhenRequestIdIsZeroOrLess_ReturnsFalseAndDoesNotQueryRepository(int id)
        {
            var repo = new Mock<IAutoRepository>();
            var handler = new DeleteAutoCommandHandler(repo.Object);

            var result = await handler.Handle(new DeleteAutoCommand { Id = id }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.False(result.Value);

            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.Remove(It.IsAny<Auto>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenAutoDoesNotExist_ReturnsFalse()
        {
            var repo = new Mock<IAutoRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Auto)null);

            var handler = new DeleteAutoCommandHandler(repo.Object);

            var result = await handler.Handle(new DeleteAutoCommand { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.False(result.Value);

            repo.Verify(x => x.GetByIdAsync(99), Times.Once);
            repo.Verify(x => x.Remove(It.IsAny<Auto>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenAutoExists_RemovesEntityAndReturnsTrue()
        {
            var repo = new Mock<IAutoRepository>();

            var entity = new Auto
            {
                Id = 1,
                Tootja = "Toyota",
                Mudel = "Corolla",
                Numbrimark = "123ABC"
            };

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(entity);

            var handler = new DeleteAutoCommandHandler(repo.Object);

            var result = await handler.Handle(new DeleteAutoCommand { Id = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.True(result.Value);

            repo.Verify(x => x.GetByIdAsync(1), Times.Once);
            repo.Verify(x => x.Remove(entity), Times.Once);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}