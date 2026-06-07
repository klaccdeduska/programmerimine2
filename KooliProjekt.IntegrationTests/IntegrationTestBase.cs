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

        protected async Task ExecuteDbAsync(Func<ApplicationDbContext, Task> action)
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await action(db);
        }

        protected async Task<T> ExecuteDbAsync<T>(Func<ApplicationDbContext, Task<T>> action)
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await action(db);
        }

        protected async Task<int> AddAutoAsync(
            string tootja = "Toyota",
            string mudel = "Corolla",
            string numbrimark = "123ABC")
        {
            return await ExecuteDbAsync(async db =>
            {
                var auto = new Auto
                {
                    Tootja = tootja,
                    Mudel = mudel,
                    Numbrimark = numbrimark
                };

                await db.Autos.AddAsync(auto);
                await db.SaveChangesAsync();

                return auto.Id;
            });
        }

        protected async Task<int> AddTootajaAsync(
            string nimi = "Mati Maasikas",
            string email = "mati@mail.com",
            string roll = "Mehaanik")
        {
            return await ExecuteDbAsync(async db =>
            {
                var tootaja = new Töötaja
                {
                    Nimi = nimi,
                    Email = email,
                    Roll = roll
                };

                await db.Töötajad.AddAsync(tootaja);
                await db.SaveChangesAsync();

                return tootaja.Id;
            });
        }

        protected async Task<int> AddOperatsiooniTyypAsync(
            string nimi = "Õlivahetus",
            string kirjeldus = "Mootoriõli vahetus")
        {
            return await ExecuteDbAsync(async db =>
            {
                var tyyp = new OperatsiooniTyyp
                {
                    Nimi = nimi,
                    Kirjeldus = kirjeldus
                };

                await db.OperatsiooniTüübid.AddAsync(tyyp);
                await db.SaveChangesAsync();

                return tyyp.Id;
            });
        }

        protected async Task<int> AddOperatsioonAsync()
        {
            return await ExecuteDbAsync(async db =>
            {
                var auto = new Auto
                {
                    Tootja = "Toyota",
                    Mudel = "Corolla",
                    Numbrimark = Guid.NewGuid().ToString("N")[..10]
                };

                var tootaja = new Töötaja
                {
                    Nimi = "Mati Maasikas",
                    Email = $"{Guid.NewGuid():N}@mail.com",
                    Roll = "Mehaanik"
                };

                var tyyp = new OperatsiooniTyyp
                {
                    Nimi = "Õlivahetus " + Guid.NewGuid().ToString("N")[..5],
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
            });
        }
    }
}