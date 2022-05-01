using System.ComponentModel.DataAnnotations;

namespace UserAPI.Dtos
{
    public class UpdateUserInfoDto
    {
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Недопустимые символы")]
        public string Name { get; set; } //Имя (запрещены все символы кроме латинских и русских букв)
        [Range(0, 2)]
        public int Gender { get; set; } //Пол 0 - женщина, 1 - мужчина, 2 - неизвестно
        public DateTime? Birthday { get; init; } //Поле даты рождения может быть Null
    }
}
