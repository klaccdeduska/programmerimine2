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
    public class SaveOperatsiooniTyypCommandHandlerTests
    {
        [Fact]
        public async Task Save_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new SaveOperatsiooniTyypCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.AddAsync(It.IsAny<OperatsiooniTyyp>()), Times.Never);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Save_WhenIdIsNegative_ThrowsArgumentException()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new SaveOperatsiooniTyypCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(new SaveOperatsiooniTyypCommand { Id = -1 }, CancellationToken.None));

            repo.Verify(x => x.AddAsync(It.IsAny<OperatsiooniTyyp>()), Times.Never);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Save_WhenIdIsZero_AddsNewOperatsiooniTyyp()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();

            repo.Setup(x => x.AddAsync(It.IsAny<OperatsiooniTyyp>()))
                .Returns(Task.CompletedTask);

            repo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new SaveOperatsiooniTyypCommandHandler(repo.Object);

            var command = new SaveOperatsiooniTyypCommand
            {
                Id = 0,
                Nimi = "Õlivahetus",
                Kirjeldus = "Mootoriõli vahetus"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal("Õlivahetus", result.Value.Nimi);
            Assert.Equal("Mootoriõli vahetus", result.Value.Kirjeldus);

            repo.Verify(x => x.AddAsync(It.IsAny<OperatsiooniTyyp>()), Times.Once);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Save_WhenOperatsiooniTyypExists_UpdatesOperatsiooniTyyp()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();

            var entity = new OperatsiooniTyyp
            {
                Id = 1,
                Nimi = "Old",
                Kirjeldus = "Old"
            };

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(entity);

            repo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new SaveOperatsiooniTyypCommandHandler(repo.Object);

            var command = new SaveOperatsiooniTyypCommand
            {
                Id = 1,
                Nimi = "Rehvide vahetus",
                Kirjeldus = "Rehvide vahetus ja tasakaalustamine"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal("Rehvide vahetus", result.Value.Nimi);
            Assert.Equal("Rehvide vahetus ja tasakaalustamine", result.Value.Kirjeldus);

            repo.Verify(x => x.GetByIdAsync(1), Times.Once);
            repo.Verify(x => x.AddAsync(It.IsAny<OperatsiooniTyyp>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Save_WhenOperatsiooniTyypDoesNotExist_ReturnsNullValue()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((OperatsiooniTyyp)null);

            var handler = new SaveOperatsiooniTyypCommandHandler(repo.Object);

            var result = await handler.Handle(new SaveOperatsiooniTyypCommand { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);

            repo.Verify(x => x.GetByIdAsync(99), Times.Once);
            repo.Verify(x => x.AddAsync(It.IsAny<OperatsiooniTyyp>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}