# SunEngine

Движок для сайтов с функциональностью форума, статей и блогов.

<img src="https://github.com/Dmitrij-Polyanin/SunEngine/blob/master/SunEngine.svg" width="250" alt="SunEngine Logo" />

<img src="https://img.shields.io/static/v1?label=Version&message=2.0.0-rc.9&color=green">   <a href="https://demo.sunengine.site"><img src="https://img.shields.io/static/v1?label=Demo&message=demo.sunengine.site&color=informational"></a>
   <a href="https://t.me/SunEngine"><img src="https://img.shields.io/static/v1?label=Telegram&message=@SunEngine&color=informational"></a>     <a href="README.RU.md"><img src="https://img.shields.io/static/v1?label=English&message=readme&color=informational"></a>


### О проекте
#### Core модули:
- Статьи
- Форум
- Блоги

#### Дружественный интерфейс
- Одностраничное приложение c красивым интерфейсом.
- Работает на персональных компьютерах, планшетах и мобильных телефонах.

#### Сделано с любовью ❤
- Красивый код, который я постоянно улучшаю и совершенствую.
- Принимаются идеи по улучшению кода и архитектуры.

#### Ключевые технологии:
В проекте используются современные и красивые технологии.

- Asp.Net Core 3.1
- VueJs — SPA клиентская часть.
- Quasar Framework — компоненты vue.
- Linq2db — доступ к базе данных.
- База данных — любая совместимая с Linq2db и FluentMigrator.

#### Быстрая работа (quick & fast)
- Быстрый доступ к данным на основе linq2db.
- Одностраничное приложение грузит только то, что нужно, без лишних запросов (SPA).
- Эффективное кэширование.

#### Гибкая настройка прав ролей пользователей
- Возможность для каждого раздела — категории сайта для разных групп пользователей задавать разные права на доступ.

#### Админка
- Редактирование разделов сайта — категорий.
- Редактирование меню сайта.
- Редактирование ролей пользователей.
- Настройки кэширования.

#### Развёртка
- Работает на Windows, Linux и macOS.
- Совместимо с большинством реляционных баз данных.

#### Безопасность
Разрабатывается с учётом последних технологий приватности и безопасности.

- Система JWT токенов авторизации, хранящихся и в localStorage, и в cookies, обеспечивают защиту от перехвата токенов, а также от многих видов XSS и CSRF атак. *
- Санитаризация всех сообщений на стороне сервера от вредоносных скриптов.
- Работает по защищённому HTTPs соединению.
- Защита сайта от flood запросов с помощью:
  - Фильтров против повторяющихся запросов публикации.
  - Captcha на критических участках. *

* Системы безопасности требуют всесторонней проверки и тестирования.

### Требования для запуска
SunEngine можно запустить на Windows, Linux и macOS.

Для запуска проекта необходимо установить:
- [DotNet Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [NodeJs](https://nodejs.org/en/download/)
- [Npm](https://www.npmjs.com/) (устанавливается сам при установке NodeJs)
- [Quasar CLI](https://quasar.dev/quasar-cli/installation) `npm install -g @quasar/cli`

### Запуск для разработки
#### Запуск из консоли
1. Скачать код проекта с официального репозитория https://github.com/sunengine/SunEngine (если не сделано)
2. Переименовть файлы и папки с суффиксом `-template`, удалив этот суффикс. (если не сделано)
3. Зайти в папку `SunEngine/SunEngine.Cli`.
4. Создать базу данных с названим SunEngine в выбранной СУБД. (если не сделано)
5. В файле `SunEngine/SunEngine.Cli/Config/DataBaseConnection.json` прописать имя провайдера и строку подключения.
6. Заполнить базу данных начальными данными `dotnet run migrate init seed` (если не сделано).
7. Запустить сервер `dotnet run server`.
8. Зайти в папку `SunEngine/Client`.
9. Установить npm модули `npm install` (если не сделано).
10. Запустить клиент `quasar dev` — откроется браузер с сайтом.

### Контакты
- [Димитрий Полянин](https://sunengine.site/user/okeanij)
- Telegram: [@okeanij](tg://resolve?domain=Okeanij)
- Группа в Telegram: [@SunEngine](tg://resolve?domain=SunEngine)
