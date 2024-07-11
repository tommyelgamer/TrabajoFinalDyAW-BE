namespace TrabajoFinalDyAW.Entities
{
    public class Merchandise
    {
        public Guid Id { get; set; }
        public string MerchandiseName { get; set; }
        public string MerchandiseBarcode { get; set; }
        public string MerchandiseDescription { get; set; }
        public int MerchandiseStock { get; set; }
    }
}
