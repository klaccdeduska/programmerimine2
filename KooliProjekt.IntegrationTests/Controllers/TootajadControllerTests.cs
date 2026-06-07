using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Tootajad;
using KooliProjekt.Application.Infrastructure.Paging;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests.Controllers
{
    public class TootajadControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {
        public TootajadControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Save_should_create_tootaja()
        {
            var command = new SaveTootajaCommand
            {
                Id = 0,
                Nimi = "Jaan Tamm",
                Email = "jaan@mail.com",
                Roll = "Mehaanik"
            };

            var response = await Client.PostAsJsonAsync("/api/Tootajad", command);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<Töötaja>(JsonOptions);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("Jaan Tamm", result.Nimi);
            Assert.Equal("jaan@mail.com", result.Email);
            Assert.Equal("Mehaanik", result.Roll);
        }

        [Fact]
        public async Task Save_should_update_tootaja()
        {
            var id = await AddTootajaAsync("Old", "old@mail.com", "Old");

            var command = new SaveTootajaCommand
            {
                Id = id,
                Nimi = "Kati Kuusk",
                Email = "kati@mail.com",
                Roll = "Admin"
            };

            var response = await Client.PostAsJsonAsync("/api/Tootajad", command);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<Töötaja>(JsonOptions);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Kati Kuusk", result.Nimi);
            Assert.Equal("kati@mail.com", result.Email);
            Assert.Equal("Admin", result.Roll);
        }

        [Fact]
        public async Task Delete_should_delete_tootaja()
        {
            var id = await AddTootajaAsync("Delete", "delete@mail.com", "Mehaanik");

            var response = await Client.DeleteAsync($"/api/Tootajad/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<bool>(JsonOptions);

            Assert.True(result);

            var entity = await ExecuteDbAsync(db => db.Töötajad.FindAsync(id).AsTask());

            Assert.Null(entity);
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