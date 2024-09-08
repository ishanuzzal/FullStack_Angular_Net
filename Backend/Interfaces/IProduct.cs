using MyProject.Dtos;
using MyProject.Models;

namespace MyProject.Interfaces
{
    public interface IProduct
    {
        public Task<Product> AddProduct(Product product);
        public Task<Product> UpdateProduct(Product model);
        
        public Task<List<Product>> GetAllProducts(string id, FormQuery formQuery);
        public Task<Product> GetProductById(string id);

        public Task<int> GetProductCount(string id);

        public Task<Product> DeleteProduct(string id);
    }
}
