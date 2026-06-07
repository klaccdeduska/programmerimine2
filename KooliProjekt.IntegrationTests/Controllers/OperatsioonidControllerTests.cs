using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Operatsioonid;
using KooliProjekt.Application.Infrastructure.Paging;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests.Controllers
{
    public class OperatsioonidControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {
        public OperatsioonidControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Save_should_create_operatsioon()
        {
            var autoId = await AddAutoAsync("Toyota", "Corolla", "OP123");
            var tootajaId = await AddTootajaAsync("Mati Maasikas", "mati2@mail.com", "Mehaanik");
            var tyypId = await AddOperatsiooniTyypAsync("Õlivahetus create", "Mootoriõli vahetus");

            var date = DateTime.Now.AddDays(-1);

            var command = new SaveOperatsioonCommand
            {
                Id = 0,
                AutoId = autoId,
                TöötajaId = tootajaId,
                TüüpId = tyypId,
                Kuupäev = date,
                Staatus = "Valmis",
                Maksumus = 150m
            };

            var response = await Client.PostAsJsonAsync("/api/Operatsioonid", command);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<Operatsioon>(JsonOptions);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(autoId, result.AutoId);
            Assert.Equal(tootajaId, result.TöötajaId);
            Assert.Equal(tyypId, result.TüüpId);
            Assert.Equal("Valmis", result.Staatus);
            Assert.Equal(150m, result.Maksumus);
        }

        [Fact]
        public async Task Save_should_update_operatsioon()
        {
            var id = await AddOperatsioonAsync();

            var autoId = await AddAutoAsync("BMW", "X5", "OP456");
            var tootajaId = await AddTootajaAsync("Kati Kuusk", "kati2@mail.com", "Admin");
            var tyypId = await AddOperatsiooniTyypAsync("Rehvide vahetus update", "Rehvide vahetus");

            var date = DateTime.Now.AddDays(-2);

            var command = new SaveOperatsioonCommand
            {
                Id = id,
                AutoId = autoId,
                TöötajaId = tootajaId,
                TüüpId = tyypId,
                Kuupäev = date,
                Staatus = "Tegemisel",
                Maksumus = 200m
            };

            var response = await Client.PostAsJsonAsync("/api/Operatsioonid", command);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<Operatsioon>(JsonOptions);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(autoId, result.AutoId);
            Assert.Equal(tootajaId, result.TöötajaId);
            Assert.Equal(tyypId, result.TüüpId);
            Assert.Equal("Tegemisel", result.Staatus);
            Assert.Equal(200m, result.Maksumus);
        }

        [Fact]
        public async Task Delete_should_delete_operatsioon()
        {
            var id = await AddOperatsioonAsync();

            var response = await Client.DeleteAsync($"/api/Operatsioonid/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<bool>(JsonOptions);

            Assert.True(result);

            var entity = await ExecuteDbAsync(db => db.Operatsioonid.FindAsync(id).AsTask());

            Assert.Null(entity);
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