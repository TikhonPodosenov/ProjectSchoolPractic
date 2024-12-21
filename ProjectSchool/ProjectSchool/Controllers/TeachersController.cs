using Microsoft.AspNetCore.Mvc;
using ProjectSchool.DB;
using ProjectSchool.Models;
using System.Text.RegularExpressions;

namespace ProjectSchool.Controllers
{
    public class TeachersController:Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public TeachersController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }
        public IActionResult Index(string subjectId, string fioId)
        {
            ViewBag.targetUrlElectronicMagazine = _globalDataService.PathElectronicemagazine;
            ViewBag.targetUrlElectronicSchedule = _globalDataService.PathSchedule;
            ViewBag.targetUrlElectronicProfile = _globalDataService.PathProfile;
            ViewBag.targetUrlElectronicNews = _globalDataService.PathNews;

            ViewBag.subject_select = subjectId;
            ViewBag.fio_select = fioId;

            List<Teachers> allTeacher;
            if (string.IsNullOrEmpty(subjectId) && string.IsNullOrEmpty(fioId))
            {
                allTeacher = _context.GetTeachers();

                HashSet<string> subjects = new HashSet<string>();
                HashSet<string> fios = new HashSet<string>();

                if (allTeacher != null)
                {
                    foreach (var item in allTeacher)
                    {
                        if (item.Subject != null) subjects.Add(item.Subject);
                        if (item.FIO != null) fios.Add(item.FIO);
                    }
                }

                List<string> Subjects = subjects.ToList();
                List<string> Fios = fios.ToList();

                _globalDataService.Subjects = ViewBag.Subjects = Subjects;
                _globalDataService.FIOs = ViewBag.FIOs = Fios;

                return View(allTeacher);
            }
            else
            {
                ViewBag.Subjects = _globalDataService.Subjects;
                ViewBag.FIOs = _globalDataService.FIOs;
                if (string.IsNullOrEmpty(subjectId))
                    subjectId = "";
                if (string.IsNullOrEmpty(fioId))
                    fioId = "";

                allTeacher = _context.GetTeachersFilter(subjectId, fioId);
                return View(allTeacher);
            }
        }
    }
}
