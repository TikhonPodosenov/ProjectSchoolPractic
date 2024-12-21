using System.ComponentModel.DataAnnotations;

namespace ProjectSchool.Models
{
    public class Home
    {
        [Required(ErrorMessage ="Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
    }
}
