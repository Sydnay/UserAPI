using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Dtos
{
    public class UserDto
    {
        [Key]
        public Guid Guid { get; init; } //Уникальный идентификатор пользователя
        public string Login { get; init; } //Уникальный Логин (запрещены все символы кроме латинских букв и цифр)
        public string Password { get; init; } //Пароль (запрещены все символы кроме латинских букв и цифр)
        public string Name { get; init; } //Имя (запрещены все символы кроме латинских и русских букв)
        [Range(0, 2)]
        public int Gender { get; init; } //Пол 0 - женщина, 1 - мужчина, 2 - неизвестно
        public DateTime? Birthday { get; init; } //Поле даты рождения может быть Null
        public bool Admin { get; init; } //Указание - является ли пользователь админом
        public DateTime CreatedOn { get; init; } //Дата создания пользователя
        public string CreatedBy { get; init; } //Логин Пользователя, от имени которого этот пользователь создан
        public DateTime ModifiedOn { get; init; } //Дата изменения пользователя
        public string ModifiedBy { get; init; } //Логин Пользователя, от имени которого этот пользователь изменён
        public DateTime RevokedOn { get; init; } //Дата удаления пользователя
        public string? RevokedBy { get; init; } //Логин Пользователя, от имени которого этот пользователь удалён

    }
}
