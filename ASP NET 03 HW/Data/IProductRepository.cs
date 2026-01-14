using ASP_NET_03_HW.Models;

namespace ASP_NET_03_HW.Data;

public interface IProductRepository
{
    public Product AddProduct(Product product);
    public Task<Product> GetProductByIdAsync(int id);
    public Task<IEnumerable<Product>> GetProductsAsync();
}
