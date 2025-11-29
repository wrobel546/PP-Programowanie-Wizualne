using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowakowskaWrobel.Smartphones.CORE;

namespace NowakowskaWrobel.Smartphones.INTERFACES
{
    public interface ISmartphoneRepository
    {
        IEnumerable<ISmartphone> GetAll();
        ISmartphone GetById(int id);
        void Add(ISmartphone smartphone);
        void Update(ISmartphone smartphone);
        void Delete(int id);

        IEnumerable<ISmartphone> Filter(
            string? modelName,
            SmartphoneOs? os,
            int? producerId,
            decimal? minPrice,
            decimal? maxPrice);

    }
}
