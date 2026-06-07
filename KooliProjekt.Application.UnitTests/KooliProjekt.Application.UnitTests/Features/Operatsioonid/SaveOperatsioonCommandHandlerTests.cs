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
    public class SaveOperatsioonCommandHandlerTests
    {
        [Fact]
        public async Task Save_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<IOperatsioonRepository>();
            var handler = new SaveOperatsioonCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.AddAsync(It.IsAny<Operatsioon>()), Times.Never);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Save_WhenIdIsNegative_ThrowsArgumentException()
        {
            var repo = new Mock<IOperatsioonRepository>();
            var handler = new SaveOperatsioonCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(new SaveOperatsioonCommand { Id = -1 }, CancellationToken.None));

            repo.Verify(x => x.AddAsync(It.IsAny<Operatsioon>()), Times.Never);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Save_WhenIdIsZero_AddsNewOperatsioon()
        {
            var repo = new Mock<IOperatsioonRepository>();

            repo.Setup(x => x.AddAsync(It.IsAny<Operatsioon>()))
                .Returns(Task.CompletedTask);

            repo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new SaveOperatsioonCommandHandler(repo.Object);

            var date = new DateTime(2026, 2, 5);

            var command = new SaveOperatsioonCommand
            {
                Id = 0,
                AutoId = 1,
                TöötajaId = 2,
                TüüpId = 3,
                Kuupäev = date,
                Staatus = "Valmis",
                Maksumus = 100m
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.AutoId);
            Assert.Equal(2, result.Value.TöötajaId);
            Assert.Equal(3, result.Value.TüüpId);
            Assert.Equal(date, result.Value.Kuupäev);
            Assert.Equal("Valmis", result.Value.Staatus);
            Assert.Equal(100m, result.Value.Maksumus);

            repo.Verify(x => x.AddAsync(It.IsAny<Operatsioon>()), Times.Once);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Save_WhenOperatsioonExists_UpdatesOperatsioon()
        {
            var repo = new Mock<IOperatsioonRepository>();

            var entity = new Operatsioon
            {
                Id = 1,
                AutoId = 10,
                TöötajaId = 20,
                TüüpId = 30,
                Kuupäev = new DateTime(2026, 1, 1),
                Staatus = "Old",
                Maksumus = 10m
            };

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(entity);

            repo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new SaveOperatsioonCommandHandler(repo.Object);

            var date = new DateTime(2026, 2, 5);

            var command = new SaveOperatsioonCommand
            {
                Id = 1,
                AutoId = 2,
                TöötajaId = 3,
                TüüpId = 4,
                Kuupäev = date,
                Staatus = "Valmis",
                Maksumus = 120.50m
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal(2, result.Value.AutoId);
            Assert.Equal(3, result.Value.TöötajaId);
            Assert.Equal(4, result.Value.TüüpId);
            Assert.Equal(date, result.Value.Kuupäev);
            Assert.Equal("Valmis", result.Value.Staatus);
            Assert.Equal(120.50m, result.Value.Maksumus);

            repo.Verify(x => x.GetByIdAsync(1), Times.Once);
            repo.Verify(x => x.AddAsync(It.IsAny<Operatsioon>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Save_WhenOperatsioonDoesNotExist_ReturnsNullValue()
        {
            var repo = new Mock<IOperatsioonRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Operatsioon)null);

            var handler = new SaveOperatsioonCommandHandler(repo.Object);

            var result = await handler.Handle(new SaveOperatsioonCommand { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);

            repo.Verify(x => x.GetByIdAsync(99), Times.Once);
            repo.Verify(x => x.AddAsync(It.IsAny<Operatsioon>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}