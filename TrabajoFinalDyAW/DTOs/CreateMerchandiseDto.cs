using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalDyAW.DTOs
{
    public class CreateMerchandiseDto
    {
        [Required(ErrorMessage = "El nombre de la mercancia es requerido")]
        public string MerchandiseName { get; set; }
        [Required]
        public string MerchandiseBarcode { get; set; }
        public string MerchandiseDescription { get; set; }
        public int MerchandiseStock { get; set; }
    }
}