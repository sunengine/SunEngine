﻿<p align="center">
<img src="https://github.com/sunengine/SunEngine/blob/master/SunEngine.svg" width="300" alt="SunEngine Logo" />
</p>

<h3 align="center">
Движок для сайтов с возможностями блога, статей и форума.<br/>
Построен на технологиях: AspNet Core, VueJs, Quasar.</h3>


# SunEngine

<a href="#"><img src="https://img.shields.io/static/v1?label=%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F&message=v2.13.4&color=green"></a>
<a href="#"><img src="https://github.com/sunengine/SunEngine/actions/workflows/dotnetcore.yml/badge.svg" ></a>
<a href="#"><img src="https://github.com/sunengine/SunEngine/actions/workflows/quasar.yml/badge.svg" ></a>
<br/>
<a href="https://sunengine.github.io/src/ru"><img src="https://img.shields.io/static/v1?label=%D0%94%D0%BE%D0%BA%D1%83%D0%BC%D0%B5%D0%BD%D1%82%D0%B0%D1%86%D0%B8%D1%8F&message=sunengine.github.io&color=informational"></a>
<a href="https://sunengine.site"><img src="https://img.shields.io/static/v1?label=%D0%A1%D0%B0%D0%B9%D1%82&message=sunengine.site&color=yellow"></a> 
<a href="https://demo.sunengine.site"><img src="https://img.shields.io/static/v1?label=%D0%94%D0%B5%D0%BC%D0%BE&message=demo.sunengine.site&color=yellow"></a>
<a href="https://t.me/SunEngine"><img src="https://img.shields.io/static/v1?label=Telegram&message=@SunEngine&color=success"></a>
<a href="https://t.me/developer_school"><img src="https://img.shields.io/static/v1?label=Telegram&message=%D0%A8%D0%BA%D0%BE%D0%BB%D0%B0%20%D1%80%D0%B0%D0%B7%D1%80%D0%B0%D0%B1%D0%BE%D1%82%D1%87%D0%B8%D0%BA%D0%B0&color=success"></a>
<a href="README.md"><img src="https://img.shields.io/static/v1?label=Readme&message=English&color=informational"></a>



### Основные возможности
 - Возможность вести блог  
 - Создавать форум и его разделы  
 - Создавать разделы статей и писать статьи  
 - Возможность комментирования любых материалов на сайте  
 - Создание профиля пользователя на сайте  
 - Личный кабинет пользователя с возможностью редактирования информации о пользователе и персональных данных  
 - Администрирование портала через панель администратора  
 - Возможность создания и изменения тем оформления сайта, выбор из [готовых тем](https://github.com/sunengine/Skins).  

### Продуманный и дружественный интерфейс
- Одностраничное приложение c современным и красивым интерфейсом.  
- Работает на персональных компьютерах, планшетах, мобильных телефонах.  
- В перспективе возможна сборка как нативное приложение Android, iOS, Windows и Linux.  

### Быстрая работа
 - Одностраничное приложение грузит только то что нужно, без лишних запросов (SPA).  
 - Быстрый доступ к данным на основе linq2db.  
 - Эффективное и настраиваемое кэширование.  

### Гибкая настройка ролей
 - Возможность гибкой настройки прав групп пользователей для каждого раздела сайта.  

### Функциональная админка
 - С возможностями создания  
   - Блогов  
   - Форумов  
   - Разделов статей  
   - Создание подразделов  
 - Интерактивное редактирование меню сайта, а так же дополнительных меню  
 - Выбора основных и дополнительных тем оформления  
 - Группы пользователей и изменение их прав  
 - Создание компонентов ленты событий сайта  
 - И другие возможности...  

### Технологии
В проекте используются современные технологии:  
 - Asp.Net Core 3.1  
 - VueJs - SPA клиентская часть  
 - Quasar Framework - Material Design VueJs компоненты  
 - PostgreSQL - база данных (в перспективе поддержка других SQL субд)  
 - Linq2db ORM - доступ к базе данных  
 
### Безопасность
 - Разрабатывается с учётом последних технологий приватности и безопасности.  
 - Новаторская система авторизации на основе трёх токенов для защиты от перехвата данных авторизации, а так же атак XSS и CSRF.  
 - AES шифрование для защиты токенов авторизации.  
 - Очистка сообщений на сервере от вредоносных скриптов.  
 - HTTPS безопасный протокол.  
 - Защита от flood запросов  
  - Фильтры против повторяющихся запросов публикации.  
  - Captcha на критических участках.  
 - Защитные механизмы от многих вредоносных типов атак.  

### Хостинг
 - Linux или Windows сервер  
 - Bash скрипты для сборки, установки и обновления  
 - [Руководство по установке на Ubuntu 18.04 сервер](https://sunengine.github.io/src/ru/manual-install_ru/step_by_step_server_installation_ru.html).    

### Дополнительно
 - Сделано с любовью ❤  
 - Качественный код и архитектура.  
 - [План развития проекта](https://sunengine.site/texts/roadmap). 

 ### Ссылки
- Документация - https://sunengine.github.io/src/ru
- Сайт - https://sunengine.site
- Демо сайт - https://demo.sunengine.site 

### Контакты
 - Дмитрий Полянин Telegram: [@Okeanij](https://t.me/Okeanij)  
 - Telegram группа SunEngine: [@SunEngine](https://t.me/SunEngine)  

### Школа разработчика Дмитрия Полянина
 - Сайт: https://okeanij.ru
 - Telegram: [@developer_school](https://t.me/developer_school)
 - Instagram: [@developer_school](https://instagram.com/developer_school)
