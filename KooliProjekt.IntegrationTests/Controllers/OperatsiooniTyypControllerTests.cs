using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using Xunit;

namespace KooliProjekt.IntegrationTests.Controllers
{
    public class OperatsiooniTyypControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {
        public OperatsiooniTyypControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {
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