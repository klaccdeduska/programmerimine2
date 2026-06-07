using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using Xunit;

namespace KooliProjekt.IntegrationTests.Controllers
{
    public class TootajadControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {
        public TootajadControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task List_should_return_tootajad()
        {
            await AddTootajaAsync("Mati Maasikas", "mati@mail.com", "Mehaanik");

            var response = await Client.GetAsync("/api/Tootajad?Page=1&PageSize=10");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<PagedResult<Töötaja>>(JsonOptions);

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.NotEmpty(result.Results);
        }

        [Fact]
        public async Task Get_should_return_tootaja()
        {
            var id = await AddTootajaAsync("Kati Kuusk", "kati@mail.com", "Admin");

            var response = await Client.GetAsync($"/api/Tootajad/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<TootajaDto>(JsonOptions);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Kati Kuusk", result.Nimi);
            Assert.Equal("kati@mail.com", result.Email);
            Assert.Equal("Admin", result.Roll);
        }
    }
}