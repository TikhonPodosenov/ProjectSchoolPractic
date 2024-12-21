using Microsoft.AspNetCore.Mvc;
using ProjectSchool.DB;

namespace ProjectSchool.Controllers
{
    public class PersonalAccountTeacherController: Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public PersonalAccountTeacherController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }

        public IActionResult Index()
        {
            ViewBag.targetUrlElectronicMagazine = _globalDataService.PathElectronicemagazine;
            int id_aut = _globalDataService.Id_aut;
            var data_teacher = _context.GetPersonalAccountTeacher(id_aut);
            return View(data_teacher);
        }
    }
}
