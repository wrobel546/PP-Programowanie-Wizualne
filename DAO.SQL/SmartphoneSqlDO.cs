using NowakowskaWrobel.Smartphones.CORE;
using NowakowskaWrobel.Smartphones.INTERFACES;
using System.ComponentModel.DataAnnotations.Schema;

namespace NowakowskaWrobel.Smartphones.DAO.SQL
{
    public class SmartphoneSqlDO : ISmartphone
    {
        public int Id { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public SmartphoneOs OperatingSystem { get; set; }

        public decimal Price { get; set; }
        public double ScreenSize { get; set; }
        public int RamGb { get; set; }

        public int StorageGb { get; set; }

        [NotMapped]
        public IProducer? Producer
        {
            get => ProducerNavigation;
            set => ProducerNavigation = (ProducerSqlDO?)value;
        }
        public int? ProducerId { get; set; }
        public ProducerSqlDO? ProducerNavigation { get; set; }
    }
}
