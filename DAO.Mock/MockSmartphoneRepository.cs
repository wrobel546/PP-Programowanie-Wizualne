using System.Collections.Generic;
using System.Linq;
using NowakowskaWrobel.Smartphones.CORE;
using NowakowskaWrobel.Smartphones.DAO.Mock;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.DAO.Mock
{
    public class MockSmartphoneRepository : ISmartphoneRepository
    {
        private readonly List<ISmartphone> _smartphones = new List<ISmartphone>();
        private readonly IProducerRepository _producerRepository;
        private int _nextId = 1;

        public MockSmartphoneRepository(IProducerRepository producerRepository)
        {
            _producerRepository = producerRepository;

            // przykładowe dane
            var samsung = _producerRepository.GetAll().First(p => p.Name == "Samsung");
            var apple = _producerRepository.GetAll().First(p => p.Name == "Apple");

            Add(new SmartphoneDO
            {
                ModelName = "Galaxy S24",
                OperatingSystem = SmartphoneOs.Android,
                Price = 3999,
                ScreenSize = 6.2,
                RamGb = 8,
                StorageGb = 128,
                Producer = samsung
            });

            Add(new SmartphoneDO
            {
                ModelName = "iPhone 15",
                OperatingSystem = SmartphoneOs.IOS,
                Price = 5199,
                ScreenSize = 6.1,
                RamGb = 6,
                StorageGb = 128,
                Producer = apple
            });
        }

        public IEnumerable<ISmartphone> GetAll()
        {
            return _smartphones.ToList();
        }

        public ISmartphone GetById(int id)
        {
            return _smartphones.FirstOrDefault(s => s.Id == id);
        }

        public void Add(ISmartphone smartphone)
        {
            smartphone.Id = _nextId++;
            _smartphones.Add(smartphone);
        }

        public void Update(ISmartphone smartphone)
        {
            var existing = GetById(smartphone.Id);
            if (existing == null)
            {
                return;
            }

            existing.ModelName = smartphone.ModelName;
            existing.OperatingSystem = smartphone.OperatingSystem;
            existing.Price = smartphone.Price;
            existing.ScreenSize = smartphone.ScreenSize;
            existing.RamGb = smartphone.RamGb;
            existing.StorageGb = smartphone.StorageGb;
            existing.Producer = smartphone.Producer;
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                _smartphones.Remove(existing);
            }
        }

        public IEnumerable<ISmartphone> Filter(
            string? modelName,
            SmartphoneOs? os,
            int? producerId,
            decimal? minPrice,
            decimal? maxPrice)
        {
            var query = _smartphones.AsQueryable();

            if (!string.IsNullOrWhiteSpace(modelName))
            {
                query = query.Where(s => s.ModelName.ToLower().Contains(modelName.ToLower()));
            }

            if (os.HasValue)
            {
                query = query.Where(s => s.OperatingSystem == os.Value);
            }

            if (producerId.HasValue)
            {
                query = query.Where(s => s.Producer != null && s.Producer.Id == producerId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(s => s.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(s => s.Price <= maxPrice.Value);
            }

            return query.ToList();
        }
    }
}
