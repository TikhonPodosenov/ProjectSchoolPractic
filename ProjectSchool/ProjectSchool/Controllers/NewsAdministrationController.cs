using Microsoft.AspNetCore.Mvc;
using ProjectSchool.DB;
using ProjectSchool.Models;

namespace ProjectSchool.Controllers
{
    public class NewsAdministrationController: Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public NewsAdministrationController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }
        public IActionResult Index()
        {
            var allNews = _context.GetNewsAdministration();
            ViewBag.targetUrlElectronicMagazine = _globalDataService.PathElectronicemagazine;
            ViewBag.targetUrlElectronicSchedule = _globalDataService.PathSchedule;
            ViewBag.targetUrlElectronicProfile = _globalDataService.PathProfile;
            return View(allNews);
        }

        [HttpPost]
        public IActionResult Index(NewsAdministration model)
        {
            _context.SetNew(model);
            return RedirectToAction("Index");
        }
    }
}
