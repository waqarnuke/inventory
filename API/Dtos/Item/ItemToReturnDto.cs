namespace API.Dtos.Item;

public class ItemToReturnDto
{
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string EMI { get; set; }
        public bool IsSingle { get; set; }
        public string ImageUrl { get; set; }
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
        public bool Status { get; set; } 
        public string Color { get; set; }
        public string Condition { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemType { get; set; }    
        public int LocationId { get; set; }
        public string Location { get; set; }
        public int MobileNetworkId { get; set; }
        public string MobileNetwork { get; set; }
        public int StorageId { get; set; }
        public string Storage { get; set; }    
}
