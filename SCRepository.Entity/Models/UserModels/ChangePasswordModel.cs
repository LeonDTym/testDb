using System.ComponentModel.DataAnnotations;

namespace SCRepository.Entity.Models.UserModels
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Минимальная длина пароля - 8 символов. Максимальная - 15 символов.")]
        [RegularExpression(@"(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{8,}", ErrorMessage = "Пароль должен состоять из цифр, строчных и заглавных букв латинского алфавита и специальных символов из набора @ # $ %.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Минимальная длина пароля - 8 символов. Максимальная - 15 символов.")]
        [RegularExpression(@"(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{8,}", ErrorMessage = "Пароль должен состоять из цифр, строчных и заглавных букв латинского алфавита и специальных символов из набора @ # $ %.")]
        public string PasswordRepeat { get; set; }
    }
}
