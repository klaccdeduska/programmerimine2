using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using Xunit;

namespace KooliProjekt.IntegrationTests.Controllers
{
    public class OperatsioonidControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {
        public OperatsioonidControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task List_should_return_operatsioonid()
        {
            await AddOperatsioonAsync();

            var response = await Client.GetAsync("/api/Operatsioonid?Page=1&PageSize=10");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<PagedResult<Operatsioon>>(JsonOptions);

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.NotEmpty(result.Results);
        }

        [Fact]
        public async Task Get_should_return_operatsioon()
        {
            var id = await AddOperatsioonAsync();

            var response = await Client.GetAsync($"/api/Operatsioonid/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<OperatsioonDto>(JsonOptions);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Valmis", result.Staatus);
            Assert.Equal(100m, result.Maksumus);
        }
    }
}