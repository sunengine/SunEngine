# SunEngine

Движок для сайтов с функциональностью форума, статей, блогов.

<img src="https://github.com/Dmitrij-Polyanin/SunEngine/blob/master/Client/src/statics/SunEngine.svg" width="250" alt="SunEngine Logo" />

Версия: 1.1.6

Демо: [demo.sunengine.site](http://demo.sunengine.site)  

#### Технологии:
- Asp.Net Core 2.2
- Linq2db - доступ к базе данных
- FluentMigrator - миграции
- VueJs - SPA клиентская часть
- Quasar Framework - компоненты vue
- База данных - любая совместимая с Linq2db и FluentMigrator  

### О проекте
#### Модули:
- Статьи
- Форум
- Блоги

#### Дружественный интерфейс
- Одностраничное приложение c красивым интерфейсом
- Работает на персональных компьютерах, планшетах, мобильных телефонах

#### Сделано с любовью ❤
- Красивый код, который я постоянно улучшаю и совершенствую.
- Принимаются идеи по улучшению кода и архитектуры.

#### Быстрая работа (quick & fast)
- Быстрый доступ к данным на основе linq2db.  
- Одностраничное приложение грузит только то что нужно, без лишних запросов (SPA).
- Эффективное кэширование

#### Гибкая настройка прав пользователей
Можно настроить для любой группы пользователей для любого раздела сайта (категории) права на доступ на конкретные операции (добавлять материалы, добавлять комментарии, редактировать и тд.)

## Контакты  
- Дмитрий Полянин  
- Telegram: [@okeanij](https://t.me/Okeanij)    
- Группа проекта в Telegram: [https://t.me/SunEngine](https://t.me/SunEngine) 

## Инсталляция
#### Перед инсталляцией должны быть установлены
- [.Net Core 2.2 SDK](https://dotnet.microsoft.com/download)
- [NodeJs](https://nodejs.org/en/download/)
- [Npm](https://www.npmjs.com)
- [Quasar cli](https://v1.quasar-framework.org/quasar-cli/installation)  `npm install -g @quasar/cli`

#### Установка и запуск в режиме Development
- Клонировать репозиторий SunEngine с GitHub.
- Из консоли зайти в папку `SunEngine.Cli`
- Заполняем базу SqLite начальными данными `dotnet run migrate init seed`
- Запускаем сервер `dotnet run server`
- Из консоли зайти в папку `Client`
- Инсталлируем npm модули `npm install`
- Запускаем клиент в dev режиме `quasar dev`
- Откроется браузер с сайтом  


Для работы с другими базами данных базу надо создать вручную и прописать в файл `Config/DataBaseConnection.json` в проекте `SunEgnine.Cli`


#### Команды для `SunEngine.dll`

```
Commands:
    server                      host server api with kestrel
    config:<path>               path to config directory, if none 'Config' is default, '.Config' suffix at the end of the path can be skipped               
    migrate                     make initial database table structure and migrations in existing database
    init                        initialize users, roles and categories tables from config directory
    check-db-con                check is data base connection is working                     
    version                     print SunEngine version
    help                        show this help   
    
Seed test data commands:    
    seed:<CategoryName>:<mCount>:<cCount>      
                                seed category and all subcategories with materials and comments
                                mCount - materials count, default if skipped
                                cCount - comments count, default if skipped
                                example - seed:SomeCategory:20:10
                                
    append-cat-name             add category name to material titles on 'seed'

Examples:
    dotnet SunEngine.dll server
    dotnet SunEngine.dll server config:local.MySite
    dotnet SunEngine.dll migrate init seed
    dotnet SunEngine.dll migrate init seed config:local.MySite
    dotnet SunEngine.dll seed:Forum:10:10
    
```

#### Работа с другими базами данных
- Перед `migrate` базу данных надо создать вручную и прописать в файл `Config/DataBaseConnection.json` в проекте `SunEgnine.Cli`
- База данных: любая совместимая с Linq2db [(список)](https://fluentmigrator.github.io/articles/faq.html) и FluentMigrator [(список)](https://linq2db.github.io/articles/general/databases.html)  
- Протестировано с MySql, Postgres, SqLite 

##### Последовательность подключения
- Если база MySql, Postgres или SqLite
  - Указать ConnectionString и имена провайдеров для FluentMigrator и Linq2db в файле `/SunEngine/SunEngine/DataBaseConnection.json`. Список провайдеров для Linq2db [(список)](https://fluentmigrator.github.io/articles/faq.html) и FluentMigrator [(список)](https://linq2db.github.io/articles/general/databases.html)
  - Если база MySql, при создании, необходимо установить кодировку базы `utf8mb4` 
- Если база другая дополнительно:
  - Подключить необходимые NuGet пакеты для работы FluentMigrator и Linq2db с этими базами.
  - В проекте `SunEngine.Migrations` в файле `DbProvider.cs` -> `AddDb` прописать базу.

## Установка и запуск на Production

- Собрать проект через `build.sh` (Параметры сборки настраиваются переменными внутри скрипта `build.sh`)
- Появится директория `/SunEngine/Build`
- Записать на сервер вручную или через `publish.sh` (Настройки подключения к серверу внутри файла скрипта)

- При публикации через `build.sh` файлы конфигурации не публикуются
- Для работы сборки на production необходимо один раз вручную переписать файлы конфигурации:
  - Папка `SunEngine.Cli/Config` - записать в корневую директорию сайта на сервере
  - Файл `/Client/config.js` записать в директорию `wwwroot` в папке сайта не сервере. 


#### Вариант запуска на Nginx на Ubuntu. 

Серверная и клиентская части запускаются на разных endpoint-ах, например клиентская часть `mysite.site`, серверная часть `mysite.site/api`.

```
server {
         listen 80;
         server_name mysite.site;
         

         add_header X-Frame-Options "SAMEORIGIN";
         add_header X-XSS-Protection "1; mode=block";
         add_header X-Content-Type-Options "nosniff";

         charset utf-8;
         
         location / {    # Endpoint для клиентской части
            root /site/mysite/wwwroot;
            try_files $uri $uri/ /index.html;   # если файл не найден - возвращаем index.html
         }
         
         location /api/ {    # Endpoint для серверной части. Работает как reverse proxy отправляя запросы в Kestrel работающий отдельным процессом.
            proxy_pass  http://localhost:5020/;
            
            client_max_body_size 11M;  # максимальный размер тела запроса, котрый допускает Nginx ~= максимальный размер для upload файла  
         }
    }
```

Теперь необходимо запустить отдельным процессом kestrel сервис по локальному адресу `http://localhost:5020`

Как это делается читаем [статью](https://kimsereyblog.blogspot.com/2018/05/manage-kestrel-process-with-systemd.html).