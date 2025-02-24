using Microsoft.AspNetCore.Http;

namespace MVCTutorial.Models
{
    public class ProductViewModel
    {
        public string ProductName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<int> ImageID { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
