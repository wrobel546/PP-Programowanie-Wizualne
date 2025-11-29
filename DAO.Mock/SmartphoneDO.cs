using NowakowskaWrobel.Smartphones.CORE;
using NowakowskaWrobel.Smartphones.INTERFACES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowakowskaWrobel.Smartphones.DAO.Mock
{
    public class SmartphoneDO : ISmartphone
    {
        public int Id { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public SmartphoneOs OperatingSystem { get; set; }
        public decimal Price { get; set; }
        public double ScreenSize { get; set; }
        public int RamGb { get; set; }
        public int StorageGb { get; set; }

        public IProducer? Producer { get; set; } 
    }
}
