using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.Operatsioonid;
using Moq;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Operatsioonid
{
    public class DeleteOperatsioonCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<IOperatsioonRepository>();
            var handler = new DeleteOperatsioonCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.Remove(It.IsAny<Operatsioon>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_WhenRequestIdIsZeroOrLess_ReturnsFalseAndDoesNotQueryRepository(int id)
        {
            var repo = new Mock<IOperatsioonRepository>();
            var handler = new DeleteOperatsioonCommandHandler(repo.Object);

            var result = await handler.Handle(new DeleteOperatsioonCommand { Id = id }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.False(result.Value);

            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.Remove(It.IsAny<Operatsioon>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenOperatsioonDoesNotExist_ReturnsFalse()
        {
            var repo = new Mock<IOperatsioonRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Operatsioon)null);

            var handler = new DeleteOperatsioonCommandHandler(repo.Object);

            var result = await handler.Handle(new DeleteOperatsioonCommand { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.False(result.Value);

            repo.Verify(x => x.GetByIdAsync(99), Times.Once);
            repo.Verify(x => x.Remove(It.IsAny<Operatsioon>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenOperatsioonExists_RemovesEntityAndReturnsTrue()
        {
            var repo = new Mock<IOperatsioonRepository>();

            var entity = new Operatsioon
            {
                Id = 1,
                AutoId = 2,
                TöötajaId = 3,
                TüüpId = 4,
                Kuupäev = new DateTime(2026, 1, 23),
                Staatus = "Valmis",
                Maksumus = 100m
            };

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(entity);

            var handler = new DeleteOperatsioonCommandHandler(repo.Object);

            var result = await handler.Handle(new DeleteOperatsioonCommand { Id = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.True(result.Value);

            repo.Verify(x => x.GetByIdAsync(1), Times.Once);
            repo.Verify(x => x.Remove(entity), Times.Once);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}