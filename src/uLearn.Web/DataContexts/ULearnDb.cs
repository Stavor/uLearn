using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using uLearn.Web.Migrations;
using uLearn.Web.Models;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts
{
	public class ULearnDb : IdentityDbContext<ApplicationUser>
	{
		public ULearnDb()
			: base("DefaultConnection")
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<ULearnDb, Configuration>());

		}

		public DbSet<UserSolution> UserSolutions { get; set; }
		public DbSet<UserQuestion> UserQuestions { get; set; }
		public DbSet<SlideRate> SlideRates { get; set; }
		public DbSet<Visiters> Visiters { get; set; }
		public DbSet<SlideHint> Hints { get; set; }
		public DbSet<Like> SolutionLikes { get; set; }
		public DbSet<UserQuiz> UserQuizzes { get; set; }
		public DbSet<UnitAppearance> Units { get; set; }
		public DbSet<TextBlob> Texts { get; set; }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Proposition>  Propositions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Mark> Marks { get; set; }
	}
}