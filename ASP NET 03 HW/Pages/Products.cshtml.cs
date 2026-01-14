using ASP_NET_03_HW.Models;
using ASP_NET_03_HW.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_NET_03_HW.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly ProductService _service;
        public ProductsModel(ProductService service)
        {
            _service = service;
        }
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public async Task OnGetAsync()
        {
            Products = await _service.GetAllProductsAsync();
        }
    }
}
