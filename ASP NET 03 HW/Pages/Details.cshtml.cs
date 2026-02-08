using ASP_NET_03_HW.Models;
using ASP_NET_03_HW.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_NET_03_HW.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ProductService _service;
        public DetailsModel(ProductService service)
        {
            _service = service;
        }
        public Product SelectedProduct { get; set; }
        public async Task OnGet(int id) =>
            SelectedProduct = await _service.GetProductByIdAsync(id);
    }
}
