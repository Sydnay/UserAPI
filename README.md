# UserAPI
Web API сервис, реализующий API методы CRUD над сущностью Users, доступ к API осуществляется через интерфейс Swagger.
Все действия происходят в оперативной памяти и не сохраняются при остановке приложения (по желанию можно подключить базу данных).
Предварительно создается Admin, от имени которого вначале происходят действия.

!!!Во всех запросах присутствовуют параметры логин и пароль выполняющего запрос!!!
