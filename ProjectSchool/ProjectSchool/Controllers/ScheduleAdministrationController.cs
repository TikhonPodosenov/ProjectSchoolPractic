using Microsoft.AspNetCore.Mvc;
using ProjectSchool.DB;
using ProjectSchool.Models;

namespace ProjectSchool.Controllers
{
    public class ScheduleAdministrationController: Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public ScheduleAdministrationController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }
        public IActionResult Index(string subjectId, string groupId, string dayId)
        {
            ViewBag.targetUrlElectronicMagazine = _globalDataService.PathElectronicemagazine;
            ViewBag.targetUrlElectronicNews = _globalDataService.PathNews;
            ViewBag.targetUrlElectronicProfile = _globalDataService.PathProfile;
            List<ScheduleAdministration> allSchedule;
            if (string.IsNullOrEmpty(subjectId) && string.IsNullOrEmpty(groupId) && string.IsNullOrEmpty(dayId))
            {
                allSchedule = _context.GetScheduleAdministration();

                HashSet<string> subjects = new HashSet<string>();
                HashSet<string> groups = new HashSet<string>();
                HashSet<string> days = new HashSet<string>();
                List<int> id_s = new List<int>();

                if (allSchedule != null)
                {
                    foreach (var item in allSchedule)
                    {
                        if (item.Subject != null) subjects.Add(item.Subject);
                        if (item.Group != null) groups.Add(item.Group);
                        if (item.Day != null) days.Add(item.Day);
                        if (item.Id != null) id_s.Add(item.Id);
                    }
                }

                List<string> Subjects = subjects.ToList();
                List<string> Groups = groups.ToList();
                List<string> Days = days.ToList();

                var dayOrder = new Dictionary<string, int>()
                {
                    { "Понедельник", 1 },
                    { "Вторник", 2 },
                    { "Среда", 3 },
                    { "Четверг", 4 },
                    { "Пятница", 5 },
                    { "Суббота", 6 }
                };

                Days = Days.OrderBy(day => dayOrder[day]).ToList();
                _globalDataService.Subjects = ViewBag.Subjects = Subjects;
                _globalDataService.Groups = ViewBag.Groups = Groups;
                _globalDataService.Days = ViewBag.Days = Days;
                _globalDataService.Id = ViewBag.Id = id_s;
                return View(allSchedule);
            }
            else
            {
                ViewBag.Subjects = _globalDataService.Subjects;
                ViewBag.Groups = _globalDataService.Groups;
                ViewBag.Days = _globalDataService.Days;
                ViewBag.Id = _globalDataService.Id;
                if (string.IsNullOrEmpty(subjectId))
                    subjectId = "";
                if (string.IsNullOrEmpty(groupId))
                    groupId = "";
                if (string.IsNullOrEmpty(dayId))
                    dayId = "";

                allSchedule = _context.GetScheduleFilterAdministration(subjectId, groupId, dayId);
                return View(allSchedule);
            }
        }

        [HttpPost]
        public IActionResult AddSchedule(ScheduleAdministration model)
        {
            if (ModelState.IsValid)
            {
                _context.SetSchedule(model);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Subjects = _globalDataService.Subjects;
                ViewBag.Groups = _globalDataService.Groups;
                ViewBag.Days = _globalDataService.Days;
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult UpdateSchedule(string action, ScheduleAdministration model)
        {
            if (string.IsNullOrEmpty(action))
            {
                ViewBag.Subjects = _globalDataService.Subjects;
                ViewBag.Groups = _globalDataService.Groups;
                ViewBag.Days = _globalDataService.Days;
                return View("Index", model);
            }

            if (ModelState.IsValid)
            {
                int id = model.Id;
                string group = model.Group;
                string day = model.Day;
                int number = model.NumberSubject;
                string subject = model.Subject;
                if (action == "delete")
                {
                    _context.DeleteSchedule(id);
                }
                else if (action == "update")
                {
                    _context.UpdateSchedule(id, group, day, number, subject);
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Subjects = _globalDataService.Subjects;
                ViewBag.Groups = _globalDataService.Groups;
                ViewBag.Days = _globalDataService.Days;
                return View("Index", model);
            }
        }
    }
}
