using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Недопустимые символы")]
        public string Login { get; set; } //Уникальный Логин (запрещены все символы кроме латинских букв и цифр)
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Недопустимые символы")]
        public string Password { get; set; } //Пароль (запрещены все символы кроме латинских букв и цифр)
        [Required]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Недопустимые символы")]
        public string Name { get; set; } //Имя (запрещены все символы кроме латинских и русских букв)
        [Required]
        [Range(0,2)]
        public int Gender { get; set; } //Пол 0 - женщина, 1 - мужчина, 2 - неизвестно
        [Required]
        public DateTime? Birthday { get; init; } //Поле даты рождения может быть Null
        [Required]
        public bool Admin { get; set; } //Указание - является ли пользователь админом
        
    }
}
