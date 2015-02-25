using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using uLearn.Web.DataContexts;
using uLearn.Web.Models;

namespace uLearn.Web.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<ULearnDb>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(ULearnDb context)
		{
			context.Database.ExecuteSqlCommand("ALTER TABLE dbo.UserSolutions DROP CONSTRAINT [FK_dbo.UserSolutions_dbo.AspNetUsers_UserId]");
			context.Database.ExecuteSqlCommand("ALTER TABLE dbo.UserSolutions ADD CONSTRAINT [FK_dbo.UserSolutions_dbo.AspNetUsers_UserId] FOREIGN KEY (UserId) REFERENCES dbo.[AspNetUsers](Id) ON UPDATE NO ACTION ON DELETE SET NULL");

			var roleStore = new RoleStore<IdentityRole>(context);
			var roleManager = new RoleManager<IdentityRole>(roleStore);
			roleManager.Create(new IdentityRole(LmsRoles.Instructor));
			roleManager.Create(new IdentityRole(LmsRoles.Admin));
			roleManager.Create(new IdentityRole(LmsRoles.Tester));
			if (!context.Users.Any(u => u.UserName == "user"))
			{
				var userStore = new UserStore<ApplicationUser>();
				var manager = new UserManager<ApplicationUser>(userStore);
				var user = new ApplicationUser { UserName = "user" };
				manager.Create(user, "asdasd");
                manager.AddToRoleAsync(user.Id, LmsRoles.Tester);
			}
			if (!context.Users.Any(u => u.UserName == "admin"))
			{
				var userStore = new UserStore<ApplicationUser>();
				var manager = new UserManager<ApplicationUser>(userStore);
				var user = new ApplicationUser { UserName = "admin" };
				manager.Create(user, "fullcontrol");
				manager.AddToRoleAsync(user.Id, LmsRoles.Admin);
			}
			context.SaveChanges();
		}
	}
}