using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrabajoFinalDyAW.DTOs
{
    public class UpdateMerchandiseDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("barcode")]
        public string? Barcode { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("stock")]
        public int? Stock { get; set; }
    }
}