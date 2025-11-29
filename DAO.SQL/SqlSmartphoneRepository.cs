using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NowakowskaWrobel.Smartphones.CORE;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.DAO.SQL
{
    public class SqlSmartphoneRepository : ISmartphoneRepository
    {
        public SqlSmartphoneRepository(IProducerRepository producerRepository)
        {
            // tylko upewniamy się, że baza istnieje i jest zseedowana
            using var ctx = new SmartphonesDbContext();
            ctx.Database.EnsureCreated();

            if (!ctx.Smartphones.Any())
            {
                var samsung = ctx.Producers.FirstOrDefault(p => p.Name == "Samsung");
                var apple = ctx.Producers.FirstOrDefault(p => p.Name == "Apple");

                if (samsung != null)
                {
                    ctx.Smartphones.Add(new SmartphoneSqlDO
                    {
                        ModelName = "Galaxy S24",
                        OperatingSystem = SmartphoneOs.Android,
                        Price = 3999,
                        ScreenSize = 6.2,
                        RamGb = 8,
                        StorageGb = 128,
                        ProducerNavigation = samsung
                    });
                }

                if (apple != null)
                {
                    ctx.Smartphones.Add(new SmartphoneSqlDO
                    {
                        ModelName = "iPhone 15",
                        OperatingSystem = SmartphoneOs.IOS,
                        Price = 5199,
                        ScreenSize = 6.1,
                        RamGb = 6,
                        StorageGb = 128,
                        ProducerNavigation = apple
                    });
                }

                ctx.SaveChanges();
            }
        }

        // ============ GET / FILTER ============

        public IEnumerable<ISmartphone> GetAll()
        {
            using var ctx = new SmartphonesDbContext();

            var list = ctx.Smartphones
                .Include(s => s.ProducerNavigation)
                .AsNoTracking()
                .ToList();

            foreach (var s in list)
            {
                s.Producer = s.ProducerNavigation;   // <-- kluczowe
            }

            return list;
        }

        public ISmartphone? GetById(int id)
        {
            using var ctx = new SmartphonesDbContext();

            var s = ctx.Smartphones
                .Include(x => x.ProducerNavigation)
                .FirstOrDefault(x => x.Id == id);

            if (s != null)
            {
                s.Producer = s.ProducerNavigation;
            }

            return s;
        }

        public IEnumerable<ISmartphone> Filter(
            string? modelName,
            SmartphoneOs? os,
            int? producerId,
            decimal? minPrice,
            decimal? maxPrice)
        {
            using var ctx = new SmartphonesDbContext();

            var query = ctx.Smartphones
                .Include(s => s.ProducerNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(modelName))
            {
                var lower = modelName.ToLower();
                query = query.Where(s => s.ModelName.ToLower().Contains(lower));
            }

            if (os.HasValue)
            {
                query = query.Where(s => s.OperatingSystem == os.Value);
            }

            if (producerId.HasValue)
            {
                query = query.Where(s => s.ProducerId == producerId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(s => s.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(s => s.Price <= maxPrice.Value);
            }

            var list = query.AsNoTracking().ToList();

            foreach (var s in list)
            {
                s.Producer = s.ProducerNavigation;
            }

            return list;
        }

        // ============ ADD / UPDATE / DELETE ============

        public void Add(ISmartphone smartphone)
        {
            using var ctx = new SmartphonesDbContext();

            ProducerSqlDO? producerEntity = null;
            if (smartphone.Producer != null)
            {
                producerEntity = ctx.Producers.Find(smartphone.Producer.Id);
            }

            var entity = new SmartphoneSqlDO
            {
                ModelName = smartphone.ModelName,
                OperatingSystem = smartphone.OperatingSystem,
                Price = smartphone.Price,
                ScreenSize = smartphone.ScreenSize,
                RamGb = smartphone.RamGb,
                StorageGb = smartphone.StorageGb,
                ProducerNavigation = producerEntity,
                ProducerId = producerEntity?.Id
            };

            ctx.Smartphones.Add(entity);
            ctx.SaveChanges();

            smartphone.Id = entity.Id;
            smartphone.Producer = producerEntity;
        }

        public void Update(ISmartphone smartphone)
        {
            using var ctx = new SmartphonesDbContext();

            var entity = ctx.Smartphones.Find(smartphone.Id);
            if (entity == null)
            {
                return;
            }

            entity.ModelName = smartphone.ModelName;
            entity.OperatingSystem = smartphone.OperatingSystem;
            entity.Price = smartphone.Price;
            entity.ScreenSize = smartphone.ScreenSize;
            entity.RamGb = smartphone.RamGb;
            entity.StorageGb = smartphone.StorageGb;

            if (smartphone.Producer != null)
            {
                entity.ProducerId = smartphone.Producer.Id;
            }
            else
            {
                entity.ProducerId = null;
            }

            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            using var ctx = new SmartphonesDbContext();

            var entity = ctx.Smartphones.Find(id);
            if (entity != null)
            {
                ctx.Smartphones.Remove(entity);
                ctx.SaveChanges();
            }
        }
    }
}
