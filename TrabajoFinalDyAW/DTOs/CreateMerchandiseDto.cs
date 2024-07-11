using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalDyAW.DTOs
{
    public class CreateMerchandiseDto
    {
        [Required(ErrorMessage = "El nombre de la mercancia es requerido")]
        [StringLength(255, MinimumLength = 1)]
        public string MerchandiseName { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string MerchandiseBarcode { get; set; }
        public string MerchandiseDescription { get; set; }
        public int MerchandiseStock { get; set; }
    }
}