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
    public class SaveTootajaCommandHandlerTests
    {
        [Fact]
        public async Task Save_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<ITootajaRepository>();
            var handler = new SaveTootajaCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.AddAsync(It.IsAny<Töötaja>()), Times.Never);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Save_WhenIdIsNegative_ThrowsArgumentException()
        {
            var repo = new Mock<ITootajaRepository>();
            var handler = new SaveTootajaCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(new SaveTootajaCommand { Id = -1 }, CancellationToken.None));

            repo.Verify(x => x.AddAsync(It.IsAny<Töötaja>()), Times.Never);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Save_WhenIdIsZero_AddsNewTootaja()
        {
            var repo = new Mock<ITootajaRepository>();

            repo.Setup(x => x.AddAsync(It.IsAny<Töötaja>()))
                .Returns(Task.CompletedTask);

            repo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new SaveTootajaCommandHandler(repo.Object);

            var command = new SaveTootajaCommand
            {
                Id = 0,
                Nimi = "Mati Maasikas",
                Email = "mati@mail.com",
                Roll = "Mehaanik"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal("Mati Maasikas", result.Value.Nimi);
            Assert.Equal("mati@mail.com", result.Value.Email);
            Assert.Equal("Mehaanik", result.Value.Roll);

            repo.Verify(x => x.AddAsync(It.IsAny<Töötaja>()), Times.Once);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Save_WhenTootajaExists_UpdatesTootaja()
        {
            var repo = new Mock<ITootajaRepository>();

            var entity = new Töötaja
            {
                Id = 1,
                Nimi = "Old",
                Email = "old@mail.com",
                Roll = "Old"
            };

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(entity);

            repo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new SaveTootajaCommandHandler(repo.Object);

            var command = new SaveTootajaCommand
            {
                Id = 1,
                Nimi = "Kati Kuusk",
                Email = "kati@mail.com",
                Roll = "Admin"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal("Kati Kuusk", result.Value.Nimi);
            Assert.Equal("kati@mail.com", result.Value.Email);
            Assert.Equal("Admin", result.Value.Roll);

            repo.Verify(x => x.GetByIdAsync(1), Times.Once);
            repo.Verify(x => x.AddAsync(It.IsAny<Töötaja>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Save_WhenTootajaDoesNotExist_ReturnsNullValue()
        {
            var repo = new Mock<ITootajaRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Töötaja)null);

            var handler = new SaveTootajaCommandHandler(repo.Object);

            var result = await handler.Handle(new SaveTootajaCommand { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);

            repo.Verify(x => x.GetByIdAsync(99), Times.Once);
            repo.Verify(x => x.AddAsync(It.IsAny<Töötaja>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}