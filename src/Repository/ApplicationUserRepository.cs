using YouBank24.Data;
using YouBank24.Models;
using YouBank24.Repository.IRepository;

namespace YouBank24.Repository; 
public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository {
    private ApplicationDbContext _db;
    public ApplicationUserRepository(ApplicationDbContext db) : base(db) {
        _db = db;
    }
}
