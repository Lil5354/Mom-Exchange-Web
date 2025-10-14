// Models/TradePost.cs
namespace B_M.Models
{
    public class TradePost
    {
        public int Id { get; set; }
        public Product ItemToOffer { get; set; } // Món đồ mẹ đang có để trao đổi
        public string DesiredItemsDescription { get; set; } // Mô tả những món đồ mẹ muốn nhận
        public string SellerName { get; set; }
        public string SellerAvatarUrl { get; set; }
        public string Location { get; set; }
    }
}