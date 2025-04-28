

namespace Core.Entities
{
    public class Photo : BaseEntity
    {
        public string PictureUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        //public Product Product { get; set; }
        //public int ProductId { get; set; }
    }
}