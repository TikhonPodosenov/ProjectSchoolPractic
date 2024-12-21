using Microsoft.AspNetCore.Mvc;
using ProjectSchool.DB;
using ProjectSchool.Models;

namespace ProjectSchool.Controllers
{
    public class ElectronicMagazineController:Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public ElectronicMagazineController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }

        public IActionResult Index(string subjectId)
        {
            ViewBag.targetUrlElectronicProfile = _globalDataService.PathProfile;
            List<ElectronicMagazine> all_subjects;

            int id_aut = _globalDataService.Id_aut;
            if (string.IsNullOrEmpty(subjectId))
            {
                HashSet<string> subjects = new HashSet<string>();
                HashSet<DateOnly> dates = new HashSet<DateOnly>();
                all_subjects = _context.GetElectronicMagazine(id_aut);
                foreach (var item in all_subjects)
                {
                    if (item.Subject != null) subjects.Add(item.Subject);
                    if (item.Date != null) dates.Add(item.Date);
                }
                List<string> Subjects = subjects.ToList();
                List<DateOnly> Dates = dates.ToList();
                _globalDataService.Subjects = ViewBag.Subjects = Subjects;
                _globalDataService.Dates = ViewBag.Dates = Dates;
                return View(all_subjects);
            }
            else
            {
                ViewBag.Subjects = _globalDataService.Subjects;
                ViewBag.Dates = _globalDataService.Dates;
                all_subjects = _context.GetElectronicMagazineFilter(id_aut, subjectId);
                return View(all_subjects);
            }
        }
    }
}
