using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using uLearn.Web.DataContexts;
using uLearn.Web.Models;

namespace uLearn.Web.Controllers
{
	public class TocController : Controller
	{
		private readonly CourseManager courseManager = CourseManager.AllCourses;
		private readonly UserSolutionsRepo solutionsRepo = new UserSolutionsRepo();
		private readonly VisitersRepo visitersRepo = new VisitersRepo();
		private readonly UserQuizzesRepo userQuizzesRepo = new UserQuizzesRepo();
		
		public async Task<ActionResult> RenderTOC(string courseId, int slideIndex = -1)
		{
			var course = courseManager.GetCourse(courseId);
			var userId = User.Identity.GetUserId();
			var solvedSlides = solutionsRepo.GetIdOfPassedSlides(course.Id, userId);
			solvedSlides.UnionWith(userQuizzesRepo.GetIdOfQuizPassedSlides(course.Id, userId));
			var units = course.Slides
				.GroupBy(
					s => s.Info.UnitName,
					(unitName, slides) => new CourseUnitModel { Slides = slides.ToArray(), Title = unitName })
				.ToArray();
			var visitedSlideIds = visitersRepo.GetIdOfVisitedSlides(course.Id, userId);
			var currentSlide = course.FindSlide(slideIndex);
			var currentUnit = units.FirstOrDefault(u => u.Slides.Contains(currentSlide));
			return PartialView(new TocModel
			{
				CourseId = course.Id,
				CurrentSlide = currentSlide,
				CurrentUnit = currentUnit,
				Units = units,
				VisitedSlideIds = visitedSlideIds,
				SolvedSlideIds = solvedSlides,
			});
		}

	}
}