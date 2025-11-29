using System.Collections.Generic;
using NowakowskaWrobel.Smartphones.CORE;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.BLC
{
    public class CatalogService
    {
        private readonly IProducerRepository _producerRepository;
        private readonly ISmartphoneRepository _smartphonesRepository;

        public CatalogService(IProducerRepository producerRepository, ISmartphoneRepository smartphonesRepository)
        {
            _producerRepository = producerRepository;
            _smartphonesRepository = smartphonesRepository;
        }

        // PRODUCENCI

        public IEnumerable<IProducer> GetProducers()
        {
            return _producerRepository.GetAll();
        }

        public IProducer? GetProducerById(int id)
        {
            return _producerRepository.GetById(id);
        }

        public void AddProducer(IProducer producer)
        {
            _producerRepository.Add(producer);
        }

        public void UpdateProducer(IProducer producer)
        {
            _producerRepository.Update(producer);
        }

        public void DeleteProducer(int id)
        {
            _producerRepository.Delete(id);
        }

        // SMARTFONY

        public IEnumerable<ISmartphone> GetSmartphones()
        {
            return _smartphonesRepository.GetAll();
        }

        public ISmartphone? GetSmartphoneById(int id)
        {
            return _smartphonesRepository.GetById(id);
        }

        public IEnumerable<ISmartphone> FilterSmartphones(
            string? modelName,
            SmartphoneOs? os,
            int? producerId,
            decimal? minPrice,
            decimal? maxPrice)
        {
            return _smartphonesRepository.Filter(modelName, os, producerId, minPrice, maxPrice);
        }

        public void AddSmartphone(ISmartphone smartphone)
        {
            _smartphonesRepository.Add(smartphone);
        }

        public void UpdateSmartphone(ISmartphone smartphone)
        {
            _smartphonesRepository.Update(smartphone);
        }

        public void DeleteSmartphone(int id)
        {
            _smartphonesRepository.Delete(id);
        }
    }
}
