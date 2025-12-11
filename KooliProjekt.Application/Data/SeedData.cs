using System;
using System.Linq;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext db)
        {
            db.Database.Migrate();

            // -------------------------
            // 1. Operatsiooni Tyyp
            // -------------------------
            if (!db.OperatsiooniTüübid.Any())
            {
                db.OperatsiooniTüübid.AddRange(
                    new OperatsiooniTyyp { Nimi = "Õlivahetus", Kirjeldus = "Mootoriõli vahetus" },
                    new OperatsiooniTyyp { Nimi = "Rehvide vahetus", Kirjeldus = "Talve/suve rehvid" },
                    new OperatsiooniTyyp { Nimi = "Diagnostika", Kirjeldus = "Auto diagnostika" },
                    new OperatsiooniTyyp { Nimi = "Pesu", Kirjeldus = "Kere ja salongi pesu" },
                    new OperatsiooniTyyp { Nimi = "Pidurite kontroll", Kirjeldus = "Pidurisüsteemi kontroll" },
                    new OperatsiooniTyyp { Nimi = "Rihmade kontroll", Kirjeldus = "Rihmade pingutamine" },
                    new OperatsiooniTyyp { Nimi = "Aku test", Kirjeldus = "Aku seisukord" },
                    new OperatsiooniTyyp { Nimi = "Filter vahetus", Kirjeldus = "Õhu / kütuse filter" },
                    new OperatsiooniTyyp { Nimi = "Kliima täitmine", Kirjeldus = "Konditsioneeri hooldus" },
                    new OperatsiooniTyyp { Nimi = "Mootori remont", Kirjeldus = "Suurem hooldus" }
                );
                db.SaveChanges();
            }

            // -------------------------
            // 2. Töötajad
            // -------------------------
            if (!db.Töötajad.Any())
            {
                db.Töötajad.AddRange(
                    new Töötaja { Nimi = "Mati Maasikas", Email = "mati@mail.com", Roll = "Mehaanik" },
                    new Töötaja { Nimi = "Kati Puu", Email = "kati@mail.com", Roll = "Admin" },
                    new Töötaja { Nimi = "Karl Kask", Email = "karl@mail.com", Roll = "Mehaanik" },
                    new Töötaja { Nimi = "Mari Mänd", Email = "mari@mail.com", Roll = "Pesija" },
                    new Töötaja { Nimi = "Jüri Juur", Email = "juri@mail.com", Roll = "Mehaanik" },
                    new Töötaja { Nimi = "Aadu Aun", Email = "aadu@mail.com", Roll = "Admin" },
                    new Töötaja { Nimi = "Rein Raba", Email = "rein@mail.com", Roll = "Mehaanik" },
                    new Töötaja { Nimi = "Tiina Tare", Email = "tiina@mail.com", Roll = "Pesija" },
                    new Töötaja { Nimi = "Jaan Juurikas", Email = "jaan@mail.com", Roll = "Mehaanik" },
                    new Töötaja { Nimi = "Laura Lille", Email = "laura@mail.com", Roll = "Admin" }
                );
                db.SaveChanges();
            }

            // -------------------------
            // 3. Autod
            // -------------------------
            if (!db.Autos.Any())
            {
                db.Autos.AddRange(
                    new Auto { Tootja = "BMW", Mudel = "530", Numbrimark = "111ABC" },
                    new Auto { Tootja = "Audi", Mudel = "A6", Numbrimark = "222BCD" },
                    new Auto { Tootja = "Toyota", Mudel = "Camry", Numbrimark = "333CDE" },
                    new Auto { Tootja = "Honda", Mudel = "Civic", Numbrimark = "444DEF" },
                    new Auto { Tootja = "Mercedes", Mudel = "E220", Numbrimark = "555EFG" },
                    new Auto { Tootja = "VW", Mudel = "Passat", Numbrimark = "666FGH" },
                    new Auto { Tootja = "Volvo", Mudel = "S60", Numbrimark = "777GHI" },
                    new Auto { Tootja = "Mazda", Mudel = "6", Numbrimark = "888HIJ" },
                    new Auto { Tootja = "Ford", Mudel = "Focus", Numbrimark = "999IJK" },
                    new Auto { Tootja = "Tesla", Mudel = "S", Numbrimark = "000JKL" }
                );
                db.SaveChanges();
            }

            // -------------------------
            // 4. Operatsioonid
            // -------------------------
            if (!db.Operatsioonid.Any())
            {
                var autod = db.Autos.ToList();
                var töötajad = db.Töötajad.ToList();
                var tüübid = db.OperatsiooniTüübid.ToList();

                var rnd = new Random();

                for (int i = 0; i < 10; i++)
                {
                    db.Operatsioonid.Add(new Operatsioon
                    {
                        AutoId = autod[rnd.Next(autod.Count)].Id,
                        TöötajaId = töötajad[rnd.Next(töötajad.Count)].Id,
                        TüüpId = tüübid[rnd.Next(tüübid.Count)].Id,
                        Kuupäev = DateTime.Now.AddDays(-rnd.Next(1, 100)),
                        Staatus = "Valmis",
                        Maksumus = rnd.Next(20, 500)
                    });
                }

                db.SaveChanges();
            }
        }
    }
}
