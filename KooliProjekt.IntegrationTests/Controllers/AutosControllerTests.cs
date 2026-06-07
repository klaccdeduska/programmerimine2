using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using Xunit;

namespace KooliProjekt.IntegrationTests.Controllers
{
    public class AutosControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {
        public AutosControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task List_should_return_autos()
        {
            await AddAutoAsync("Toyota", "Corolla", "123ABC");

            var response = await Client.GetAsync("/api/Autos?Page=1&PageSize=10");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<PagedResult<Auto>>(JsonOptions);

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.NotEmpty(result.Results);
        }

        [Fact]
        public async Task Get_should_return_auto()
        {
            var id = await AddAutoAsync("BMW", "X5", "456DEF");

            var response = await Client.GetAsync($"/api/Autos/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<AutoDto>(JsonOptions);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("BMW", result.Tootja);
            Assert.Equal("X5", result.Mudel);
            Assert.Equal("456DEF", result.Numbrimark);
        }
    }
}