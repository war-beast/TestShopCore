using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestShopCore.Models;

namespace TestShopCore.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private DataContext db;

        public ProductRepository(DataContext context)
        {
            db = context;
        }

        public void Create(Product item)
        {
            db.Products.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
                db.Products.Remove(item);
        }

        public IEnumerable<Product> Find(Func<Product, bool> predicate)
        {
            return db.Products.Include(pr => pr.Category).Where(predicate).ToList();
        }

        public Product Get(int id)
        {
            return db.Products.Include(pr => pr.Category).FirstOrDefault(pr => pr.Id == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Products.Include(pr => pr.Category);
        }

        public void Update(Product item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}