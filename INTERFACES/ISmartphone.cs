using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowakowskaWrobel.Smartphones.CORE;

namespace NowakowskaWrobel.Smartphones.INTERFACES
{
    public interface ISmartphone
    {
        int Id { get; set; }
        string ModelName { get; set; }
        SmartphoneOs OperatingSystem { get; set; }
        decimal Price { get; set; }
        double ScreenSize { get; set; }
        int RamGb { get; set; }
        int StorageGb { get; set; }

        IProducer? Producer { get; set; }
    }
}
