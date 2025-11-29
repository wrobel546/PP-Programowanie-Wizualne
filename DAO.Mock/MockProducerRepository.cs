using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.DAO.Mock
{
    public class MockProducerRepository : IProducerRepository
    {
        private readonly List<IProducer> _producers = new List<IProducer>();
        private int _nextId = 1;

        public MockProducerRepository() {
            Add(new ProducerDO { Name = "Samsung", Country = "Korea Południowa" });
            Add(new ProducerDO { Name = "Apple", Country = "USA" });
            Add(new ProducerDO { Name = "Xiaomi", Country = "Chiny" });
        }

        public IEnumerable<IProducer> GetAll()
        {
            return _producers.ToList(); 
        }

        public IProducer GetById(int id)
        {
            return _producers.FirstOrDefault(p => p.Id == id);
        }

        public void Add(IProducer producer)
        {
            producer.Id = _nextId++;
            _producers.Add(producer);
        }

        public void Update(IProducer producer) 
        { 
            var existing = GetById(producer.Id);
            if (existing == null)
            {
                return;
            }


            existing.Name = producer.Name;
            existing.Country = producer.Country;
            existing.Founded = producer.Founded;
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                _producers.Remove(existing);
            }
        }

    }
}
