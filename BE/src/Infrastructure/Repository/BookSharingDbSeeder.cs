using Infrastructure.Persistance;
using Infrastructure.Repository.Implementations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class BookSharingDbSeeder
    {
        private readonly BookSharingContext _context;

        public BookSharingDbSeeder(BookSharingContext context)
        {
            _context = context;
        }

        public async Task<bool> SeedUserRoles(params string[] roles)
        {
            bool addedNewRole = false;

            foreach (var role in roles)
            {
                if (!_context.Roles.Any(r => r.Name == role))
                {
                    _context.Roles.Add(new Role { Name = role, NormalizedName = role });
                    addedNewRole = true;
                }
            }
            await _context.SaveChangesAsync();
            return addedNewRole;
        }

        public async Task<bool> SeedAdmin(string firstname, string lastname, string username, string password, string email, params string[] roles)
        {
            if (!_context.Users.Any(u => u.UserName == username))
            {
                var user = new User
                {
                    FirstName = firstname,
                    LastName = lastname,
                    UserName = username,
                    NormalizedUserName = username,
                    Email = email,
                    NormalizedEmail = email,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var hashed = new PasswordHasher<User>().HashPassword(user, password);
                user.PasswordHash = hashed;
                var userStore = new UserStoreRepository(_context);
                await userStore.CreateAsync(user);

                foreach (var role in roles)
                {
                    await userStore.AddToRoleAsync(user, role);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
