using System.Text.RegularExpressions;

namespace ProjectSchool
{
    public class GlobalDataService
    {
        private int id_aut;

        private string post;

        private string surname;

        private string name;

        private string patronymic;

        private string group;

        private string numberOfparent;

        private string subject;

        private string pathProfile;

        private string pathElectronicemagazine;

        private string pathSchedule;

        private string pathNews;

        private List<string> subjects;

        private List<string> groups;

        private List<string> days;

        private List<string> posts;

        private List<string> fio;

        private List<DateOnly> dates;

        private List<int> id;

        public int Id_aut { get { return id_aut; } set { id_aut = value; } }

        public string Post { get { return post; } set { post = value; } }

        public string Surname { get { return surname; } set { surname = value; } }

        public string Name { get { return name; } set { name = value; } }

        public string Patronymic { get { return patronymic; } set { patronymic = value; } }

        public string Group { get { return group; } set { group = value; } }

        public string Subject { get { return subject; } set { subject = value; } }

        public string NumberOfParent { get { return numberOfparent; } set { numberOfparent = value; } }

        public string PathProfile {  get { return pathProfile; } set { pathProfile = value; } }

        public string PathElectronicemagazine {  get { return pathElectronicemagazine; } set {pathElectronicemagazine = value; } }

        public string PathSchedule {  get { return pathSchedule; } set {pathSchedule = value; } }

        public string PathNews {  get { return pathNews; } set {pathNews = value; } }

        public List<string> Subjects { get { return subjects; } set {subjects = value; } }

        public List<string> Groups { get { return groups; } set { groups = value; } }

        public List<string> Days { get { return days; } set { days = value; } }

        public List<string> Posts { get { return posts; } set { posts = value; } }

        public List<string> FIOs { get { return fio; } set { fio = value; } }

        public List<DateOnly> Dates { get { return dates; } set { dates = value; } }

        public List<int> Id { get { return id; } set { id = value; } }
    }
}
