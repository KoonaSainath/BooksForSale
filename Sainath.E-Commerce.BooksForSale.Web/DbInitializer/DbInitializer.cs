using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using System.Configuration;

namespace Sainath.E_Commerce.BooksForSale.Web.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly BooksForSaleDbContext dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration configuration;
        public DbInitializer(BooksForSaleDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            this.configuration = configuration;
        }
        public void Initialize()
        {
            //check if there are any pending migrations to be updated and update them to database
            if (dbContext.Database.GetPendingMigrationsAsync().Result.Count() > 0)
            {
                dbContext.Database.MigrateAsync();
            }

            //create all roles here rather than like we did in Register.cshtml
            if (!_roleManager.RoleExistsAsync(GenericConstants.ROLE_ADMIN).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(GenericConstants.ROLE_ADMIN)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(GenericConstants.ROLE_EMPLOYEE)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(GenericConstants.ROLE_COMPANY_CUSTOMER)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(GenericConstants.ROLE_CUSTOMER)).GetAwaiter().GetResult();
            }

            //create a default admin account so that atleast one admin account will be created by default who can create any number of admin/company/employee accounts
            string EMAIL = configuration["AdminUserAccount:Email"] ?? string.Empty;
            string PASSWORD = configuration["AdminUserAccount:Password"] ?? string.Empty;

            BooksForSaleUser adminUserFromDb = dbContext.BooksForSaleUsers.Where(user => user.Email == EMAIL).FirstOrDefault();

            if(adminUserFromDb == null)
            {
                BooksForSaleUser user = new BooksForSaleUser();
                user.UserName = EMAIL;
                user.Name = "Default admin";
                user.Email = EMAIL;
                user.PhoneNumber = "9999999999";
                user.StreetAddress = "admin street";
                user.City = "admin city";
                user.State = "admin state";
                user.PostalCode = "000000";

                IdentityResult result = _userManager.CreateAsync(user, PASSWORD).GetAwaiter().GetResult();

                adminUserFromDb = dbContext.BooksForSaleUsers.Where(user => user.Email == EMAIL).FirstOrDefault();
                _userManager.AddToRoleAsync(adminUserFromDb, GenericConstants.ROLE_ADMIN).GetAwaiter().GetResult();
            }
        }
    }
}
