using Core.DbModels;

namespace MyCourseWork.Models.CartModels
{
    public class CartItem : BaseEntity
    {
        public Product Product { get; set; }
        
        public float Price { get; set; }
        
        public int Quantity { get; set; }
        
        public string CartProductId { get; set; }
    }
}