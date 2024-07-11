using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalDyAW.Presenters
{
    public class MerchandisePresenter
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Barcode { get; set; }
        [Required]
        public int Stock { get; set; }
    }
}