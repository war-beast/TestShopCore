using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestShopCore.Models;

namespace TestShopCore.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private DataContext db;

        public OrderRepository(DataContext context)
        {
            db = context;
        }

        public void Create(Order item)
        {
            db.Orders.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Orders.Find(id);
            if (item != null)
                db.Orders.Remove(item);
        }

        public IEnumerable<Order> Find(Func<Order, bool> predicate)
        {
            return db.Orders.Include(cs => cs.Customer).Include(cs => cs.OrderItems).Where(predicate);
        }

        public Order Get(int id)
        {
            return db.Orders.Include(cs => cs.Customer).Include(cs => cs.OrderItems).FirstOrDefault(cs => cs.Id == id);
        }

        public IEnumerable<Order> GetAll()
        {
            return db.Orders.Include(cs => cs.Customer).Include(cs => cs.OrderItems);
        }

        public void Update(Order item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}