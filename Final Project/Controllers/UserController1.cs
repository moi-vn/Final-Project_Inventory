using Final_Project.Data;
using Final_Project.Models;
using Final_Project.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    [Authorize]
    public class UserController1 : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public UserController1(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: Add Item
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // POST: Add Item
        [HttpPost]
        public async Task<IActionResult> Add(AddItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var item = new Inventory
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Quantity = viewModel.Quantity,
                    Price = viewModel.Price,
                    IsAvailable = viewModel.IsAvailable
                };

                await dbContext.Inventory.AddAsync(item);

                var history = new ItemHistory
                {
                    ItemName = item.Name,
                    Action = "Added",
                    ActionDate = DateTime.Now,
                    Description = $"Added item with quantity {item.Quantity} and price {item.Price}"
                };

                await dbContext.ItemHistories.AddAsync(history);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("List");
            }

            return View(viewModel);
        }

        // GET: Inventory List + History
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var items = await dbContext.Inventory.ToListAsync();
            var history = await dbContext.ItemHistories
                .OrderByDescending(h => h.ActionDate)
                .Take(10)
                .ToListAsync();

            var viewModel = new InventoryListViewModel
            {
                Items = items,
                History = history
            };

            return View(viewModel);
        }

        // GET: Edit Item
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var inventory = await dbContext.Inventory.FindAsync(id);

            return View(inventory);
        }

        // POST: Edit Item
        [HttpPost]
        public async Task<IActionResult> Edit(Inventory viewModel)
        {
            var item = await dbContext.Inventory.FindAsync(viewModel.ID);

            if (item is not null)
            {
                item.Name = viewModel.Name;
                item.Description = viewModel.Description;
                item.Quantity = viewModel.Quantity;
                item.Price = viewModel.Price;
                item.IsAvailable = viewModel.IsAvailable;

                var history = new ItemHistory
                {
                    ItemName = item.Name,
                    Action = "Edited",
                    ActionDate = DateTime.Now,
                    Description = $"Updated item: Qty = {item.Quantity}, Price = {item.Price}"
                };

                await dbContext.ItemHistories.AddAsync(history);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List");
        }

        // POST: Delete Item
        [HttpPost]
        public async Task<IActionResult> Delete(Inventory viewModel)
        {
            var item = await dbContext.Inventory.AsNoTracking().FirstOrDefaultAsync(x => x.ID == viewModel.ID);

            if (item != null)
            {
                var history = new ItemHistory
                {
                    ItemName = item.Name,
                    Action = "Deleted",
                    ActionDate = DateTime.Now,
                    Description = $"Deleted item: {item.Description}"
                };

                dbContext.ItemHistories.Add(history);
                dbContext.Inventory.Remove(item);

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List");
        }

        // GET: Full History View
        [HttpGet]
        public async Task<IActionResult> History()
        {
            var history = await dbContext.ItemHistories
                .OrderByDescending(h => h.ActionDate)
                .ToListAsync();

            return View(history);
        }
    }
}
