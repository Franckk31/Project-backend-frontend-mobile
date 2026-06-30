using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.Services
{
    // Implementation ของ IProductService ที่ต่อกับ SQL Server จริงผ่าน EF Core
    // สร้างแยกจาก ProductService.cs เดิม (ซึ่งเป็น in-memory) เพื่อไม่ให้กระทบของเดิม
    // สลับมาใช้ตัวนี้ได้โดยเปลี่ยน registration ใน Program.cs:
    //   builder.Services.AddScoped<IProductService, ProductDbService>();
    public class ProductDbService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductDbService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public Product Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public bool Update(int id, Product product)
        {
            var existing = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existing is null) return false;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Stock = product.Stock;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var existing = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existing is null) return false;

            _context.Products.Remove(existing);
            _context.SaveChanges();
            return true;
        }
    }
    
    }