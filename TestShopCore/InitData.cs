using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestShopCore.Models;

namespace TestShopCore
{
    public static class InitData
    {
        public static async Task Initialize(DataContext context)
        {
            if (!context.Products.Any())
            {
                context.Categories.Add(new Category
                {
                    Name = "Смартфоны",
                    Products = new List<Product> {
                        new Product { Name = "Samsung Galaxy A7", Price = 600.6M, Rating = 1, Photo = "/Photo/30042525m.jpg", Description = "Описание товара." },
                        new Product { Name = "Huawei P20 Lite", Price = 660, Rating = 3, Photo = "/Photo/30042521m.jpg", Description = "Описание товара." },
                        new Product { Name = "Apple iPhone XS Max", Price = 900, Rating = 5, Photo = "/Photo/30040019m.jpg", Description = "Описание товара." }
                    }
                });
                context.Categories.Add(new Category
                {
                    Name = "Компьютеры",
                    Products = new List<Product> {
                        new Product { Name = "Apple MacBook 12 Core M3", Price = 1000, Rating = 3, Photo = "/Photo/30028509m.jpg", Description = "Описание товара." },
                        new Product { Name = "Irbis NB211", Price = 630.35M, Rating = 2, Photo = "/Photo/30028509m.jpg", Description = "Описание товара." }
                    }
                });
                context.Categories.Add(new Category
                {
                    Name = "Бытовая техника",
                    Products = new List<Product> {
                        new Product { Name = "Робот-пылесос Tefal Smart Force Extreme RG7145RH", Price = 1000, Rating = 4, Photo="/Photo/20059428m.jpg", Description = "Описание товара." },
                        new Product { Name = "Робот-пылесос Philips FC8796/01", Price = 680, Rating = 2, Photo="/Photo/20059428m.jpg", Description = "Описание товара." },
                        new Product { Name = "Кофеварка гейзерная Rondell RDS-499", Price = 940, Rating = 3, Photo="/Photo/50045503m.jpg", Description = "Описание товара." },
                        new Product { Name = "Холодильник Hotpoint-Ariston HF 9201 X RO", Price = 1300, Rating = 5, Photo="/Photo/20032511m.jpg", Description = "Описание товара." }
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
