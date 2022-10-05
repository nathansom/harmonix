using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},               
                new AppRole{Name = "Piano"},    // Free Account
                new AppRole{Name = "Forte"},    // Tier 1 Subscription
                new AppRole{Name = "Fortissimo"},  // Tier 2 Subscription
                new AppRole{Name = "Pianissimo"},   // Tier 2 Subscription (temporary day free trial)
                new AppRole{Name = "OrgAdmin"},
                new AppRole{Name = "OrgModerator"},
                new AppRole{Name = "OrgMember"},
                new AppRole{Name = "Recruiter"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);                
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
                await userManager.AddToRoleAsync(user, "Pianissimo");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator", "Forte"});
        }


        public static async Task SeedJobs(DataContext context)
        {
            if( await context.Jobs.AnyAsync()) return;

            var jobData = await System.IO.File.ReadAllTextAsync("Data/jobSeedDataUpdated.json");
            var jobs = JsonSerializer.Deserialize<List<Job>>(jobData);
            foreach(var job in jobs)
            {
                context.Jobs.Add(job);
            }
            await context.SaveChangesAsync();
        }

        public static async Task SeedOrganizations(DataContext context)
        {
            if (await context.Organizations.AnyAsync())return; 

            var OrganizationData = await System.IO.File.ReadAllTextAsync("Data/OrgnizationSeedData.json");
            var organizations = JsonSerializer.Deserialize<List<Organization>>(OrganizationData);
            
            foreach(var organization in organizations)
            {
                context.Organizations.Add(organization);
            }

            await context.SaveChangesAsync();
        }

    }
}