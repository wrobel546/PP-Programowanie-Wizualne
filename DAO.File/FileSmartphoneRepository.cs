using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NowakowskaWrobel.Smartphones.CORE;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.DAO.File
{
    public class FileSmartphoneRepository : ISmartphoneRepository
    {
        private readonly string _filePath;
        private readonly List<ISmartphone> _smartphones = new List<ISmartphone>();
        private readonly IProducerRepository _producerRepository;
        private int _nextId = 1;

        public FileSmartphoneRepository(IProducerRepository producerRepository)
        {
            _producerRepository = producerRepository;

            var folder = AppContext.BaseDirectory;
            _filePath = Path.Combine(folder, "smartphones.json");

            Load();
        }

        private void Load()
        {
            _smartphones.Clear();

            if (!System.IO.File.Exists(_filePath))
            {
                // brak pliku – przykładowe dane
                var samsung = _producerRepository.GetAll().FirstOrDefault(p => p.Name == "Samsung");
                var apple = _producerRepository.GetAll().FirstOrDefault(p => p.Name == "Apple");

                if (samsung != null)
                {
                    Add(new SmartphoneFileDO
                    {
                        ModelName = "Galaxy S24",
                        OperatingSystem = SmartphoneOs.Android,
                        Price = 3999,
                        ScreenSize = 6.2,
                        RamGb = 8,
                        StorageGb = 128,
                        Producer = samsung
                    });
                }

                if (apple != null)
                {
                    Add(new SmartphoneFileDO
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

                Save();
                return;
            }

            var json = System.IO.File.ReadAllText(_filePath);
            var list = JsonSerializer.Deserialize<List<SmartphoneFileDO>>(json) ?? new List<SmartphoneFileDO>();

            foreach (var s in list)
            {
                // odtworzenie producenta na podstawie Id (jeśli był zapisany)
                IProducer? prod = null;
                if (s.Producer != null)
                {
                    prod = _producerRepository.GetById(s.Producer.Id);
                }

                var smartphone = new SmartphoneFileDO
                {
                    Id = s.Id,
                    ModelName = s.ModelName,
                    OperatingSystem = s.OperatingSystem,
                    Price = s.Price,
                    ScreenSize = s.ScreenSize,
                    RamGb = s.RamGb,
                    StorageGb = s.StorageGb,
                    Producer = prod
                };

                _smartphones.Add(smartphone);

                if (smartphone.Id >= _nextId)
                {
                    _nextId = smartphone.Id + 1;
                }
            }
        }

        private void Save()
        {
            var list = _smartphones
                .Select(s => new SmartphoneFileDO
                {
                    Id = s.Id,
                    ModelName = s.ModelName,
                    OperatingSystem = s.OperatingSystem,
                    Price = s.Price,
                    ScreenSize = s.ScreenSize,
                    RamGb = s.RamGb,
                    StorageGb = s.StorageGb,
                    Producer = s.Producer // zapisujemy Id + dane producenta
                })
                .ToList();

            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_filePath, json);
        }

        public IEnumerable<ISmartphone> GetAll()
        {
            return _smartphones.ToList();
        }

        public ISmartphone? GetById(int id)
        {
            return _smartphones.FirstOrDefault(s => s.Id == id);
        }

        public void Add(ISmartphone smartphone)
        {
            smartphone.Id = _nextId++;
            _smartphones.Add(smartphone);
            Save();
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

            Save();
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                _smartphones.Remove(existing);
                Save();
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
                var lower = modelName.ToLower();
                query = query.Where(s => s.ModelName.ToLower().Contains(lower));
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
