using System;
using TestShopCore.Models;

namespace TestShopCore.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private DataContext db;
        private CategoryRepository _categoryRepository;
        private ProductRepository _productRepository;
        private CustomerRepository _customer;
        private OrderRepository _orderRepository;
        private OrderItemsRepository _orderItemsRepository;

        public UnitOfWork(DataContext context)
        {
            db = context;
        }

        public CategoryRepository Categories
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(db);
                return _categoryRepository;
            }
        }

        public ProductRepository Products
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(db);
                return _productRepository;
            }
        }

        public CustomerRepository Customer
        {
            get
            {
                if (_customer == null)
                    _customer = new CustomerRepository(db);
                return _customer;
            }
        }

        public OrderRepository Order
        {
            get
            {
                if (_orderRepository == null)
                    _orderRepository = new OrderRepository(db);
                return _orderRepository;
            }
        }

        public OrderItemsRepository OrderItem
        {
            get
            {
                if (_orderItemsRepository == null)
                    _orderItemsRepository = new OrderItemsRepository(db);
                return _orderItemsRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}