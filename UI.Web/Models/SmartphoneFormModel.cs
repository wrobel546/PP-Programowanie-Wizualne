using System.ComponentModel.DataAnnotations;
using NowakowskaWrobel.Smartphones.CORE;

namespace NowakowskaWrobel.Smartphones.UI.Web.Models
{
    public class SmartphoneFormModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Model musi mieć co najmniej 2 znaki.")]
        public string ModelName { get; set; } = string.Empty;

        [Required]
        public SmartphoneOs OperatingSystem { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cena nie może być ujemna.")]
        public decimal Price { get; set; }

        [Range(0.1, 20, ErrorMessage = "Przekątna ekranu musi być dodatnia.")]
        public double ScreenSize { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "RAM musi być dodatni.")]
        public int RamGb { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Pamięć musi być dodatnia.")]
        public int StorageGb { get; set; }

        public int? ProducerId { get; set; }
    }
}
