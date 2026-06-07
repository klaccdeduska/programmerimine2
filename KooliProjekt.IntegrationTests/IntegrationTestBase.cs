using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        protected readonly CustomWebApplicationFactory Factory;
        protected readonly HttpClient Client;

        protected readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        protected IntegrationTestBase(CustomWebApplicationFactory factory)
        {
            Factory = factory;
            Client = factory.CreateClient();
        }

        protected async Task<int> AddAutoAsync(
            string tootja = "Toyota",
            string mudel = "Corolla",
            string numbrimark = "123ABC")
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var auto = new Auto
            {
                Tootja = tootja,
                Mudel = mudel,
                Numbrimark = numbrimark
            };

            await db.Autos.AddAsync(auto);
            await db.SaveChangesAsync();

            return auto.Id;
        }

        protected async Task<int> AddTootajaAsync(
            string nimi = "Mati Maasikas",
            string email = "mati@mail.com",
            string roll = "Mehaanik")
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var tootaja = new Töötaja
            {
                Nimi = nimi,
                Email = email,
                Roll = roll
            };

            await db.Töötajad.AddAsync(tootaja);
            await db.SaveChangesAsync();

            return tootaja.Id;
        }

        protected async Task<int> AddOperatsiooniTyypAsync(
            string nimi = "Õlivahetus",
            string kirjeldus = "Mootoriõli vahetus")
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var tyyp = new OperatsiooniTyyp
            {
                Nimi = nimi,
                Kirjeldus = kirjeldus
            };

            await db.OperatsiooniTüübid.AddAsync(tyyp);
            await db.SaveChangesAsync();

            return tyyp.Id;
        }

        protected async Task<int> AddOperatsioonAsync()
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

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

            await db.Autos.AddAsync(auto);
            await db.Töötajad.AddAsync(tootaja);
            await db.OperatsiooniTüübid.AddAsync(tyyp);
            await db.SaveChangesAsync();

            var operatsioon = new Operatsioon
            {
                AutoId = auto.Id,
                TöötajaId = tootaja.Id,
                TüüpId = tyyp.Id,
                Kuupäev = DateTime.Now.AddDays(-1),
                Staatus = "Valmis",
                Maksumus = 100m
            };

            await db.Operatsioonid.AddAsync(operatsioon);
            await db.SaveChangesAsync();

            return operatsioon.Id;
        }
    }
}