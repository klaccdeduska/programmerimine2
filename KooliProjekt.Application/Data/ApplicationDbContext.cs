using Microsoft.EntityFrameworkCore;
using System;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Наши сущности
        public DbSet<Auto> Autos { get; set; }
        public DbSet<Operatsioon> Operatsioonid { get; set; }
        public DbSet<OperatsiooniTyyp> OperatsiooniTüübid { get; set; }
        public DbSet<Töötaja> Töötajad { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Уникальные индексы
            modelBuilder.Entity<Auto>()
                .HasIndex(a => a.Numbrimark)
                .IsUnique();

            modelBuilder.Entity<Töötaja>()
                .HasIndex(t => t.Email)
                .IsUnique();

            modelBuilder.Entity<OperatsiooniTyyp>()
                .HasIndex(t => t.Nimi)
                .IsUnique();

            // Связи Operatsioon
            modelBuilder.Entity<Operatsioon>()
                .HasOne(o => o.Auto)
                .WithMany()
                .HasForeignKey(o => o.AutoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Operatsioon>()
                .HasOne(o => o.Tüüp)
                .WithMany()
                .HasForeignKey(o => o.TüüpId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Operatsioon>()
                .HasOne(o => o.Töötaja)
                .WithMany()
                .HasForeignKey(o => o.TöötajaId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== SEED DATA =====

            // Работники
            modelBuilder.Entity<Töötaja>().HasData(
                new Töötaja { Id = 1, Nimi = "Admin", Email = "admin@example.com", Roll = "Administraator" },
                new Töötaja { Id = 2, Nimi = "Jaan", Email = "jaan@example.com", Roll = "Töötaja" }
            );

            // Авто
            modelBuilder.Entity<Auto>().HasData(
                new Auto { Id = 1, Tootja = "Toyota", Mudel = "Corolla", Numbrimark = "123ABC" },
                new Auto { Id = 2, Tootja = "BMW", Mudel = "320", Numbrimark = "555BMW" }
            );

            // Типы операций
            modelBuilder.Entity<OperatsiooniTyyp>().HasData(
                new OperatsiooniTyyp { Id = 1, Nimi = "Õlivahetus", Kirjeldus = "Mootoriõli vahetus" },
                new OperatsiooniTyyp { Id = 2, Nimi = "Rehvide vahetus", Kirjeldus = "Rehvide vahetus komplektiga" }
            );

            // Операции
            modelBuilder.Entity<Operatsioon>().HasData(
                new Operatsioon
                {
                    Id = 1,
                    AutoId = 1,
                    TüüpId = 1,
                    TöötajaId = 2,
                    Staatus = "Ootel",
                    Kuupäev = DateTime.Now.AddDays(-2),
                    Maksumus = 35
                },
                new Operatsioon
                {
                    Id = 2,
                    AutoId = 2,
                    TüüpId = 2,
                    TöötajaId = 2,
                    Staatus = "Tegemisel",
                    Kuupäev = DateTime.Now.AddDays(-1),
                    Maksumus = 50
                }
            );
        }
    }
}
