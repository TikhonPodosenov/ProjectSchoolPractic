using Microsoft.EntityFrameworkCore;
using ProjectSchool.Models;
using Moq;
using ProjectSchool.DB;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using ProjectSchool.Controllers;
using ProjectSchool;
using ProjectSchool.Tests;

namespace TestProject2
{
    public class ApplicationDbContextTests
    {
        [Fact]
        public void GetPersonalAccount_ValidId_ReturnsPersonalAccount()
        {
            var sqlQueryExecutorMock = new Mock<ISqlQueryExecutor>();
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDb").Options;
            var dbContext = new ApplicationDbContext(dbContextOptions, sqlQueryExecutorMock.Object);
            int id_aut = 45;

            var expectedPersonalAccount = new List<PersonalAccount>
             {
                 new PersonalAccount { Name = "Алексей", Surname = "Смирнов", Patronymic = "Викторович", Group = "5В", Number = "89123456789" }
             };

            sqlQueryExecutorMock.Setup(executor => executor.ExecuteQuery<PersonalAccount>(
             "EXEC SelectStudent @id_aut",
             It.Is<object[]>(parameters => parameters != null &&
                                            parameters.Length == 1 &&
                                            parameters[0] is SqlParameter &&
                                           ((SqlParameter)parameters[0]).ParameterName == "@id_aut" &&
                                           (int)((SqlParameter)parameters[0]).Value == id_aut)))
             .Returns(expectedPersonalAccount);

            // Act
            var result = dbContext.GetPersonalAccount(id_aut);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Алексей", result[0].Name);
            Assert.Equal("Смирнов", result[0].Surname);
            Assert.Equal("Викторович", result[0].Patronymic);
            Assert.Equal("5В", result[0].Group);
            Assert.Equal("89123456789", result[0].Number);
        }

