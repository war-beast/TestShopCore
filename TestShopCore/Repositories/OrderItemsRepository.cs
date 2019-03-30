using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestShopCore.Models;

namespace TestShopCore.Repositories
{
    public class OrderItemsRepository : IRepository<OrderItem>
    {
        private DataContext db;

        public OrderItemsRepository(DataContext context)
        {
            db = context;
        }

        public void Create(OrderItem item)
        {
            db.OrderItems.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.OrderItems.Find(id);
            if (item != null)
                db.OrderItems.Remove(item);
        }

        public IEnumerable<OrderItem> Find(Func<OrderItem, bool> predicate)
        {
            return db.OrderItems.Include(cs => cs.Product).Where(predicate);
        }

        public OrderItem Get(int id)
        {
            return db.OrderItems.Include(cs => cs.Product).FirstOrDefault(cs => cs.Id == id);
        }

        public IEnumerable<OrderItem> GetAll()
        {
            return db.OrderItems.Include(cs => cs.Product);
        }

        public void Update(OrderItem item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}