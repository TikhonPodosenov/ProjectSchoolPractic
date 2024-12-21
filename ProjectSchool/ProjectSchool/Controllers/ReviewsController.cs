using Microsoft.AspNetCore.Mvc;
using ProjectSchool.DB;
using ProjectSchool.Models;

namespace ProjectSchool.Controllers
{
    public class ReviewsController:Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public ReviewsController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }
        public IActionResult Index(string postId)
        {
            ViewBag.targetUrlElectronicMagazine = _globalDataService.PathElectronicemagazine;
            ViewBag.targetUrlElectronicSchedule = _globalDataService.PathSchedule;
            ViewBag.targetUrlElectronicProfile = _globalDataService.PathProfile;
            ViewBag.targetUrlElectronicNews = _globalDataService.PathNews;
            ViewBag.Post = _globalDataService.Post;

            List<Reviews> all_reviews;
            if (string.IsNullOrEmpty(postId)){
                HashSet<string> posts1 = new HashSet<string>();
                all_reviews = _context.GetReviews();
                foreach(var item in all_reviews)
                {
                    if (item.Post != null) posts1.Add(item.Post);
                }
                List<string> posts2 = posts1.ToList();
                _globalDataService.Posts = ViewBag.Posts = posts2;
                return View(all_reviews);
            }
            else
            {
                ViewBag.Posts = _globalDataService.Posts;
                all_reviews = _context.GetReviewsFilter(postId);
                return View(all_reviews);
            }
        }

        [HttpPost]
        public IActionResult Index(Reviews model)
        {
            _context.SetReview(model, _globalDataService.Id_aut);
            return RedirectToAction("Index");
        }
    }
}