        [Fact]
        public void VerificationOfAuthorization_ValidCredentials_ReturnsDataAutorization()
        {
            // Arrange
            var sqlQueryExecutorMock = new Mock<ISqlQueryExecutor>();
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDb").Options;
            var dbContext = new ApplicationDbContext(dbContextOptions, sqlQueryExecutorMock.Object);
            string login = "test_login";
            string password = "test_password";

            var expectedDataAutorization = new List<DataAutorization>
            {
                new DataAutorization { Id_aut = 1, Post = "Test Post" }
            };

            sqlQueryExecutorMock.Setup(executor => executor.ExecuteQuery<DataAutorization>(
               "EXEC VerificationOfAuthorization @login, @password",
              It.Is<object[]>(parameters => parameters != null &&
                                              parameters.Length == 2 &&
                                              parameters[0] is SqlParameter &&
                                             ((SqlParameter)parameters[0]).ParameterName == "@login" &&
                                             (string)((SqlParameter)parameters[0]).Value == login &&
                                              parameters[1] is SqlParameter &&
                                             ((SqlParameter)parameters[1]).ParameterName == "@password" &&
                                               (string)((SqlParameter)parameters[1]).Value == password)))
               .Returns(expectedDataAutorization);

            // Act
            var result = dbContext.VerificationOfAuthorization(login, password);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].Id_aut);
            Assert.Equal("Test Post", result[0].Post);
        }

        [Fact]
        public async Task CheckPersonData_InvalidInput_ReturnsViewWithValidationErrors()
        {
            // Arrange
            var sqlQueryExecutorMock = new Mock<ISqlQueryExecutor>();
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDb").Options;
            var dbContext = new ApplicationDbContext(dbContextOptions, sqlQueryExecutorMock.Object);
            var controller = new HomeController(dbContext, new GlobalDataService());
            controller.ModelState.AddModelError("Login", "Введите логин");
            controller.ModelState.AddModelError("Password", "Введите пароль");
            var model = new Home { Login = null, Password = null };
            // Act
            var result = await controller.CheckPersonData(model) as ViewResult;


            // Assert
            Assert.NotNull(result);
            Assert.False(result.ViewData.ModelState.IsValid);
            Assert.True(result.ViewData.ModelState.ContainsKey("Login"));
            Assert.True(result.ViewData.ModelState.ContainsKey("Password"));
            Assert.Equal("Введите логин", result.ViewData.ModelState["Login"].Errors[0].ErrorMessage);
            Assert.Equal("Введите пароль", result.ViewData.ModelState["Password"].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task CheckPersonData_InvalidCredentials_ReturnsViewWithAuthorizationError()
        {
            // Arrange
            var sqlQueryExecutorMock = new Mock<ISqlQueryExecutor>();
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDb").Options;
            var dbContext = new ApplicationDbContext(dbContextOptions, sqlQueryExecutorMock.Object);
            var controller = new HomeController(dbContext, new GlobalDataService());

            string login = "wrong_login";
            string password = "wrong_password";
            var model = new Home { Login = login, Password = password };

            sqlQueryExecutorMock.Setup(executor => executor.ExecuteQuery<DataAutorization>(
                 "EXEC VerificationOfAuthorization @login, @password",
                 It.Is<object[]>(parameters => parameters != null &&
                                                parameters.Length == 2 &&
                                                parameters[0] is SqlParameter &&
                                               ((SqlParameter)parameters[0]).ParameterName == "@login" &&
                                               (string)((SqlParameter)parameters[0]).Value == login &&
                                                parameters[1] is SqlParameter &&
                                               ((SqlParameter)parameters[1]).ParameterName == "@password" &&
                                                 (string)((SqlParameter)parameters[1]).Value == HomeController.HashPassword(password))))
                .Returns(new List<DataAutorization>());


            // Act
            var result = await controller.CheckPersonData(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(result.ViewData.ModelState.IsValid);
            Assert.True(result.ViewData.ModelState.ContainsKey(string.Empty));
            Assert.Equal("Неправильные данные", result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task CheckPersonData_ValidCredentials_RedirectsToPersonalAccount()
        {
            // Arrange
            var sqlQueryExecutorMock = new Mock<ISqlQueryExecutor>();
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDb").Options;
            var dbContext = new ApplicationDbContext(dbContextOptions, sqlQueryExecutorMock.Object);
            var globalDataService = new GlobalDataService();
            var controller = new HomeController(dbContext, globalDataService);
            string login = "test_login";
            string password = "test_password";
            var model = new Home { Login = login, Password = password };

            var expectedDataAutorization = new List<DataAutorization>
            {
                new DataAutorization { Id_aut = 1, Post = "Ученик" }
            };


            sqlQueryExecutorMock.Setup(executor => executor.ExecuteQuery<DataAutorization>(
            "EXEC VerificationOfAuthorization @login, @password",
              It.Is<object[]>(parameters => parameters != null &&
                                           parameters.Length == 2 &&
                                           parameters[0] is SqlParameter &&
                                          ((SqlParameter)parameters[0]).ParameterName == "@login" &&
                                          (string)((SqlParameter)parameters[0]).Value == login &&
                                           parameters[1] is SqlParameter &&
                                          ((SqlParameter)parameters[1]).ParameterName == "@password" &&
                                            (string)((SqlParameter)parameters[1]).Value == HomeController.HashPassword(password))))
              .Returns(expectedDataAutorization);

            // Act
            var result = await controller.CheckPersonData(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("PersonalAccount", result.ControllerName);
            Assert.Equal(1, globalDataService.Id_aut);
            Assert.Equal("Ученик", globalDataService.Post);
        }

        [Fact]
        public void GetScheduleFilter_ValidFilters_ReturnsScheduleList()
        {
            // Arrange
            var sqlQueryExecutorMock = new Mock<ISqlQueryExecutor>();
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDb").Options;
            var dbContext = new ApplicationDbContext(dbContextOptions, sqlQueryExecutorMock.Object);
            string subject = "Алгебра";
            string group = "10A";
            string day = "Понедельник";

            var expectedSchedule = new List<Schedule>
            {
                new Schedule { Id = 1, Group = group, Day = day, Subject = subject, NumberSubject = 1 }
            };

            sqlQueryExecutorMock.Setup(executor => executor.ExecuteQuery<Schedule>(
                 "EXEC SelectScheduleFilter @subject, @group, @day",
                   It.Is<object[]>(parameters => parameters != null &&
                                                 parameters.Length == 3 &&
                                                   parameters[0] is SqlParameter &&
                                                  ((SqlParameter)parameters[0]).ParameterName == "@subject" &&
                                                  (string)((SqlParameter)parameters[0]).Value == subject &&
                                                  parameters[1] is SqlParameter &&
                                                   ((SqlParameter)parameters[1]).ParameterName == "@group" &&
                                                  (string)((SqlParameter)parameters[1]).Value == group &&
                                                   parameters[2] is SqlParameter &&
                                                  ((SqlParameter)parameters[2]).ParameterName == "@day" &&
                                                  (string)((SqlParameter)parameters[2]).Value == day)))
                    .Returns(expectedSchedule);

            // Act
            var result = dbContext.GetScheduleFilter(subject, group, day);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(group, result[0].Group);
            Assert.Equal(day, result[0].Day);
            Assert.Equal(subject, result[0].Subject);
            Assert.Equal(1, result[0].NumberSubject);
        }

        [Fact]
        public void SetNew_ValidNews_ExecutesSqlQueryRaw()
        {
            // Arrange
            var sqlQueryExecutorMock = new Mock<ISqlQueryExecutor>();
            var databaseMock = new Mock<DatabaseFacade>(new Mock<DbContext>().Object);

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDb").Options;
            var dbContext = new TestApplicationDbContext(dbContextOptions, sqlQueryExecutorMock.Object)
            {
                Database = databaseMock.Object
            };
            var model = new NewsAdministration { News_ = "Test News", Date = DateTime.Now };

            databaseMock.Setup(db => db.ExecuteSqlRaw(
                   "EXEC AddNew @new = @new, @date = @date",
                    It.Is<object[]>(parameters => parameters != null &&
                                                parameters.Length == 2 &&
                                                parameters[0] is SqlParameter &&
                                                 ((SqlParameter)parameters[0]).ParameterName == "@new" &&
                                                 (string)((SqlParameter)parameters[0]).Value == model.News_ &&
                                                 parameters[1] is SqlParameter &&
                                                  ((SqlParameter)parameters[1]).ParameterName == "@date" &&
                                                   ((DateTime)((SqlParameter)parameters[1]).Value).Date == model.Date.Date)));

            // Act
            dbContext.SetNew(model);

            // Assert
            databaseMock.Verify(db => db.ExecuteSqlRaw(
                   "EXEC AddNew @new = @new, @date = @date",
                     It.Is<object[]>(parameters => parameters != null &&
                                                parameters.Length == 2 &&
                                                 parameters[0] is SqlParameter &&
                                                  ((SqlParameter)parameters[0]).ParameterName == "@new" &&
                                                 (string)((SqlParameter)parameters[0]).Value == model.News_ &&
                                                 parameters[1] is SqlParameter &&
                                                 ((SqlParameter)parameters[1]).ParameterName == "@date" &&
                                                  ((DateTime)((SqlParameter)parameters[1]).Value).Date == model.Date.Date)), Times.Once);
        }

        [Fact]
        public void HashPassword_ValidPassword_ReturnsHashedString()
        {
            // Arrange
            string password = "test_password";

            // Act
            string hashedPassword = HomeController.HashPassword(password);

            // Assert
            Assert.NotNull(hashedPassword);
            Assert.NotEmpty(hashedPassword);

            //Проверка на то что метод работает, и что строка не пустая.
        }

        [Fact]
        public void HashPassword_SamePasswords_ReturnsSameHash()
        {
            // Arrange
            string password = "test_password";
            string password2 = "test_password";
            // Act
            string hashedPassword1 = HomeController.HashPassword(password);
            string hashedPassword2 = HomeController.HashPassword(password2);


            // Assert
            Assert.Equal(hashedPassword1, hashedPassword2);
        }

        [Fact]
        public void HashPassword_DifferentPasswords_ReturnsDifferentHash()
        {
            // Arrange
            string password = "test_password";
            string password2 = "test_password2";
            // Act
            string hashedPassword1 = HomeController.HashPassword(password);
            string hashedPassword2 = HomeController.HashPassword(password2);


            // Assert
            Assert.NotEqual(hashedPassword1, hashedPassword2);
        }
    }
}