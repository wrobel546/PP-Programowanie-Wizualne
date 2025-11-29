using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.DAO.SQL
{
    public class SqlProducerRepository : IProducerRepository
    {
        public SqlProducerRepository()
        {
            using var ctx = new SmartphonesDbContext();
            ctx.Database.EnsureCreated();

            if (!ctx.Producers.Any())
            {
                // seed przykładowych danych
                ctx.Producers.AddRange(
                    new ProducerSqlDO { Name = "Samsung", Country = "Korea Poludniowa" },
                    new ProducerSqlDO { Name = "Apple", Country = "USA" },
                    new ProducerSqlDO { Name = "Xiaomi", Country = "Chiny" }
                );
                ctx.SaveChanges();
            }
        }

        public IEnumerable<IProducer> GetAll()
        {
            using var ctx = new SmartphonesDbContext();
            return ctx.Producers.AsNoTracking().ToList();
        }

        public IProducer? GetById(int id)
        {
            using var ctx = new SmartphonesDbContext();
            return ctx.Producers.Find(id);
        }

        public void Add(IProducer producer)
        {
            using var ctx = new SmartphonesDbContext();
            var entity = new ProducerSqlDO
            {
                Name = producer.Name,
                Country = producer.Country,
                Founded = producer.Founded
            };
            ctx.Producers.Add(entity);
            ctx.SaveChanges();

            producer.Id = entity.Id;
        }

        public void Update(IProducer producer)
        {
            using var ctx = new SmartphonesDbContext();
            var entity = ctx.Producers.Find(producer.Id);
            if (entity == null)
            {
                return;
            }

            entity.Name = producer.Name;
            entity.Country = producer.Country;
            entity.Founded = producer.Founded;

            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            using var ctx = new SmartphonesDbContext();
            var entity = ctx.Producers.Find(id);
            if (entity != null)
            {
                ctx.Producers.Remove(entity);
                ctx.SaveChanges();
            }
        }
    }
}
