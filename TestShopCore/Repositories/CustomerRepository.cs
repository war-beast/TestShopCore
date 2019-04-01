using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestShopCore.Models;

namespace TestShopCore.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private DataContext db;

        public CustomerRepository(DataContext context)
        {
            db = context;
        }

        public Customer Get(int id)
        {
            return db.Customers.Include(cs => cs.Orders).FirstOrDefault(cst => cst.Id == id);
        }

        public void Create(Customer item)
        {
            db.Customers.Add(item);
        }

        public IEnumerable<Customer> GetAll()
        {
            return db.Customers.Include(cs => cs.Orders);
        }

        public IEnumerable<Customer> Find(Func<Customer, bool> predicate)
        {
            return db.Customers.Include(cs => cs.Orders).Where(predicate);
        }

        public void Update(Customer item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = db.Customers.Find(id);
            if (item != null)
                db.Customers.Remove(item);
        }
    }
}