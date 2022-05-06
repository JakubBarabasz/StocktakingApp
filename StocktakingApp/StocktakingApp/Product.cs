namespace StocktakingApp
{
    public class Product
    {
        public int ProductId { get; set; }
        public string SerialCode { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}