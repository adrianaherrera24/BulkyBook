using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db) // Dependecy Injection of db
        {
            _db = db;
        }

        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
