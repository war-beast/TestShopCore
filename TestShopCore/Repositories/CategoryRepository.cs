using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestShop.Repositories;
using TestShopCore.Models;

namespace TestShopCore.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private DataContext db;

        public CategoryRepository(DataContext context)
        {
            db = context;
        }

        public void Create(Category item)
        {
            db.Categories.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Categories.Find(id);
            if (item != null)
                db.Categories.Remove(item);
        }

        public IEnumerable<Category> Find(Func<Category, bool> predicate)
        {
            return db.Categories.Include(ct => ct.Products).Where(predicate).ToList();
        }

        public Category Get(int id)
        {
            return db.Categories.Include(ct => ct.Products).FirstOrDefault(ct => ct.Id == id);
        }

        public IEnumerable<Category> GetAll()
        {
            return db.Categories.Include(ct => ct.Products);
        }

        public void Update(Category item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}