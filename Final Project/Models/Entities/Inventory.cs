namespace Final_Project.Models.Entities
{
    public class Inventory
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
