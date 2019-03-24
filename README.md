# SunEngine

Движок для сайтов с функциональностью форума, статей, блогов.

<img src="https://github.com/Dmitrij-Polyanin/SunEngine/blob/master/Client/src/statics/SunEngine.svg" width="250" alt="SunEngine Logo" />

Версия: 1.0.0-beta.3

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
- Mobile ready  

#### Сделано с любовью ❤
- Красивый код, который я постоянно улучшаю и совершенствую.
- Если что-то ещё в плане кода не настолько красиво как могло бы быть, принимается обратная связь.

#### Быстрая работа (quick & fast)
- Быстрый доступ к данным на основе linq2db.  
- Приложение грузит только то что нужно, без запроса всех данных при каждом запросе. SPA. 
- Эффективное кэширование

#### Гибкая настройка прав пользователей
Можно настроить для любой группы пользователей для любого раздела сайта (категории) права на доступ на конкретные операции (добавлять материалы, добавлять комментарии, редактировать и тд.)

## Ближайшие планы
- Улучшение серверного кэширования
- Cli для работы с базой и сервисных функций
- Модульная система

## Контакты  
- Дмитрий Полянин  
- Telegram: [@okeanij](https://t.me/Okeanij)    
- Группа проекта в Telegram: [https://t.me/SunEngine](https://t.me/SunEngine) 

## Инсталляция
#### Перед инсталяцией должны быть установлены
- [.Net Core 2.2 SDK](https://dotnet.microsoft.com/download)
- [NodeJs](https://nodejs.org/en/download/)
- [Npm](https://www.npmjs.com)
- [Quasar cli](https://v1.quasar-framework.org/quasar-cli/installation)  `npm install -g @quasar/cli`

#### Установка и запуск для Dev или Demo целей
- Клонировать репозиторий SunEngine с GitHub.
- По умолчанию стоит база SQLite (файл: `SunEngine/SunEngine.db`)
- Скомпилировать солюшен.
- Зайти в дирректорию куда скомпилировался код `/SunEngine/SunEngine/bin/Debug/netcoreapp2.2/` 
- Заполнить базу `dotnet SunEngine.dll migrate init add-test-data`
- Запускаем сервер `dotnet SunEngine.dll server`
- Компилируем и запускаем клиентскую часть.  
  - Зайти в консоль в директорию `SunEngine/Client`
  - `quasar dev` 
  - Откроется браузер с сайтом
- Если что-то не работает написать сюда в Issue, или Telegram группу.

#### Комманды для `dotnet SunEngine.dll`
- `server` - запуск API сервера
- `migrate` - создание таблиц
- `init` - заполнение изначальными данными (пользователи, роли, категории), начальные данные настраиваются в папке `/SunEngine/SunEngine/Config`
- `add-test-data` - заполняет базу данными для тестов (материалы и комментарии)
- `config:<pathToConfigDirectory>` - указываем дирректорию из которой будут браться все настройки, если не указывать берётся `/Config`, можно указывать как полные, так и локальные пути.
  - Пример `config:/local.Config.MySite`

#### Работа с другими базами данных
- База данных: любая совместимая с Linq2db [(список)](https://fluentmigrator.github.io/articles/faq.html) и FluentMigrator [(список)](https://linq2db.github.io/articles/general/databases.html)  
- Протестировано с MySql, Postgres, SqLite 

##### Последовательность подключения
- Если база MySql, Postgres или SqLite
  - Указать ConnectionString и имена провайдеров для FluentMigrator и Linq2db в файле `/SunEngine/SunEngine/DataBaseConnection.json`. Список провайдеров для Linq2db [(список)](https://fluentmigrator.github.io/articles/faq.html) и FluentMigrator [(список)](https://linq2db.github.io/articles/general/databases.html)  
- Если база другая дополнительно:
  - Подключить необходимые NuGet пакеты для работы FluentMigrator и Linq2db с этими базами.
  - В проекте `SunEngine.Migrations` в файле `DbProvider.cs` -> `AddDb` прописать базу.

## Установка и запуск на Production

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
            
            client_max_body_size 11M; 
         }
         
         location /api/ {    # Endpoint для серверной части. Работает как reverse proxy отправляя запросы в Kestrel работающий отдельным процессом.
            proxy_pass  http://localhost:5020/;
            
            client_max_body_size 11M;  # максимальный размер тела запроса, котрый допускает Nginx ~= максимальный размер для upload файла  
         }
         
         client_max_body_size 11M;
    }
```

Теперь необходимо запустить отдельным процессом kestrel сервис по локальному адресу `http://localhost:5020`

Как это делается читаем [статью](https://kimsereyblog.blogspot.com/2018/05/manage-kestrel-process-with-systemd.html).

## Лицензия

Кратко:
- Без оплаты для некоммерческого использования.
- Платно для коммерческого использования.

**[Текст лицензии](docs/licence.md)**