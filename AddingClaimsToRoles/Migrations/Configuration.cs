namespace AddingClaimsToRoles.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using AddingClaimsToRoles.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<AddingClaimsToRoles.Models.ProjectEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AddingClaimsToRoles.Models.ProjectEntities context)
        {
            //  This method will be called after migrating to the latest version.

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string[] roles = new string[] { "Administrators", "Employees" };
            foreach (var roleName in roles)
            {
                IdentityResult roleResult;

                // Check to see if Role Exists, if not create it
                if (!RoleManager.RoleExists(roleName))
                {
                    roleResult = RoleManager.Create(new IdentityRole(roleName));
                }
            }


            if (!context.ApplicationUsers.Any(a => a.Email == "arthur.hylton@gmail.com"))
            {
                var user = new ApplicationUser { UserName = "arthur.hylton@gmail.com", Email = "arthur.hylton@gmail.com" };

                UserManager.Create(user, "Password1!");
                UserManager.AddToRole(user.Id, "Employees");
            }

            //check if created user is in Employees role and add if not
            var empRoleID = context.IdentityRoles.First(r => r.Name == "Employees").Id; //this var will also be used later on
            var user1 = context.ApplicationUsers.Where(a => a.UserName == "arthur.hylton@gmail.com").First();

            if (!user1.Roles.Any(r => r.RoleId == empRoleID))
            {
                UserManager.AddToRole(user1.Id, "Employees");
            }

            List<IdentityClaim> Claims = new List<IdentityClaim>();
            Claims.Add(new IdentityClaim { Name = "Add", Value = "IndividualPlanItems" });
            Claims.Add(new IdentityClaim { Name = "Edit", Value = "IndividualPlanItems" });

            foreach (var claim in Claims)
            {
                if (!context.IdentityClaims.Any(c => c.Name == claim.Name && c.Value == claim.Value))
                {
                    context.IdentityClaims.Add(claim);
                }
            }
            context.SaveChanges();


            //check if Employees role can "Add IndividualPlanItems"
            if (!context.IdentityClaims.First(c => c.Name == "Add" && c.Value == "IndividualPlanItems").IdentityRoles.Any(r => r.Name == "Employees"))
            {
                //Employees cannot "Add IndividualPlanItems" so let's fix that
                context.IdentityClaims.First(c => c.Name == "Add" && c.Value == "IndividualPlanItems").IdentityRoles.Add(context.IdentityRoles.First(r => r.Name == "Employees"));
                context.SaveChanges();
            }
        }
    }
}
