using System;
using Identity.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure;

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}

public class IdentityContextSeed: IDbSeeder<IdentityContext>
{
    public async Task SeedAsync(IdentityContext context)
    {
        if (!await context.Users.AnyAsync()) 
        {
            var permissionViewReport = new Permission();
            permissionViewReport.Name = "ViewReport";

            var permissionSendReport = new Permission();
            permissionSendReport.Name = "SendReport";

            await context.AddRangeAsync([permissionViewReport, permissionSendReport]);
            await context.SaveChangesAsync();

            var roleAdminReport = new Role();
            roleAdminReport.Name = "AdminReport";

            var roleStaffReport = new Role();
            roleStaffReport.Name = "StaffReport";
            roleStaffReport.RolePermissions.Add(new RolePermission() { Role = roleStaffReport, Permission = permissionViewReport });

            var roleAdmin = new Role();
            roleAdmin.Name = "admin";

            await context.AddRangeAsync([roleAdminReport, roleStaffReport, roleAdmin]);
            await context.SaveChangesAsync();

            var admin = new User();
            admin.Username = "admin";
            admin.Password = "admin123";
            admin.UserRoles.Add(new UserRole() { User = admin, Role = roleAdmin } );

            var adminReport = new User();
            adminReport.Username = "adminreport";
            adminReport.Password = "adminreport123";
            adminReport.UserRoles.Add(new UserRole() { User = adminReport, Role = roleAdminReport } );

            var staffReport = new User();
            staffReport.Username = "staffreport";
            staffReport.Password = "staffreport123";
            staffReport.UserRoles.Add(new UserRole() { User = staffReport, Role = roleStaffReport } );

            var users = new List<User>() 
            { 
                admin, adminReport, staffReport
            };

            await context.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}