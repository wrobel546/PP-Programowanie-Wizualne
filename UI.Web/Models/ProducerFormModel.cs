using System.ComponentModel.DataAnnotations;

namespace NowakowskaWrobel.Smartphones.UI.Web.Models
{
    public class ProducerFormModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Nazwa musi mieć co najmniej 2 znaki.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;
    }
}
