using Microsoft.AspNetCore.Mvc;
using ProjectSchool.DB;
using ProjectSchool.Models;

namespace ProjectSchool.Controllers
{
    public class ElectronicMagazineTeacherController : Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public ElectronicMagazineTeacherController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }

        public IActionResult Index(string groupId, string studentId)
        {
            ViewBag.targetUrlElectronicProfile = _globalDataService.PathProfile;
            ViewBag.targetUrlSchedule = _globalDataService.PathSchedule;
            ViewBag.targetUrlNews = _globalDataService.PathNews;
            List<ElectronicMagazineTeacher> all_groups;

            int id_aut = _globalDataService.Id_aut;
            if (string.IsNullOrEmpty(groupId) && string.IsNullOrEmpty(studentId))
            {
                all_groups = _context.GetElectronicMagazineTeacher(id_aut);
                HashSet<string> students = new HashSet<string>();
                HashSet<string> groups = new HashSet<string>();
                HashSet<DateOnly> dates = new HashSet<DateOnly>();
                foreach (var item in all_groups)
                {
                    if (item.Student != null) students.Add(item.Student);
                    if (item.Group != null) groups.Add(item.Group);
                    if (item.Date != null) dates.Add(item.Date);
                }
                List<String> Students = students.ToList();
                List<String> Groups = groups.ToList();
                List<DateOnly> Dates = dates.ToList();
                _globalDataService.FIOs = ViewBag.Students = Students;
                _globalDataService.Groups = ViewBag.Groups = Groups;
                _globalDataService.Dates = ViewBag.Dates = Dates;
                return View(all_groups);
            }
            else
            {
                ViewBag.Students = _globalDataService.FIOs;
                ViewBag.Groups = _globalDataService.Groups;
                ViewBag.Dates = _globalDataService.Dates;
                all_groups = _context.GetElectronicMagazineTeacherFilter(id_aut, groupId, studentId);
                return View(all_groups);
            }
        }
    }
}
