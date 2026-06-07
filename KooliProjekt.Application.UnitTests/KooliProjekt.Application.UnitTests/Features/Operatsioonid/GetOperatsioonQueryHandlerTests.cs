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
    public class GetOperatsioonQueryHandlerTests
    {
        [Fact]
        public async Task Handle_WhenRequestIsNull_ReturnsResultWithNullValue()
        {
            var repo = new Mock<IOperatsioonRepository>();
            var handler = new GetOperatsioonQueryHandler(repo.Object);

            var result = await handler.Handle(null, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Empty(result.Errors);
            repo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenOperatsioonExists_ReturnsOperatsioonDto()
        {
            var repo = new Mock<IOperatsioonRepository>();

            var date = new DateTime(2026, 1, 16);

            repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Operatsioon
                {
                    Id = 1,
                    AutoId = 2,
                    TöötajaId = 3,
                    TüüpId = 4,
                    Kuupäev = date,
                    Staatus = "Valmis",
                    Maksumus = 120.50m
                });

            var handler = new GetOperatsioonQueryHandler(repo.Object);

            var result = await handler.Handle(new GetOperatsioonQuery { Id = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal(2, result.Value.AutoId);
            Assert.Equal(3, result.Value.TöötajaId);
            Assert.Equal(4, result.Value.TüüpId);
            Assert.Equal(date, result.Value.Kuupäev);
            Assert.Equal("Valmis", result.Value.Staatus);
            Assert.Equal(120.50m, result.Value.Maksumus);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task Handle_WhenOperatsioonDoesNotExist_ReturnsResultWithNullValue()
        {
            var repo = new Mock<IOperatsioonRepository>();

            repo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Operatsioon)null);

            var handler = new GetOperatsioonQueryHandler(repo.Object);

            var result = await handler.Handle(new GetOperatsioonQuery { Id = 99 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Empty(result.Errors);
        }
    }
}