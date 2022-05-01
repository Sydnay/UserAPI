using System.ComponentModel.DataAnnotations;

namespace UserAPI.Dtos
{
    public class UpdateUserLoginDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Недопустимые символы")]
        public string Login { get; set; } //Уникальный Логин (запрещены все символы кроме латинских букв и цифр)
    }
}
