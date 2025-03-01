namespace E_CommerceMVC.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Qty { get; set; }
    }
}
