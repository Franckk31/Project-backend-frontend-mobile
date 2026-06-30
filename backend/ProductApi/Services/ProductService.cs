using ProductApi.Models;

namespace ProductApi.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product? GetById(int id);
        Product Create(Product product);
        bool Update(int id, Product product);
        bool Delete(int id);
    }

    // ตัวอย่างนี้เก็บข้อมูลใน Memory เท่านั้น (ของจริงควรต่อกับ Database เช่น EF Core + SQL Server)
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "เมาส์ไร้สาย", Description = "เมาส์บลูทูธ", Price = 299, Stock = 50 },
            new Product { Id = 2, Name = "คีย์บอร์ดเครื่องกล", Description = "คีย์บอร์ด RGB", Price = 1290, Stock = 20 },
            new Product { Id = 3, Name = "หูฟัง", Description = "หูฟังตัดเสียงรบกวน", Price = 2590, Stock = 15 },
        };

        private int _nextId = 4;

        public IEnumerable<Product> GetAll() => _products;

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product Create(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return product;
        }

        public bool Update(int id, Product product)
        {
            var existing = GetById(id);
            if (existing is null) return false;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            return true;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing is null) return false;
            _products.Remove(existing);
            return true;
        }
    }
}
