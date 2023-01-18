using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class ApplicationRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationRepository(ApplicationDbContext db) : base(db) // Dependecy Injection of db
        {
            _db = db;
        }

    }
}
