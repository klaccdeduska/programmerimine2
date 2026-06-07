using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Autos;
using KooliProjekt.Application.Infrastructure.Paging;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests.Controllers
{
    public class AutosControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {
        public AutosControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Save_should_create_auto()
        {
            var command = new SaveAutoCommand
            {
                Id = 0,
                Tootja = "Audi",
                Mudel = "A6",
                Numbrimark = "AUD123"
            };

            var response = await Client.PostAsJsonAsync("/api/Autos", command);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<Auto>(JsonOptions);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("Audi", result.Tootja);
            Assert.Equal("A6", result.Mudel);
            Assert.Equal("AUD123", result.Numbrimark);
        }

        [Fact]
        public async Task Save_should_update_auto()
        {
            var id = await AddAutoAsync("Old", "Old", "OLD123");

            var command = new SaveAutoCommand
            {
                Id = id,
                Tootja = "Mercedes",
                Mudel = "E",
                Numbrimark = "MER123"
            };

            var response = await Client.PostAsJsonAsync("/api/Autos", command);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<Auto>(JsonOptions);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Mercedes", result.Tootja);
            Assert.Equal("E", result.Mudel);
            Assert.Equal("MER123", result.Numbrimark);
        }

        [Fact]
        public async Task Delete_should_delete_auto()
        {
            var id = await AddAutoAsync("Delete", "Me", "DEL123");

            var response = await Client.DeleteAsync($"/api/Autos/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<bool>(JsonOptions);

            Assert.True(result);

            var entity = await ExecuteDbAsync(db => db.Autos.FindAsync(id).AsTask());

            Assert.Null(entity);
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