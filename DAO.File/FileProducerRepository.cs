using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.DAO.File
{
    public class FileProducerRepository : IProducerRepository
    {
        private readonly string _filePath;
        private readonly List<IProducer> _producers = new List<IProducer>();
        private int _nextId = 1;

        public FileProducerRepository()
        {
            var folder = AppContext.BaseDirectory;
            _filePath = Path.Combine(folder, "producers.json");

            Load();
        }

        private void Load()
        {
            _producers.Clear();

            if (!System.IO.File.Exists(_filePath))
            {
                Add(new ProducerFileDO { Name = "Samsung", Country = "Korea Poludniowa" });
                Add(new ProducerFileDO { Name = "Apple", Country = "USA" });
                Add(new ProducerFileDO { Name = "Xiaomi", Country = "Chiny" });
                Save();
                return;
            }

            var json = System.IO.File.ReadAllText(_filePath);
            var list = JsonSerializer.Deserialize<List<ProducerFileDO>>(json) ?? new List<ProducerFileDO>();

            foreach (var p in list)
            {
                _producers.Add(p);
                if (p.Id >= _nextId)
                {
                    _nextId = p.Id + 1;
                }
            }
        }

        private void Save()
        {
            var list = _producers
                .Select(p => new ProducerFileDO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Country = p.Country,
                    Founded = p.Founded
                })
                .ToList();

            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_filePath, json);
        }

        public IEnumerable<IProducer> GetAll()
        {
            return _producers.ToList();
        }

        public IProducer? GetById(int id)
        {
            return _producers.FirstOrDefault(p => p.Id == id);
        }

        public void Add(IProducer producer)
        {
            producer.Id = _nextId++;
            _producers.Add(producer);
            Save();
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
            Save();
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                _producers.Remove(existing);
                Save();
            }
        }
    }
}
