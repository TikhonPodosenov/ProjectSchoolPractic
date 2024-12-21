using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProjectSchool.Models;

namespace ProjectSchool.DB
{
    public class ApplicationDbContext : DbContext, ISqlQueryExecutor
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public List<T> ExecuteQuery<T>(string sql, params object[] parameters)
        {
            return this.Database.SqlQueryRaw<T>(sql, parameters).ToList();
        }

        private readonly ISqlQueryExecutor _sqlQueryExecutor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ISqlQueryExecutor sqlQueryExecutor) : base(options)
        {
            _sqlQueryExecutor = sqlQueryExecutor;
        }

        //public List<PersonalAccount> GetPersonalAccount(int id_aut)
        //{
        //    return _sqlQueryExecutor.ExecuteQuery<PersonalAccount>(
        //        "EXEC SelectStudent @id_aut",
        //       new SqlParameter("@id_aut", id_aut)
        //    );
        //}
        public List<PersonalAccount> GetPersonalAccount(int id_aut)
        {
            return this.Database.SqlQueryRaw<PersonalAccount>("EXEC SelectStudent @id_aut",
                new Microsoft.Data.SqlClient.SqlParameter("@id_aut", id_aut)).ToList();
        }

        public List<PersonalAccountTeacher> GetPersonalAccountTeacher(int id_aut)
        {
            return this.Database.SqlQueryRaw<PersonalAccountTeacher>("EXEC SelectTeacher @id_aut",
                new Microsoft.Data.SqlClient.SqlParameter("@id_aut", id_aut)).ToList();
        }

        public List<PersonalAccountAdministration> GetPersonalAccountAdministration(int id_aut)
        {
            return this.Database.SqlQueryRaw<PersonalAccountAdministration>("EXEC SelectAdministrator @id_aut",
                new Microsoft.Data.SqlClient.SqlParameter("@id_aut", id_aut)).ToList();
        }

        //public List<DataAutorization> VerificationOfAuthorization(string login, string password)
        //{
        //    return _sqlQueryExecutor.ExecuteQuery<DataAutorization>(
        //         "EXEC VerificationOfAuthorization @login, @password",
        //       new SqlParameter("@login", login),
        //        new SqlParameter("@password", password)
        //     );
        //}

        public List<DataAutorization> VerificationOfAuthorization(string login, string password)
        {
            return this.Database.SqlQueryRaw<DataAutorization>("EXEC VerificationOfAuthorization @login, @password",
                new Microsoft.Data.SqlClient.SqlParameter("@login", login),
                new Microsoft.Data.SqlClient.SqlParameter("@password", password)).ToList();
        }

        public List<News> GetNews()
        {
            return this.Database.SqlQueryRaw<News>("EXEC SelectNews").ToList();
        }

        public List<NewsAdministration> GetNewsAdministration()
        {
            return this.Database.SqlQueryRaw<NewsAdministration>("EXEC SelectNews").ToList();
        }

        public List<Reviews> GetReviews()
        {
            return this.Database.SqlQueryRaw<Reviews>("EXEC SelectReviews").ToList();
        }

        public List<Reviews> GetReviewsFilter(string post)
        {
            return this.Database.SqlQueryRaw<Reviews>("EXEC SelectReviewsFilter @post",
                new Microsoft.Data.SqlClient.SqlParameter("@post", post)).ToList();
        }

        public List<Teachers> GetTeachers()
        {
            return this.Database.SqlQueryRaw<Teachers>("EXEC SelectTeachers").ToList();
        }

        public List<Teachers> GetTeachersFilter(string subject, string fio)
        {
            return this.Database.SqlQueryRaw<Teachers>("EXEC SelectTeachersFilter @subject, @fio",
                 new Microsoft.Data.SqlClient.SqlParameter("@subject", subject),
                 new Microsoft.Data.SqlClient.SqlParameter("@fio", fio)).ToList();
        }

        public List<Schedule> GetSchedule()
        {
            return this.Database.SqlQueryRaw<Schedule>("EXEC SelectSchedule").ToList();
        }

        //public List<Schedule> GetScheduleFilter(string subject, string group, string day)
        //{
        //    return _sqlQueryExecutor.ExecuteQuery<Schedule>(
        //         "EXEC SelectScheduleFilter @subject, @group, @day",
        //          new SqlParameter("@subject", subject),
        //           new SqlParameter("@group", group),
        //         new SqlParameter("@day", day)
        //    );
        //}
        public List<Schedule> GetScheduleFilter(string subject, string group, string day)
        {
            return this.Database.SqlQueryRaw<Schedule>("EXEC SelectScheduleFilter @subject, @group, @day",
                new Microsoft.Data.SqlClient.SqlParameter("@subject", subject),
                new Microsoft.Data.SqlClient.SqlParameter("@group", group),
                new Microsoft.Data.SqlClient.SqlParameter("@day", day)).ToList();
        }

        public List<ScheduleAdministration> GetScheduleAdministration()
        {
            return this.Database.SqlQueryRaw<ScheduleAdministration>("EXEC SelectSchedule").ToList();
        }

        public List<ScheduleAdministration> GetScheduleFilterAdministration(string subject, string group, string day)
        {
            return this.Database.SqlQueryRaw<ScheduleAdministration>("EXEC SelectScheduleFilter @subject, @group, @day",
                new Microsoft.Data.SqlClient.SqlParameter("@subject", subject),
                new Microsoft.Data.SqlClient.SqlParameter("@group", group),
                new Microsoft.Data.SqlClient.SqlParameter("@day", day)).ToList();
        }

        public List<ElectronicMagazine> GetElectronicMagazine(int id_aut)
        {
            return this.Database.SqlQueryRaw<ElectronicMagazine>("EXEC SelectElectronicMagazine @id_aut",
                new Microsoft.Data.SqlClient.SqlParameter("@id_aut", id_aut)).ToList();
        }

        public List<ElectronicMagazineTeacher> GetElectronicMagazineTeacher(int id_aut)
        {
            return this.Database.SqlQueryRaw<ElectronicMagazineTeacher>("EXEC SelectElectroniceMagazineTeacher @id_aut",
                new Microsoft.Data.SqlClient.SqlParameter("@id_aut", id_aut)).ToList();
        }

        public List<ElectronicMagazineTeacher> GetElectronicMagazineTeacherFilter(int id_aut, string group, string fio)
        {
            return this.Database.SqlQueryRaw<ElectronicMagazineTeacher>("EXEC SelectElectroniceMagazineTeacherFilter @id_aut, @group, @fio",
                new Microsoft.Data.SqlClient.SqlParameter("@id_aut", id_aut),
                new Microsoft.Data.SqlClient.SqlParameter("@group", group),
                 new Microsoft.Data.SqlClient.SqlParameter("@fio", string.IsNullOrEmpty(fio) ? (object)DBNull.Value : fio)).ToList();
        }

        public List<ElectronicMagazine> GetElectronicMagazineFilter(int id_aut, string subject)
        {
            return this.Database.SqlQueryRaw<ElectronicMagazine>("EXEC SelectElectronicMagazineFilter @id_aut, @subject",
                new Microsoft.Data.SqlClient.SqlParameter("@id_aut", id_aut),
                 new Microsoft.Data.SqlClient.SqlParameter("@subject", subject)).ToList();
        }

        public List<string> GetAllGroups()
        {
            return this.Database.SqlQueryRaw<string>("EXEC SelectAllGroups").ToList();
        }

        public void SetReview(Reviews model, int id)
        {
            SqlParameter dateParameter = new SqlParameter("@date", model.DateTime);
            SqlParameter idParameter = new SqlParameter("@id_aut", id);
            SqlParameter reviewParameter = new SqlParameter("@review", model.Review);
            this.Database.ExecuteSqlRaw("EXEC AddReview @date = @date, @id_aut = @id_aut, @review = @review",
                 dateParameter, idParameter, reviewParameter);
        }

        public void SetNew(NewsAdministration model)
        {
            SqlParameter newParameter = new SqlParameter("@new", model.News_);
            SqlParameter dateParameter = new SqlParameter("@date", model.Date.Date);
            this.Database.ExecuteSqlRaw("EXEC AddNew @new = @new, @date = @date",
                 newParameter, dateParameter);
        }

        public void SetSchedule(ScheduleAdministration model)
        {
            SqlParameter groupParameter = new SqlParameter("@class", model.Group);
            SqlParameter subjectParameter = new SqlParameter("@subject", model.Subject);
            SqlParameter dayParameter = new SqlParameter("@day", model.Day);
            SqlParameter numberParameter = new SqlParameter("@lesson", model.NumberSubject);
            this.Database.ExecuteSqlRaw("EXEC AddSchedule @class = @class, @subject = @subject, @day = @day, @lesson = @lesson",
                 groupParameter, subjectParameter, dayParameter, numberParameter);
        }

        public void UpdateSchedule(int id, string group, string day, int number, string subject)
        {
            SqlParameter idParametr = new SqlParameter("@id", id);
            SqlParameter groupParameter = new SqlParameter("@group", group);
            SqlParameter dayParameter = new SqlParameter("@day", day);
            SqlParameter numberParameter = new SqlParameter("@number", number);
            SqlParameter subjectParameter = new SqlParameter("@subject", subject);
            this.Database.ExecuteSqlRaw("EXEC UpdateSchedule @id = @id, @group = @group, @day = @day, @number = @number, @subject = @subject",
                idParametr, groupParameter, dayParameter, numberParameter, subjectParameter);
        }

        public void DeleteSchedule(int id)
        {
            SqlParameter idParametr = new SqlParameter("@id", id);
            this.Database.ExecuteSqlRaw("EXEC DeleteSchedule @id = @id",
                idParametr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonalAccount>().HasNoKey();
            modelBuilder.Entity<Home>().HasNoKey();
            modelBuilder.Entity<ElectronicMagazine>().HasNoKey();
            modelBuilder.Entity<News>().HasNoKey();
            modelBuilder.Entity<Reviews>().HasNoKey();
            modelBuilder.Entity<Schedule>().HasNoKey();
            modelBuilder.Entity<Teachers>().HasNoKey();
            modelBuilder.Entity<DataAutorization>().HasNoKey();
        }

       
    }
}
