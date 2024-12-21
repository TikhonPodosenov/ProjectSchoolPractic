using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSchool.DB;
using ProjectSchool.Models;

namespace ProjectSchool.Controllers
{
    public class NewsController:Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public NewsController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }
        public IActionResult Index()
        {
            var allNews = _context.GetNews();
            ViewBag.targetUrlElectronicMagazine = _globalDataService.PathElectronicemagazine;
            ViewBag.targetUrlElectronicSchedule = _globalDataService.PathSchedule;
            ViewBag.targetUrlElectronicProfile = _globalDataService.PathProfile;
            return View(allNews);
        }
    }
}
