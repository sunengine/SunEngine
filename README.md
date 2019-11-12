# SunEngine

Site engine that supports forums, articles and blogs.

<img src="https://github.com/Dmitrij-Polyanin/SunEngine/blob/master/Client/src/statics/SunEngine.svg" width="250" alt="SunEngine Logo" />

Version: 1.18.1

Demo site: [demo.sunengine.site](http://demo.sunengine.site). 

[Текст на русском](README.RU.md).

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

- Asp.Net Core 3.0
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
- [.NET Core 3.0 + ASP.NET Core Runtimes](https://dotnet.microsoft.com/download/dotnet-core/3.0)
- [NodeJs](https://nodejs.org/en/download/)
- [Npm](https://www.npmjs.com/) (will already be installed along with NodeJs)
- [Quasar CLI](https://quasar.dev/quasar-cli/installation) `npm install -g @quasar/cli`

### Launch for development
#### Launch from console
1. Download the project code from the official repository https://github.com/sunengine/SunEngine (if not done)
2. Rename files and folders with the suffix `-template`, and remove this suffix. (if not done)
3. Go to folder `SunEngine/SunEngine.Cli`.
4. Create a database `SunEngine` in the selected DBMS. (if not done)
5. Register the provider name and connection string in the file `SunEngine/SunEngine.Cli/Config/DataBaseConnection.json`.
6. Fill the database with initial data `dotnet run migrate init seed` (if not done).
7. Run server `dotnet run server`.
8. Go to folder `SunEngine/Client`.
9. Install npm modules `npm install` (if not done yet).
10. Run client `quasar dev` — browser will be opened.

### Contacts
- [Dimitrij Polianin](https://sunengine.site/user/okeanij)
- Telegram: [@okeanij](https://t.me/Okeanij)
- Project's Telegram group: [@SunEngine](https://t.me/SunEngine)
