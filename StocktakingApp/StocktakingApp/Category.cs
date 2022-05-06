using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace StocktakingApp
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string SerialCode { get; set; }
        public int Amount
        {
            get;
            set;
        }


        public virtual ICollection<Product>
            Products
        { get; private set; } =
            new ObservableCollection<Product>();
    }
}