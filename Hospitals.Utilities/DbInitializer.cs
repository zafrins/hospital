using Hospital.Models;
using Hospital.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospitals.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private UserManager<IdentityUser> _userManager;  
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;

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
                // Fix the typo and use the correct method name 'GetPendingMigrations'
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }
            if (!_roleManager.RoleExistsAsync(WebSiteRoles.Website_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Patient)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Doctor)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "admin@xyz.com",
                }, "Admin@123").GetAwaiter().GetResult();
                var Appuser = _context.ApplicationUsers.FirstOrDefault(x => x.Email == "admin@xyz.com");
                if (Appuser != null)
                {
                    _userManager.AddToRoleAsync(Appuser, WebSiteRoles.Website_Admin).GetAwaiter().GetResult();

                }
            }
        }
    }
}
