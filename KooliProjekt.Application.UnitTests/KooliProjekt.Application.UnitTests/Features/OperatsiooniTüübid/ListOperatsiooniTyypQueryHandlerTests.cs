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
    public class ListOperatsiooniTyypQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task Handle_WhenSearchIsProvided_ReturnsMatchingOperatsiooniTyybid()
        {
            using var dbContext = GetDbContext();

            await dbContext.OperatsiooniTüübid.AddAsync(new OperatsiooniTyyp
            {
                Nimi = "Õlivahetus",
                Kirjeldus = "Mootoriõli vahetus"
            });

            await dbContext.OperatsiooniTüübid.AddAsync(new OperatsiooniTyyp
            {
                Nimi = "Rehvide vahetus",
                Kirjeldus = "Rehvide vahetus ja tasakaalustamine"
            });

            await dbContext.SaveChangesAsync();

            var repo = new OperatsiooniTyypRepository(dbContext);
            var handler = new ListOperatsiooniTyypQueryHandler(repo);

            var result = await handler.Handle(new ListOperatsiooniTyypQuery
            {
                Page = 1,
                PageSize = 10,
                Search = "rehvide"
            }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);

            var item = Assert.Single(result.Value.Results);
            Assert.Equal("Rehvide vahetus", item.Nimi);
        }
        [Fact]
        public async Task Handle_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new ListOperatsiooniTyypQueryHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));

            repo.Verify(x => x.Query(), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_WhenPageIsZeroOrLess_ThrowsArgumentException(int page)
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new ListOperatsiooniTyypQueryHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(new ListOperatsiooniTyypQuery
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
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new ListOperatsiooniTyypQueryHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(new ListOperatsiooniTyypQuery
                {
                    Page = 1,
                    PageSize = pageSize
                }, CancellationToken.None));

            repo.Verify(x => x.Query(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPageSizeIsTooLarge_ThrowsArgumentException()
        {
            var repo = new Mock<IOperatsiooniTyypRepository>();
            var handler = new ListOperatsiooniTyypQueryHandler(repo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(new ListOperatsiooniTyypQuery
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

            await dbContext.OperatsiooniTüübid.AddAsync(new OperatsiooniTyyp
            {
                Nimi = "Õlivahetus",
                Kirjeldus = "Mootoriõli vahetus"
            });

            await dbContext.OperatsiooniTüübid.AddAsync(new OperatsiooniTyyp
            {
                Nimi = "Rehvide vahetus",
                Kirjeldus = "Rehvide vahetus ja tasakaalustamine"
            });

            await dbContext.SaveChangesAsync();

            var repo = new OperatsiooniTyypRepository(dbContext);
            var handler = new ListOperatsiooniTyypQueryHandler(repo);

            var result = await handler.Handle(new ListOperatsiooniTyypQuery
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