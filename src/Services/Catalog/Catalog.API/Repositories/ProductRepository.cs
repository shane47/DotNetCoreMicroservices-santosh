using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext dbContext;

        public ProductRepository(ICatalogContext DbContext)
        {
            dbContext = DbContext ?? throw new ArgumentException(nameof(DbContext));
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await dbContext
                            .Products
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await dbContext
                            .Products
                            .Find(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await dbContext
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await dbContext
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            await dbContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var updatedResult = await dbContext
                                        .Products
                                        .ReplaceOneAsync(filter: r => r.Id == product.Id, replacement: product);
            return updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            var res = await dbContext.Products.DeleteOneAsync(filter);
            return res.IsAcknowledged && res.DeletedCount > 0;
        }        
    }
}
