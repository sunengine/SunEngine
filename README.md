![SunEngine Logo](https://github.com/Dmitrij-Polyanin/SunEngine/blob/master/Client/src/statics/SunEngine.svg =200)

# SunEngine

Движок сайта - форум, статьи, блог.

Текущая версия: 0.4.6

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

#### Гибкая настройка прав пользователей.
Можно настроить для любой группы пользователей для любого раздела сайта (категории) права на доступ на конкретные операции (Добавлять тексты, Добавлять сообщения, Редактировать и тд.)

## Ближайшие планы
- Модульная система
- Рефакторинг кода
- Админка
- Улучшение кэширования

## Проект находится в стадии разработки
Желающие 
- Потестировать
- Предложить идеи по коду  

Пишите на Telegram: @okeanij  
Или сюда на github.

## Инсталляция
#### Перед инсталяцией должны быть установлены
- [.Net Core 2.2 SDK](https://dotnet.microsoft.com/download)
- [NodeJs](https://nodejs.org/en/download/)
- [Npm](https://www.npmjs.com)
- [Quasar-CLI](https://quasar-framework.org/guide/quasar-cli.html)  
`npm install -g quasar-cli`

#### Установка и запуск SunEngine
- Клонировать репозиторий SunEngine с GitHub.
- Создать на MySql пустую базу c названием `<DataBaseName>`.
- Переименовать `SunEngine/DataBaseConnectionExample.json` в `SunEngine/DataBaseConnection.json`.
- Указать название базы и `ConnectionString` в файле `SunEngine/DataBaseConnection.json`.
- Запустить проект Migrations. Произойдёт создание таблиц.
- Запустить проект DataSeedDev. Заполнение таблиц тестовыми данными.
- Компилируем и запускаем серверную часть Asp.Net Core.  
  - Компилируем и запускаем проект SunEngine.
- Компилируем и запускаем клиентскую часть.  
  - Зайти в коность в директорию `SunEngine/Client`
  - `quasar dev` 
  - Откроется браузер с сайтом
- Если что-то не работает написать мне.

#### Использование других баз данных

 - Подключиь NuGet пакет поддержки базы данных для [FluentMigrator](https://fluentmigrator.github.io) в проекте `Migrations`.
 - Прописать поддержку в файле `Migrations/Main.cs`.  
   Строку `.AddMySql5()` заменяем на нужное.
 - На данный момент тестировалось только с MySql.
  
##### Пример с подключением MS SqlServer
 - Устанавливаем NuGet пакет [
FluentMigrator.Runner.SqlServer](https://www.nuget.org/packages/FluentMigrator.Runner.SqlServer/)
в проект `Migrations`.
 - Заменяем в `Migrations/Main.cs`  `.AddMySql5()` -> `.AddSqlServer()`.

#### Лицензия
- В процессе разработки
- Все права принадлежат создателю Дмитрий Полянин
- Free для некомерческого использования. Платно для коммерческого.