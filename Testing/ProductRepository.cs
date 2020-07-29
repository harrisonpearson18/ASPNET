﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Testing.Models;

namespace Testing
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _conn;

        public ProductRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _conn.Query<Product>("Select * FROM PRODUCTS;");
        }

        public Product getProduct(int id)
        {
            return (Product)_conn.QuerySingle<Product>("Select * from products where productid = @id",
                new { id = id });
        }

        public void UpdateProduct(Product product)
        {
            _conn.Execute("UPDATE products SET Name = @name, Price = @price WHERE ProductID = @id",
                new { name = product.Name, price = product.Price, id = product.ProductID });
        }
        public IEnumerable<Category> GetCategories()
        {
            return _conn.Query<Category>("SELECT * FROM categories;");
        }
        public Product AssignCategory()
        {
            var categoryList = GetCategories();
            var product = new Product();
            product.Categories = categoryList;

            return product;
        }

        public void InsertProduct(Product productToInsert)
        {
            _conn.Execute("INSERT INTO products (NAME, PRICE, CATEGORYID) VALUES (@name, @price, @categoryID);",
                new { name = productToInsert.Name, price = productToInsert.Price, categoryID = productToInsert.CategoryID });

        }

        public void DeleteProduct(Product product)
        {
            _conn.Execute("DELETE FROM REVIEWS WHERE ProductID = @id;",
                                       new { id = product.ProductID });
            _conn.Execute("DELETE FROM Sales WHERE ProductID = @id;",
                                       new { id = product.ProductID });
            _conn.Execute("DELETE FROM Products WHERE ProductID = @id;",
                                       new { id = product.ProductID });
        }

    }
}