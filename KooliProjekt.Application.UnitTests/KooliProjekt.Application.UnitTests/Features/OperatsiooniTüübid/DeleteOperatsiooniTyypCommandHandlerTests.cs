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
    public class DeleteOperatsiooniTyypCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new DeleteOperatsiooniTyypCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.Remove(It.IsAny<OperatsiooniTyyp>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_WhenRequestIdIsZeroOrLess_ReturnsFalseAndDoesNotQueryRepository(int id)
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new DeleteOperatsiooniTyypCommandHandler(repo.Object);

            var result = await handler.Handle(new DeleteOperatsiooniTyypCommand { Id = id }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.False(result.Value);

            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.Remove(It.IsAny<OperatsiooniTyyp>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenOperatsiooniTyypDoesNotExist_ReturnsFalse()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((OperatsiooniTyyp)null);

            var handler = new DeleteOperatsiooniTyypCommandHandler(repo.Object);

            var result = await handler.Handle(new DeleteOperatsiooniTyypCommand { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.False(result.Value);

            repo.Verify(x => x.GetByIdAsync(99), Times.Once);
            repo.Verify(x => x.Remove(It.IsAny<OperatsiooniTyyp>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenOperatsiooniTyypExists_RemovesEntityAndReturnsTrue()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();

            var entity = new OperatsiooniTyyp
            {
                Id = 1,
                Nimi = "Õlivahetus",
                Kirjeldus = "Mootoriõli vahetus"
            };

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(entity);

            var handler = new DeleteOperatsiooniTyypCommandHandler(repo.Object);

            var result = await handler.Handle(new DeleteOperatsiooniTyypCommand { Id = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.True(result.Value);

            repo.Verify(x => x.GetByIdAsync(1), Times.Once);
            repo.Verify(x => x.Remove(entity), Times.Once);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}