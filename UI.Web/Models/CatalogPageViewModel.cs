using System.Collections.Generic;
using NowakowskaWrobel.Smartphones.CORE;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.UI.Web.Models
{
    public class CatalogPageViewModel
    {
        public List<IProducer> Producers { get; set; } = new List<IProducer>();
        public List<ISmartphone> Smartphones { get; set; } = new List<ISmartphone>();

        public ProducerFormModel ProducerForm { get; set; } = new ProducerFormModel();
        public SmartphoneFormModel SmartphoneForm { get; set; } = new SmartphoneFormModel();

        public string? Search { get; set; }
        public SmartphoneOs? OperatingSystem { get; set; }
        public int? ProducerId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
