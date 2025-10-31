namespace DataAccess.Models
{
    public class Fruits
    {
        public int Id { get; set; }                
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
