using Microsoft.EntityFrameworkCore;
using MyProject.Context;
using MyProject.Dtos;
using MyProject.Interfaces;
using MyProject.Models;

namespace MyProject.Repository
{
    public class ProductOperation : IProduct
    {
        private readonly AppDbContext _context;
        public ProductOperation(AppDbContext context)
        {
            _context = context;   
        }
        public async Task<Product> AddProduct(Product product)
        {
            var result = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Product> DeleteProduct(string id)
        {
            var result = await _context.Products.FirstOrDefaultAsync(e => e.ProductId.Equals(id));
            if (result != null)
            {
                _context.Products.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<List<Product>> GetAllProducts(string id,FormQuery formQuery)
        {
            var skipNumber = (formQuery.PageNumber-1)*formQuery.PageSize;
            Console.WriteLine($"Skip Number: {skipNumber} {formQuery.PageNumber}");
            var products = await _context.Products.Skip(skipNumber).Take(formQuery.PageSize).Where(e => e.AppUserId.Equals(id)).ToListAsync();
            return products;
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products.FirstOrDefaultAsync(e => e.ProductId.Equals(id));
        }

        public async Task<int> GetProductCount(string id)
        {
            return await _context.Products.CountAsync(e => e.AppUserId == id);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = await _context.Products.FirstOrDefaultAsync(e => e.ProductId == product.ProductId);
            if(result != null)
            {
                result.ProductId = product.ProductId;
                result.ProductName = product.ProductName;
                result.ProductPrice = product.ProductPrice;
                result.ProductDescription = product.ProductDescription;
                result.ProductQuantity = product.ProductQuantity;

                await _context.SaveChangesAsync();
                return result;

            }
            return null;
        }
    }
}
