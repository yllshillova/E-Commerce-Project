using System.Linq;

namespace API.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public void AddItem(Product product, int quantity){
            // kushti nese ekziston produkti ne shporte nese jo ather me shtu
            if(Items.All(item => item.ProductId != product.Id)){
                Items.Add(new BasketItem{Product = product, Quantity = quantity});
                return;
            }
            var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id );
            // kushti per menaxhimin e kuantitetit te nje item te caktuar qe ekziston ne shporte
            if(existingItem != null) existingItem.Quantity +=quantity;
        }

        public void RemoveItem(int productId, int quantity)
        {
            // e marrum item masnej nese e hekum item me u zbrit kuantiteti e nese osht 0 me u fshi prej listes se items ne Basket.
            var item = Items.FirstOrDefault(item => item.ProductId == productId);
            if(item == null) return;
            item.Quantity -= quantity;
            if(item.Quantity == 0) Items.Remove(item);
        }
    }
}