using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSchool.DB;

namespace ProjectSchool.Controllers
{
    public class SchoolInformationController:Controller
    {
        private GlobalDataService _globalDataService;

        public SchoolInformationController(GlobalDataService globalDataService)
        {
            _globalDataService = globalDataService;
        }
        public IActionResult Index()
        {
            ViewBag.targetUrlElectronicMagazine = _globalDataService.PathElectronicemagazine;
            ViewBag.targetUrlProfile = _globalDataService.PathProfile;
            ViewBag.targetUrlNews = _globalDataService.PathNews;
            ViewBag.targetUrlSchedule = _globalDataService.PathSchedule;
            return View();
        }
    }
}
