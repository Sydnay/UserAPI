using MongoDB.Bson.Serialization.Attributes;

namespace UserAPI.Dtos
{
    public class UserInfoDto
    {
        public string Name { get; set; } //Имя (запрещены все символы кроме латинских и русских букв)
        public int Gender { get; set; } //Пол 0 - женщина, 1 - мужчина, 2 - неизвестно
        public DateTime? Birthday { get; init; } //Поле даты рождения может быть Null
        public bool Active { get; set; }//Статус активный или нет
    }
}
