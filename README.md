# SunEngine

Site engine that supports forums, articles and blogs.

<img src="https://github.com/Dmitrij-Polyanin/SunEngine/blob/master/Client/src/statics/SunEngine.svg" width="250" alt="SunEngine Logo" />

Version: 1.10.4

Demo site: [demo.sunengine.site](http://demo.sunengine.site). 

Статья на русском: [ссылка](README.ru.md).

### About project
#### Core modules
- Articles
- Forum
- Blogs

#### Friendly interface
- Single-page application with a beautiful interface.
- Works on personal computers, tablets and mobile phones.

#### Made with love ❤
- Beautiful code that we constantly improve.
- We are open to new ideas for improving code and architecture.

#### Key technologies
The project uses modern and beautiful technologies.

- Asp.Net Core 2.2
- Linq2db — database framework.
- FluentMigrator — database migrations.
- VueJs — SPA-based client side.
- Quasar Framework — vue components.
- Database — any compatible with Linq2db and FluentMigrator.

#### Performance
- Fast data access based on linq2db.
- Single-page application loads only necessary data, without extra requests.
- Efficient caching.

#### Flexible configuration of user role rights
- Opportunity for each section — site's category — set different access rights for different groups of users.

#### Admin panel
- Edit site sections — categories.
- Edit site menu.
- Edit user roles.
- Customize caching.

#### Deployment
- Works on Windows, Linux and macOS.
- Compatible with most relational databases.

### Launch prerequisites
SunEngine can be launched on Windows, Linux and macOS.
  
To run the project you need to install:
- [.NET Core 2.2 + ASP.NET Core Runtimes](https://dotnet.microsoft.com/download/dotnet-core/2.2)
- [NodeJs](https://nodejs.org/en/download/)
- [Npm](https://www.npmjs.com/) (will already be installed along with NodeJs)
- [Quasar CLI](https://quasar.dev/quasar-cli/installation) `npm install -g @quasar/cli`

### Launch for development
#### Launch from console
1. Go to folder `SunEngine/SunEngine.Cli`.
2. Fill SQLite database with initial data `dotnet run migrate init seed` (if not done yet).
3. Run server `dotnet run server`.
4. Go to folder `SunEngine/Client`.
5. Install npm modules `npm install` (if not done yet).
6. Run client `quasar dev` — browser will be opened.

### Contacts
- Dmitrij Polianin
- Skype: dmitrij.polyanin
- Telegram: [@okeanij](https://t.me/Okeanij)
- Project's Telegram group: [@SunEngine](https://t.me/SunEngine)
