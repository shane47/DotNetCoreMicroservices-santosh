using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration config;
        private readonly NpgsqlConnection connection;
        public DiscountRepository(IConfiguration config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }
        public async Task<Coupon> GetDiscount(string productName)
        {
            string query = "SELECT * FROM Coupon WHERE productname = @ProductName";
            using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(query, new { ProductName = productName });
            if (null == coupon)
                return new Coupon
                {
                    ProductName = "No Product",
                    Amount = 0,
                    Description = "No Discount"
                };
            
            return coupon;
        }
        
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
            string query = "INSERT INTO Coupon (id, productname, description, amount) VALUES (@id, @ProductName, @Description, @Amount)";
            
            var affected = await connection.ExecuteAsync(query, new { id=coupon.Id, ProductName = coupon.ProductName,
            Description = coupon.Description, Amount = coupon.Amount });

            if (0 == affected)
                return false;
            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
            string query = "UPDATE Coupon SET productname=@ProductName, description = @Description, amount = @Amount WHERE id = @Id";

            var affected = await connection.ExecuteAsync(query, new
            {
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
                Id = coupon.Id
            });

            if (0 == affected)
                return false;
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
            string query = "DELETE FROM Coupon WHERE productname = @ProductName";

            var affected = await connection.ExecuteAsync(query, new { ProductName = productName});

            if (0 == affected)
                return false;

            return true;
        }
    }
}
