using YouBank24.Models;

namespace YouBank24.Repository.IRepository; 
public interface IApplicationUserRepository : IRepository<ApplicationUser> {
    ApplicationUser GetUserById(string id);
    ApplicationUser GetUserByEmail(string email);
    IEnumerable<ApplicationUser> GetAllUsersExceptCurrentUser(string currentUserId);
}
