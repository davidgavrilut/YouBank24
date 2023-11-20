using YouBank24.Data;
using YouBank24.Models;
using YouBank24.Repository.IRepository;
using static NHibernate.Engine.Query.CallableParser;

namespace YouBank24.Repository; 
public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository {
    private ApplicationDbContext _db;
    public ApplicationUserRepository(ApplicationDbContext db) : base(db) {
        _db = db;
    }

    public ApplicationUser GetUserById(string userId)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(userId));
        }
        return user;
    }

    public ApplicationUser GetUserByEmail(string email)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(email));
        }
        return user;
    }

    public IEnumerable<ApplicationUser> GetAllUsersExceptCurrentUser(string currentUserId)
    {
        IEnumerable<ApplicationUser> users = _db.ApplicationUsers.Where(u => u.Id != currentUserId);
        if (users.Count() == 0)
        {
            throw new ArgumentNullException(nameof(currentUserId));
        }
        return users.ToList();
    }
}
