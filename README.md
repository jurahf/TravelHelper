# TravelHelper
Проект разработан в рамках хакатона NaviHack.

### Используются технологии:
.NET Framework 4.5
Сервер - ASP.Net WebAPI, Entity Framework 6.2
Клиент - ASP.Net MVC, bootstrap 3.3.7

### Адреса ресурсов
Расположены в файлах конфигурации приложений:
\TravelHelper\Client\Web.config           - параметр serverAddress
\TravelHelper\Server\Web.config           - параметры origins, NaviApiAddress

Также в файле js содержатся адреса для AJAX-запросов:
\TravelHelper\Client\Scripts\Custom\ServerApiConfig.js

### База данных
Для работы сервера необходимо подключение к базе данных.
Строка подключения содержится в файле конфигурации \TravelHelper\Server\Web.config в разделе ConnectionStrings.
По-умолчанию используется MS SQL.

Для первоначального запуска приложения понадобится создать базу данных. 
Необходимые скрипты находятся в папке \TravelHelper\Server\Models\db.


####
Так как приложение используется в рамках хакатона, система авторизации отключена. 
Все действия, где требуются данные пользователя происходят под логином TestUser (он должен быть создан в базе данных).


