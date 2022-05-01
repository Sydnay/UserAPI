using System.ComponentModel.DataAnnotations;

namespace UserAPI.Dtos
{
    public class UpdateUserPasswordDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Недопустимые символы")]
        public string Password { get; set; } //Пароль (запрещены все символы кроме латинских букв и цифр)
    }
}
