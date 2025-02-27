using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using MVCTutorial.Models;

namespace MVCTutorial.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            var itemList = GetItemList();
            ViewBag.ItemList = itemList;
            return View();
        }

        [HttpPost]
        public IActionResult SaveList(string ItemList)
        {
            try
            {
                var arr = ItemList.Split(",", StringSplitOptions.RemoveEmptyEntries);
                var itemList = GetItemList();
                List<MyShop> checkedItems = new();

                if (arr.Length > 0)
                    checkedItems = itemList.Where(x => arr.Contains(x.ItemID.ToString())).ToList();

                return PartialView("_checkedItems", checkedItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao processar a requisição: " + ex.Message);
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        private List<MyShop> GetItemList()
        {
            return new()
            {
                new MyShop { ItemID = 1, ItemName = "Rice", IsAvailable = false },
                new MyShop { ItemID = 2, ItemName = "Pulse", IsAvailable = false },
                new MyShop { ItemID = 3, ItemName = "Salt", IsAvailable = false },
                new MyShop { ItemID = 4, ItemName = "Soap", IsAvailable = false },
                new MyShop { ItemID = 5, ItemName = "Book", IsAvailable = false }
            };
        }
    }
}
