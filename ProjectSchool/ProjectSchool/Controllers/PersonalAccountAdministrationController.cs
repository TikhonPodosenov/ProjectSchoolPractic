using Microsoft.AspNetCore.Mvc;
using ProjectSchool.DB;

namespace ProjectSchool.Controllers
{
    public class PersonalAccountAdministrationController: Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public PersonalAccountAdministrationController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }

        public IActionResult Index()
        {
            int id_aut = _globalDataService.Id_aut;
            ViewBag.targetUrlElectronicMagazine = _globalDataService.PathElectronicemagazine;
            ViewBag.targetUrlSchedule = _globalDataService.PathSchedule;
            ViewBag.targetUrlNews = _globalDataService.PathNews;
            var data_administrater = _context.GetPersonalAccountAdministration(id_aut);
            return View(data_administrater);
        }
        
    }
}
