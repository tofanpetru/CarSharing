using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Repository.Implementations
{
    public class UserStoreRepository : UserStore<User, Role, BookSharingContext, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>
    {
        public UserStoreRepository(BookSharingContext context) : base(context) { }
    }
}
