using System.ComponentModel.DataAnnotations;

namespace SCRepository.Entity.Models.UserModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Минимальная длина пароля - 8 символов. Максимальная - 15 символов.")]
        //[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,15}$", ErrorMessage = "Пароль должен состоять из цифр, прописных и строчных букв латинского алфавита")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите личный идентификатор")]
        public string UserNameProfile { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Минимальная длина пароля - 8 символов. Максимальная - 15 символов.")]
        //[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,15}$", ErrorMessage = "Пароль должен состоять из цифр, прописных и строчных букв латинского алфавита")]
        public string PasswordProfile { get; set; }
    }
}
