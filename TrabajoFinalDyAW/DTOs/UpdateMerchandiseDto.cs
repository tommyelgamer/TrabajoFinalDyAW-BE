using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalDyAW.DTOs
{
    public class UpdateMerchandiseDto
    {
        public string? MerchandiseName { get; set; }
        public string? MerchandiseBarcode { get; set; }
        public string? MerchandiseDescription { get; set; }
        public int? MerchandiseStock { get; set; }
    }
}