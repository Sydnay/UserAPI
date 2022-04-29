using MongoDB.Bson.Serialization.Attributes;
namespace Users
{
    public record User
    {
        [BsonId]
        public Guid Guid { get; init; } //Уникальный идентификатор пользователя
        public string Login { get; set; } //Уникальный Логин (запрещены все символы кроме латинских букв и цифр)
        public string Password { get; set; } //Пароль (запрещены все символы кроме латинских букв и цифр)
        public string Name { get; set; } //Имя (запрещены все символы кроме латинских и русских букв)
        public int Gender { get; set; } //Пол 0 - женщина, 1 - мужчина, 2 - неизвестно
        public DateTime? Birthday { get; init; } //Поле даты рождения может быть Null
        public bool Admin { get; set; } //Указание - является ли пользователь админом
        public DateTime CreatedOn { get; init; } //Дата создания пользователя
        public string CreatedBy { get; init; } //Логин Пользователя, от имени которого этот пользователь создан
        public DateTime ModifiedOn { get; set; } //Дата изменения пользователя
        public string ModifiedBy { get; set; } //Логин Пользователя, от имени которого этот пользователь изменён
        [BsonIgnore]
        public DateTime RevokedOn { get; set; } //Дата удаления пользователя
        [BsonIgnore]
        public string RevokedBy { get; set; } //Логин Пользователя, от имени которого этот пользователь удалён

    }
}