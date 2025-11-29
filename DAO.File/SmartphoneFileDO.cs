using NowakowskaWrobel.Smartphones.CORE;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.DAO.File
{
    public class SmartphoneFileDO : ISmartphone
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
