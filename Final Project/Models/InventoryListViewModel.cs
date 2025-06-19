using Final_Project.Models.Entities;

namespace Final_Project.Models
{
    public class InventoryListViewModel
    {
        public List<Inventory> Items { get; set; }
        public List<ItemHistory> History { get; set; }
    }
}
