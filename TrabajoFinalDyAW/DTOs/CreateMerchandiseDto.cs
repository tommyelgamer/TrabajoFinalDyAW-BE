using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrabajoFinalDyAW.DTOs
{
    public class CreateMerchandiseDto
    {
        [Required(ErrorMessage = "El nombre de la mercancia es requerido")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El codigo de barras de la mercancia es requerido")]
        [JsonPropertyName("barcode")]
        public string Barcode { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [Required]
        [JsonPropertyName("stock")]
        public int Stock { get; set; }
    }
}