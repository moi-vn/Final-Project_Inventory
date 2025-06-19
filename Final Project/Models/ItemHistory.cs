namespace Final_Project.Models
{
    public class ItemHistory
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public string? Description { get; set; }
    }
}
