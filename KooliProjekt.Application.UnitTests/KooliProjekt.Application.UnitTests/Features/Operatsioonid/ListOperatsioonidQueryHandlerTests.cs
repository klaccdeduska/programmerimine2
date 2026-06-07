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
    public class ListOperatsioonidQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task Handle_WhenSearchIsProvided_ReturnsMatchingOperatsioonid()
        {
            using var dbContext = GetDbContext();

            var auto = new Auto
            {
                Tootja = "Toyota",
                Mudel = "Corolla",
                Numbrimark = "123ABC"
            };

            var tootaja = new Töötaja
            {
                Nimi = "Mati Maasikas",
                Email = "mati@mail.com",
                Roll = "Mehaanik"
            };

            var tyyp = new OperatsiooniTyyp
            {
                Nimi = "Õlivahetus",
                Kirjeldus = "Mootoriõli vahetus"
            };

            await dbContext.Autos.AddAsync(auto);
            await dbContext.Töötajad.AddAsync(tootaja);
            await dbContext.OperatsiooniTüübid.AddAsync(tyyp);
            await dbContext.SaveChangesAsync();

            await dbContext.Operatsioonid.AddAsync(new Operatsioon
            {
                AutoId = auto.Id,
                TöötajaId = tootaja.Id,
                TüüpId = tyyp.Id,
                Kuupäev = new DateTime(2026, 2, 12),
                Staatus = "Valmis",
                Maksumus = 100m
            });

            await dbContext.Operatsioonid.AddAsync(new Operatsioon
            {
                AutoId = auto.Id,
                TöötajaId = tootaja.Id,
                TüüpId = tyyp.Id,
                Kuupäev = new DateTime(2026, 2, 13),
                Staatus = "Ootel",
                Maksumus = 50m
            });

            await dbContext.SaveChangesAsync();

            var repo = new OperatsioonRepository(dbContext);
            var handler = new ListOperatsioonidQueryHandler(repo);

            var result = await handler.Handle(new ListOperatsioonidQuery
            {
                Page = 1,
                PageSize = 10,
                Search = "valmis"
            }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);

            var item = Assert.Single(result.Value.Results);
            Assert.Equal("Valmis", item.Staatus);
        }
        [Fact]
        public async Task Handle_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<IOperatsioonRepository>();
            var handler = new ListOperatsioonidQueryHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.Query(), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_WhenPageIsZeroOrLess_ThrowsArgumentException(int page)
        {
            var repo = new Mock<IOperatsioonRepository>();
            var handler = new ListOperatsioonidQueryHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(new ListOperatsioonidQuery
                {
                    Page = page,
                    PageSize = 10
                }, CancellationToken.None));

            repo.Verify(x => x.Query(), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_WhenPageSizeIsZeroOrLess_ThrowsArgumentException(int pageSize)
        {
            var repo = new Mock<IOperatsioonRepository>();
            var handler = new ListOperatsioonidQueryHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(new ListOperatsioonidQuery
                {
                    Page = 1,
                    PageSize = pageSize
                }, CancellationToken.None));

            repo.Verify(x => x.Query(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPageSizeIsTooLarge_ThrowsArgumentException()
        {
            var repo = new Mock<IOperatsioonRepository>();
            var handler = new ListOperatsioonidQueryHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(new ListOperatsioonidQuery
                {
                    Page = 1,
                    PageSize = 101
                }, CancellationToken.None));

            repo.Verify(x => x.Query(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRequestIsValid_ReturnsPagedResult()
        {
            using var dbContext = GetDbContext();

            var auto = new Auto
            {
                Tootja = "Toyota",
                Mudel = "Corolla",
                Numbrimark = "123ABC"
            };

            var tootaja = new Töötaja
            {
                Nimi = "Mati Maasikas",
                Email = "mati@mail.com",
                Roll = "Mehaanik"
            };

            var tyyp = new OperatsiooniTyyp
            {
                Nimi = "Õlivahetus",
                Kirjeldus = "Mootoriõli vahetus"
            };

            await dbContext.Autos.AddAsync(auto);
            await dbContext.Töötajad.AddAsync(tootaja);
            await dbContext.OperatsiooniTüübid.AddAsync(tyyp);
            await dbContext.SaveChangesAsync();

            await dbContext.Operatsioonid.AddAsync(new Operatsioon
            {
                AutoId = auto.Id,
                TöötajaId = tootaja.Id,
                TüüpId = tyyp.Id,
                Kuupäev = new DateTime(2026, 1, 16),
                Staatus = "Valmis",
                Maksumus = 100m
            });

            await dbContext.Operatsioonid.AddAsync(new Operatsioon
            {
                AutoId = auto.Id,
                TöötajaId = tootaja.Id,
                TüüpId = tyyp.Id,
                Kuupäev = new DateTime(2026, 1, 17),
                Staatus = "Ootel",
                Maksumus = 50m
            });

            await dbContext.SaveChangesAsync();

            var repo = new OperatsioonRepository(dbContext);
            var handler = new ListOperatsioonidQueryHandler(repo);

            var result = await handler.Handle(new ListOperatsioonidQuery
            {
                Page = 1,
                PageSize = 10
            }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Results.Count);
            Assert.Equal(1, result.Value.CurrentPage);
            Assert.Equal(10, result.Value.PageSize);
            Assert.Equal(2, result.Value.RowCount);
        }
    }
}