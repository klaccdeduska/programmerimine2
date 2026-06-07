using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.OperatsiooniTüübid;
using KooliProjekt.Application.Infrastructure.Paging;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests.Controllers
{
    public class OperatsiooniTyypControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {
        public OperatsiooniTyypControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Save_should_create_operatsiooni_tyyp()
        {
            var command = new SaveOperatsiooniTyypCommand
            {
                Id = 0,
                Nimi = "Pidurite remont",
                Kirjeldus = "Pidurisüsteemi remont"
            };

            var response = await Client.PostAsJsonAsync("/api/OperatsiooniTyyp", command);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<OperatsiooniTyyp>(JsonOptions);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("Pidurite remont", result.Nimi);
            Assert.Equal("Pidurisüsteemi remont", result.Kirjeldus);
        }

        [Fact]
        public async Task Save_should_update_operatsiooni_tyyp()
        {
            var id = await AddOperatsiooniTyypAsync("Old", "Old");

            var command = new SaveOperatsiooniTyypCommand
            {
                Id = id,
                Nimi = "Rehvide vahetus",
                Kirjeldus = "Rehvide vahetus ja tasakaalustamine"
            };

            var response = await Client.PostAsJsonAsync("/api/OperatsiooniTyyp", command);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<OperatsiooniTyyp>(JsonOptions);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Rehvide vahetus", result.Nimi);
            Assert.Equal("Rehvide vahetus ja tasakaalustamine", result.Kirjeldus);
        }

        [Fact]
        public async Task Delete_should_delete_operatsiooni_tyyp()
        {
            var id = await AddOperatsiooniTyypAsync("Delete type", "Delete description");

            var response = await Client.DeleteAsync($"/api/OperatsiooniTyyp/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<bool>(JsonOptions);

            Assert.True(result);

            var entity = await ExecuteDbAsync(db => db.OperatsiooniTüübid.FindAsync(id).AsTask());

            Assert.Null(entity);
        }
        [Fact]
        public async Task List_should_return_operatsiooni_tyybid()
        {
            await AddOperatsiooniTyypAsync("Õlivahetus", "Mootoriõli vahetus");

            var response = await Client.GetAsync("/api/OperatsiooniTyyp?Page=1&PageSize=10");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<PagedResult<OperatsiooniTyyp>>(JsonOptions);

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.NotEmpty(result.Results);
        }

        [Fact]
        public async Task Get_should_return_operatsiooni_tyyp()
        {
            var id = await AddOperatsiooniTyypAsync("Rehvide vahetus", "Rehvide vahetus ja tasakaalustamine");

            var response = await Client.GetAsync($"/api/OperatsiooniTyyp/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<OperatsiooniTyypDto>(JsonOptions);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Rehvide vahetus", result.Nimi);
            Assert.Equal("Rehvide vahetus ja tasakaalustamine", result.Kirjeldus);
        }
    }
}