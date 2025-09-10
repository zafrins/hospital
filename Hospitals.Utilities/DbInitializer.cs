using Hospital.Models;
using Hospital.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Hospitals.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                             ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            // Create roles
            string[] roles = { WebSiteRoles.Website_Admin, WebSiteRoles.Website_Patient, WebSiteRoles.Website_Doctor };
            foreach (var role in roles)
            {
                if (!_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
            }

            // Create admin
            var adminEmail = "admin@xyz.com";
            var adminUser = _userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = _userManager.CreateAsync(adminUser, "Admin@123").GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // Assign admin role
            if (!_userManager.IsInRoleAsync(adminUser, WebSiteRoles.Website_Admin).GetAwaiter().GetResult())
            {
                _userManager.AddToRoleAsync(adminUser, WebSiteRoles.Website_Admin).GetAwaiter().GetResult();
            }
        }

    }
}
