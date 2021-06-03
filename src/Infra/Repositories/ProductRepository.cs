using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(Product model)
        {
            _context.Add(model);            
        }

        public void Delete(Guid id)
        {
            var model = _context.Product.FirstOrDefault(x => x.Id == id);
            _context.Remove(model);            
        }

        public void Update(Product model)
        {
            _context.Update(model);            
        }

        public bool Exists(string name)
        {
            var model = _context.Product.FirstOrDefault(x => x.Name == name);

            if (model is null)
                return true;

            return false;
        }

        public bool ExistsUpdate(string name, Guid id)
        {
            var model = _context.Product.FirstOrDefault(x => x.Name == name && x.Id != id);

            if (model is null)
                return false;

            return true;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Product.AsNoTracking();
        }

        public Product GetById(Guid id)
        {
            return _context.Product
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Product> Search(string filter)
        {
            return _context.Product
                .AsNoTracking()
                .Where(x => x.Name.Contains(filter) || x.Price.ToString().Contains(filter));                
        }

    }
}