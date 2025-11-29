using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowakowskaWrobel.Smartphones.INTERFACES
{
    public interface IProducerRepository
    {
        IEnumerable<IProducer> GetAll();
        IProducer? GetById(int id);
        void Add(IProducer producer);
        void Update(IProducer producer);
        void Delete(int id);
    }
}
