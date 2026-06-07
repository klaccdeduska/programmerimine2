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
    public class SaveAutoCommandHandlerTests
    {
        [Fact]
        public async Task Save_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<IAutoRepository>();
            var handler = new SaveAutoCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.AddAsync(It.IsAny<Auto>()), Times.Never);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Save_WhenIdIsNegative_ThrowsArgumentException()
        {
            var repo = new Mock<IAutoRepository>();
            var handler = new SaveAutoCommandHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(new SaveAutoCommand { Id = -1 }, CancellationToken.None));

            repo.Verify(x => x.AddAsync(It.IsAny<Auto>()), Times.Never);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Save_WhenIdIsZero_AddsNewAuto()
        {
            var repo = new Mock<IAutoRepository>();

            repo.Setup(x => x.AddAsync(It.IsAny<Auto>()))
                .Returns(Task.CompletedTask);

            repo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new SaveAutoCommandHandler(repo.Object);

            var command = new SaveAutoCommand
            {
                Id = 0,
                Tootja = "Toyota",
                Mudel = "Corolla",
                Numbrimark = "123ABC"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal("Toyota", result.Value.Tootja);
            Assert.Equal("Corolla", result.Value.Mudel);
            Assert.Equal("123ABC", result.Value.Numbrimark);

            repo.Verify(x => x.AddAsync(It.IsAny<Auto>()), Times.Once);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Save_WhenAutoExists_UpdatesAuto()
        {
            var repo = new Mock<IAutoRepository>();

            var entity = new Auto
            {
                Id = 1,
                Tootja = "Old",
                Mudel = "Old",
                Numbrimark = "OLD"
            };

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(entity);

            repo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new SaveAutoCommandHandler(repo.Object);

            var command = new SaveAutoCommand
            {
                Id = 1,
                Tootja = "BMW",
                Mudel = "X5",
                Numbrimark = "456DEF"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal("BMW", result.Value.Tootja);
            Assert.Equal("X5", result.Value.Mudel);
            Assert.Equal("456DEF", result.Value.Numbrimark);

            repo.Verify(x => x.GetByIdAsync(1), Times.Once);
            repo.Verify(x => x.AddAsync(It.IsAny<Auto>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Save_WhenAutoDoesNotExist_ReturnsNullValue()
        {
            var repo = new Mock<IAutoRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Auto)null);

            var handler = new SaveAutoCommandHandler(repo.Object);

            var result = await handler.Handle(new SaveAutoCommand { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);

            repo.Verify(x => x.GetByIdAsync(99), Times.Once);
            repo.Verify(x => x.AddAsync(It.IsAny<Auto>()), Times.Never);
            repo.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}