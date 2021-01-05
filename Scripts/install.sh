#!/bin/bash

###############################################################
#─────────────────────────────────────────────────────────────#
#───────────────╔══════════════════════════════╗──────────────#
#───────────────║ install and update SunEngine ║──────────────#
#───────────────║     Script version: 0.81     ║──────────────#
#───────────────╚══════════════════════════════╝──────────────#
#─────────────────────────────────────────────────────────────#
#─────────── https://github.com/sunengine/SunEngine ──────────#
#─────────────────── https://sunengine.site ──────────────────#
#─────────────────────────────────────────────────────────────#
###############################################################

#region определяем язык пользователя

if [[ "$LANG" == "ru_RU.UTF-8" ]]
then
  # --help текст на русском
  TEXT_HELP=(
    "    -h, --help        Информация о командах"
    "    -d, --directory   Полный путь к папке установки, по умолчанию /var/www/(--host)"
    "    -H, --host        Домен либо ip (заодно имя БД и имя пользователя БД)"
    "        --port        Внутренний порт используемый сервером Kestrel до проксирования через Nginx (стоит задавать только если стандартный порт 5050 занят)"
    "    -u  --user        Пользователь от имено которого будет работать сервис и которому будут принадлежать все файлы сайта"
    "    -p, --pgpass      Пароль PostgreSQL от юзера postgres (нужно для создания новой БД)"
    "                      (какой сейчас установлен или какой поставить при установке PostgreSQL)"
    "    -P, --pguserpass  Пароль PostgreSQL от пользователя от оимени которого создается БД (значение ключа --host)"
    "                      (значение по умолчанию 16 случайных сииволов, задавать вручную только когда пользователь уже создан)"
    "    -s, --silent      Установка без участия пользователя (на все вапросы отвечать да)"
  )
  TEXT_APTROOTERROR="Поскольку скрипт использует apt то нужны права администратора"
  TEXT_INSTALLED="установлен"
  TEXT_DBDETECTED="Обнаружена БД скорее всего она осталась от предыдущей установки, удалить?"
  TEXT_GITDETECTED="Обнаружены файлы движка скорее всего они остались от предыдущей установки, удалить?"
  TEXT_DBALREADYEXISTS="БД уже есть, возможно вы хотели запустить обновление а не установку?"
  TEXT_GITALREADYEXISTS="Установка прервана чтобы не повредить существующие конфиги, сохраните важную информацию и повторите установку после удаления папки с файлами движка"
  TEXT_DOWNLOADERROR="Не удалось скачать файлы движка"
  TEXT_HTTPSCONFIG="настраиваем сертификат для https"
  TEXT_CREATEADMINACCOUNTTITLE="создание аккаунта администратора сайта"
  TEXT_CREATEADMINACCOUNTUSERNAME="Нужно создать аккаунт администратора сайта, введите имя пользователя нового аккаунта"
  TEXT_CREATEADMINACCOUNTPASS="Введите пароль нового аккаунта"
  TEXT_CREATEADMINACCOUNTEMAIL="Введите email нового аккаунта"
  TEXT_INSTALL_ERROR="Установка прервана."
  TEXT_INPUT_HOST_TITLE="HOST"
  TEXT_INPUT_HOST_INPUTBOX="хост на который должен откликатся сайт (example.com, localhost, 127.0.0.1:8008)"
  TEXT_INPUT_HOST_SILENTERROR="для успешной установки хост обязательно должен быть задан"
  TEXT_INPUT_USER_TITLE="USER"
  TEXT_INPUT_USER_INPUTBOX="Пользователь от имено которого будет работать сервис и которому будут принадлежать все файлы сайта"
  TEXT_INPUT_DIRECTORY_TITLE="DIRECTORY"
  TEXT_INPUT_DIRECTORY_INPUTBOX="Полный путь к папке установки"
  TEXT_INPUT_PGUSERPASS_TITLE="USER"
  TEXT_FUNC_INPUT_HOST_SILENTERROR="PostgreSQL пользователь $1 уже существует, нужен пароль передайте его ключем "
  TEXT_INPUT_PORT_TITLE="Kestrel port"
  TEXT_INPUT_PORT_INPUTBOX="Внутренний порт используемый сервером Kestrel до проксирования через Nginx, стоит задавать только если стандартный порт 5050 занят или вы по каким либо причинам не хотите использовать этот порт"
  TEXT_FUNC_SYSTEMDCONFIG() {
    # настраиваю systemd демон sunengine.site.service
    echo "настраиваю systemd демон $1.service"
  }
  TEXT_FUNC_USERADMINCREATED() {
    # Создан пользователь администратор
    # имя пользователя: $ADMINUSERNAME
    # пароль: $ADMINPASSWORD
    # email: $ADMINEMAIL"
    echo -e "Создан пользователь администратор\nимя пользователя: $1\nпароль: $2\nemail: $3"
  }
  TEXT_FUNC_USERCREATED() {
    # Пользователь "sunenginesite" создан
    echo "Пользователь \"$1\" создан"
  }
  TEXT_FUNC_ADDREPO() {
    # Для установки PostgreSQL нужны репозитории от apt.postgresql.org\n\nДобавить репозитории?
    # Для установки dotnet нужны репозитории от packages.microsoft.com\n\nДобавить репозитории?
    echo "Для установки $1 нужны репозитории от $2\n\nДобавить репозитории?"
  }
  TEXT_FUNC_ADDREPO() {
    # Добавляю репозитории packages.microsoft.com в /etc/apt/sources.list.d/microsoft-prod.list
    # Добавляю репозитории apt.postgresql.org в /etc/apt/sources.list.d/pgdg.list
    echo "Добавляю репозитории $1 в $2"
  }
  TEXT_FUNC_NOTSUPPORTED() {
    # PostgreSQL не поддерживает Debian Buster а значит SunEngin запустить не получится
    # dotnet не поддерживает Debian Buster а значит SunEngin запустить не получится
    echo "$1 не поддерживает $2 $3 а значит SunEngin запустить не получится"
  }
  TEXT_FUNC_PGUSERCREATED() {
    # Создан PostgreSQL пользователь "sunengine.site" с паролем "qwerty"
    echo "Создан PostgreSQL пользователь \"$1\" с паролем \"$2\""
  }
  TEXT_FUNC_PGDBCREATED() {
    # БД "sunengine.site" создана
    echo "БД \"$1\" создана"
  }
  TEXT_FUNC_INPUT_PGUSERPASS_INPUTBOX() {
    echo "PostgreSQL пользователь $1 уже существует, введите пароль от этого пользователя"
  }
else
 then
  # --help english text (autotranslate from russian)
  TEXT_HELP = (
    "-h, --help Command information"
    "-d, --directory full path to the installation folder, by default / var / www / (- host)"
    "-H, --host domain or ip (along with the database name and database username)"
    "--port Internal port used by the Kestrel server before proxying through Nginx (should only be set if the default port 5050 is busy)"
    "-u --user User from whose name the service will run and who will own all site files"
    "-p, --pgpass PostgreSQL password from the postgres user (needed to create a new database)"
    "(which is currently installed or which one to install when installing PostgreSQL)"
    "-P, --pguserpass PostgreSQL password from the user from whose name the database is created (the value of the --host key)"
    "(default value 16 random characters, set manually only when the user is already created)"
    "-s, --silent Installation without user intervention (answer yes to all queries)"
  )
  TEXT_APTROOTERROR = "Since the script uses apt, you need administrator rights"
  TEXT_INSTALLED = "installed"
  TEXT_DBDETECTED = "The database was found. Most likely it was left from a previous installation, should you delete it?"
  TEXT_GITDETECTED = "Engine files found, most likely they are left over from a previous installation, delete?"
  TEXT_DBALREADYEXISTS = "There is already a database, maybe you wanted to run an update rather than an installation?"
  TEXT_GITALREADYEXISTS = "Installation aborted to avoid damaging existing configs, save important information and retry installation after deleting the folder with engine files"
  TEXT_DOWNLOADERROR = "Failed to download engine files"
  TEXT_HTTPSCONFIG = "configuring the certificate for https"
  TEXT_CREATEADMINACCOUNTTITLE = "create site administrator account"
  TEXT_CREATEADMINACCOUNTUSERNAME = "You need to create a site administrator account, enter the username of the new account"
  TEXT_CREATEADMINACCOUNTPASS = "Please enter your new account password"
  TEXT_CREATEADMINACCOUNTEMAIL = "Please enter your new account email"
  TEXT_INSTALL_ERROR = "Installation aborted."
  TEXT_INPUT_HOST_TITLE = "HOST"
  TEXT_INPUT_HOST_INPUTBOX = "host to which the site should respond (example.com, localhost, 127.0.0.1:8008)"
  TEXT_INPUT_HOST_SILENTERROR = "the host must be specified for a successful installation"
  TEXT_INPUT_USER_TITLE = "USER"
  TEXT_INPUT_USER_INPUTBOX = "The user on whose behalf the service will run and who will own all the site files"
  TEXT_INPUT_DIRECTORY_TITLE = "DIRECTORY"
  TEXT_INPUT_DIRECTORY_INPUTBOX = "Full path to installation folder"
  TEXT_INPUT_PGUSERPASS_TITLE = "USER"
  TEXT_FUNC_INPUT_HOST_SILENTERROR = "PostgreSQL user $ 1 already exists, you need a password, pass it with a key"
  TEXT_INPUT_PORT_TITLE = "Kestrel port"
  TEXT_INPUT_PORT_INPUTBOX = "The internal port used by the Kestrel server before proxying through Nginx, you should only set it if the standard port 5050 is busy or you do not want to use this port for some reason"
  TEXT_FUNC_SYSTEMDCONFIG () {
    # setup the systemd daemon sunengine.site.service
    echo "setting up systemd daemon $ 1.service"
  }
  TEXT_FUNC_USERADMINCREATED () {
    # Administrator user created
    # username: $ ADMINUSERNAME
    # password: $ ADMINPASSWORD
    # email: $ ADMINEMAIL "
    echo -e "Created user admin \ nusername: $ 1 \ npassword: $ 2 \ nemail: $ 3"
  }
  TEXT_FUNC_USERCREATED () {
    # User "sunenginesite" has been created
    echo "User \" $ 1 \ "created"
  }
  TEXT_FUNC_ADDREPO () {
    # To install PostgreSQL you need repositories from apt.postgresql.org \ n \ nAdd repositories?
    # To install dotnet you need repositories from packages.microsoft.com \ n \ nAdd repositories?
    echo "To install $ 1 you need repositories from $ 2 \ n \ nAdd repositories?"
  }
  TEXT_FUNC_ADDREPO () {
    # Add packages.microsoft.com repositories to /etc/apt/sources.list.d/microsoft-prod.list
    # Add repositories apt.postgresql.org to /etc/apt/sources.list.d/pgdg.list
    echo "Adding repositories $ 1 to $ 2"
  }
  TEXT_FUNC_NOTSUPPORTED () {
    # PostgreSQL does not support Debian Buster and therefore SunEngin cannot be started
    # dotnet does not support Debian Buster, which means SunEngin will not run
    echo "$ 1 does not support $ 2 $ 3 so SunEngin will not run"
  }
  TEXT_FUNC_PGUSERCREATED () {
    # Created PostgreSQL user "sunengine.site" with password "qwerty"
    echo "Created PostgreSQL user \" $ 1 \ "with password \" $ 2 \ ""
  }
  TEXT_FUNC_PGDBCREATED () {
    # DB "sunengine.site" is created
    echo "DB \" $ 1 \ "created"
  }
  TEXT_FUNC_INPUT_PGUSERPASS_INPUTBOX () {
    echo "PostgreSQL user $ 1 already exists, please enter password for this user"
  }
fi

# версия dotnet требуемая для работы
dotnetPackageName="aspnetcore-runtime-3.1"
dotnetVersionName="Microsoft.AspNetCore.App 3.1"

#region ввод данных от пользователя

# Инициализация начальных значений переменных
# папка установки
PARAM_DIRECTORY=""
PARAM_DIRECTORY_FLAG=false

# пароль от пользователя postgres (нужно для создания новой БД)
PARAM_PGPASS="postgre"
PARAM_PGPASS_FLAG=true

# порт на котором работает PostgreSQL
# PGPORT="5432"

# пользователь владелец файлов сайта
PARAM_USER=""
PARAM_USER_FLAG=false

# пароль от пользователя владельца базы данных
PARAM_PGUSERPASS=$(head /dev/urandom | tr -dc A-Za-z0-9 | head -c 16)
PARAM_PGUSERPASS_FLAG=false

# на что откликатся
PARAM_HOST="localhost"
PARAM_HOST_FLAG=false

# порт на который повесить наш бекенд
PARAM_PORT="5050"

# "тихий" режим, без никаких запросов от пользователя
SILENT=false

while true; do
  case "$1" in
    -h | --help )
      for t in "${TEXT_HELP[@]}"; do
        echo "$t";
      done
      exit 0;
    ;;
    -d | --directory )
      # папка установки
      PARAM_DIRECTORY="$2"
      PARAM_DIRECTORY_FLAG=true
      shift 2
    ;;
    -p | --pgpass )
      # пароль от пользователя postgres (нужно для создания новой БД)
      PARAM_PGPASS="$2"
      PARAM_PGPASS_FLAG=true
      shift 2
    ;;
    -u | --user )
      PARAM_USER="$2"
      PARAM_USER_FLAG=true
      shift 2
    ;;
    -P | --pguserpass )
      PARAM_PGUSERPASS="$2"
      PARAM_PGUSERPASS_FLAG=true
      shift 2
    ;;
    -H | --host )
      PARAM_HOST="$2"
      PARAM_HOST_FLAG=true
      shift 2
    ;;
    --port )
      PARAM_PORT="$2"
      shift 2
    ;;
    -s | --silent )
      SILENT=true
      shift
    ;;
    -- )
      shift
      break
    ;;
    * )
      break
    ;;
  esac
done

# если нету прав рута то дальше делать нечего
if (( $EUID != 0 )); then
  echo $TEXT_APTROOTERROR 1>&2
  exit 100
fi


# Запросить ввод с клавиатуры
# $1 - флаг, true если значение уже задано через параметры или как-то еще в противном случае false
# $2 - title
# $3 - inputbox
# $4 - значение по умолчанию
# $5 - ошибка если тихий режим и нету значения
ReadText() {
  # если тихий режим
  if $SILENT
  then
    # если значение не задано
    if ! $1
    then
      echo $5
      exit 1
    fi
  fi
  # запрашиваем ввод с клавиатуры
  ret=$(whiptail --title "$2" --inputbox "$3" 10 60 "$4" 3>&1 1>&2 2>&3)
  # Обработка кнопки "Отмена"
  exitstatus=$?
  if [[ $exitstatus != 0 ]]
  then
    echo "$TEXT_INSTALL_ERROR $2"
    exit 1
  fi
  echo $ret
}

# проверка хоста на правильность ввода
tempVar=$(ReadText $PARAM_HOST_FLAG "$TEXT_INPUT_HOST_TITLE" "$TEXT_INPUT_HOST_INPUTBOX" "$PARAM_HOST" "$TEXT_INPUT_HOST_SILENTERROR") || { echo "$tempVar" & exit 1; }
PARAM_HOST=$tempVar

# Если пользователь не задан то формируем его имя с хоста
if ! $PARAM_USER_FLAG
then
  PARAM_USER=$(echo "$PARAM_HOST" | tr -dc '[:alnum:]\n\r')
  PARAM_USER_FLAG=true
fi  # проверка юзера на правильность ввода
tempVar=$(ReadText $PARAM_USER_FLAG "$TEXT_INPUT_USER_TITLE" "$TEXT_INPUT_USER_INPUTBOX" "$PARAM_USER" "") || { echo "$tempVar" & exit 1; }
PARAM_USER=$tempVar

# если директория не задана задаем директорию по умолчанию
if ! $PARAM_DIRECTORY_FLAG
then
  PARAM_DIRECTORY="/var/www/$PARAM_USER/"
  PARAM_DIRECTORY_FLAG=true
fi  # проверка директории на правильность ввода
tempVar=$(ReadText $PARAM_DIRECTORY_FLAG "$TEXT_INPUT_DIRECTORY_TITLE" "$TEXT_INPUT_DIRECTORY_INPUTBOX" "$PARAM_DIRECTORY" "$TEXT_INPUT_DIRECTORY_SILENTERROR") || { echo "$tempVar" & exit 1; }
PARAM_DIRECTORY=$tempVar

tempVar=$(ReadText $PARAM_PGPASS_FLAG "$TEXT_INPUT_PGPASS_TITLE" "$TEXT_INPUT_PGPASS_INPUTBOX" "$PARAM_PGPASS" "") || { echo "$tempVar" & exit 1; }
PARAM_PGPASS=$tempVar

tempVar=$(ReadText $PARAM_PGUSERPASS_FLAG "$TEXT_INPUT_PGUSERPASS_TITLE" "$TEXT_INPUT_PGUSERPASS_INPUTBOX" "$PARAM_PGUSERPASS" "") || { echo "$tempVar" & exit 1; }
PARAM_HOST=$tempVar

tempVar=$(ReadText $PARAM_PORT_FLAG "$TEXT_INPUT_PORT_TITLE" "$TEXT_INPUT_PORT_INPUTBOX" "$PARAM_PORT" "") || { echo "$tempVar" & exit 1; }
PARAM_PORT=$tempVar

SILENTINSTALL="debconf-apt-progress -- "
if $SILENT
then
  SILENTINSTALL=""
fi

#endregion


# Узнаем данные о системе на которой нас запустили
# имя дисрибутива
distr=$(grep ^ID= /etc/*-release | cut -f2 -d'=')
# версия дистрибутива
version=$(grep ^VERSION_ID= /etc/*-release | cut -f2 -d'=' | sed -e 's/^"//' -e 's/"$//')
version_codename=$(grep ^VERSION_CODENAME= /etc/*-release | cut -f2 -d'=')

# ставим "зависимости" скрипта
installDependencies()
{
  $SILENTINSTALL apt-get update
  $SILENTINSTALL apt-get -y install wget apt-transport-https dpkg git nginx certbot
  
  if [ "$distr" == "ubuntu" ]
  then
    $SILENTINSTALL apt-get -y install gnupg2
  fi
}

# Окно ошибки
Error() {
  if [[ $SILENT ]]
  then
    echo "$2"
  else
    whiptail --title "$1" --msgbox  "$2" 10 60
  fi
  exit 0
}

#region dotnet

# Добавление репозиториев Microsoft для dotnet
addDotnetRepo() {
  echo $(TEXT_FUNC_ADDREPO "Microsoft" "/etc/apt/sources.list.d/microsoft-prod.list")
  case "$distr" in
    "debian" )
      if [ "$version" != "10" ] && [ "$version" != "9" ]
      then
        Error "dotnet" "$(TEXT_FUNC_NOTSUPPORTED "dotnet" "$distr" "$version_codename")"
      fi
      # добавляем репозиторий
      if ($SILENT || whiptail --title "dotnet" --yesno "$(TEXT_FUNC_ADDREPO "dotnet" "packages.microsoft.com")" 11 60) then
        wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg
        mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
        wget -q https://packages.microsoft.com/config/debian/$version/prod.list
        mv prod.list /etc/apt/sources.list.d/microsoft-prod.list
        chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg
        chown root:root /etc/apt/sources.list.d/microsoft-prod.list
      else
        exit 0
      fi
    ;;
    "ubuntu" )
      if [ "$version" != "16.04" ] && [ "$version" != "18.04" ] && [ "$version" != "19.04" ] && [ "$version" != "19.10" ] && [ "$version" != "20.04"]
      then
        Error "dotnet" "$(TEXT_FUNC_NOTSUPPORTED "dotnet" "$distr" "$version_codename")"
      fi
      if ($SILENT || whiptail --title "dotnet" --yesno "$(TEXT_FUNC_ADDREPO "dotnet" "packages.microsoft.com")" 11 60) then
        wget -q https://packages.microsoft.com/config/ubuntu/$version/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
        dpkg -i packages-microsoft-prod.deb
        if [ "$version" = "18.04" ]
        then
          add-apt-repository universe
        fi
      else
        exit 0
      fi
    ;;
    * )
      Error "dotnet" "$(TEXT_FUNC_NOTSUPPORTED "dotnet" "$distr" "$version_codename")"
    ;;
  esac
  $SILENTINSTALL apt-get update
}

# Установка dotnet нужной версии если таковая еще не установлена
checkDotnetVersion() {
  if ((! echo $(whereis dotnet) | grep "/usr/bin/dotnet" > /dev/null)
  && (! echo $(dotnet --list-runtimes) | grep "$dotnetVersionName" > /dev/null))
  then
    $SILENTINSTALL apt-get -y install $dotnetPackageName
  fi
  echo "$dotnetPackageName $TEXT_INSTALLED"
}

#endregion

# Добавление репозиториев PostgreSQL
addPgSQLRepo() {
  echo $(TEXT_FUNC_ADDREPO "PostgreSQL" "/etc/apt/sources.list.d/pgdg.list")
  case $distr in
    debian | ubuntu )
      # добавляем репозиторий
      if ($SILENT || whiptail --title "PostgreSQL" --yesno "$(TEXT_FUNC_ADDREPO "PostgreSQL" "apt.postgresql.org")" 11 60) then
        echo -e "deb http://apt.postgresql.org/pub/repos/apt/ $version_codename-pgdg main" > pgdg.list
        mv pgdg.list /etc/apt/sources.list.d/pgdg.list
        chown root:root /etc/apt/sources.list.d/pgdg.list
        wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | apt-key add -
      else
        exit 0
      fi
    ;;
    * )
      Error "PostgreSQL" "$(TEXT_FUNC_NOTSUPPORTED "PostgreSQL" "$distr" "$version_codename")"
    ;;
  esac
  $SILENTINSTALL apt-get update
}


checkPostgreSQLVersion() {
  if ([[ "$(whereis psql)" != *"/usr/bin/psql"* ]] ||
  (! echo $(psql --version) | grep "(PostgreSQL) 11" > /dev/null))
  then
    $SILENTINSTALL apt-get -y install postgresql-11
  else
    echo "postgresql-11 $TEXT_INSTALLED"
  fi
}

# Проверяем пользователя от бд, ведь для безопасности для всего должны быть свои пользователи верно?
checkPgUser() {
  tempVar=$(su - postgres -c "PGPASSWORD=$PARAM_PGPASS psql -c \"CREATE USER '$PARAM_HOST' WITH PASSWORD '$PARAM_PGUSERPASS';\"" )
  # todo заменить на цикл
  if [ "$tempVar" != "CREATE ROLE" ]
  then
    PARAM_PGUSERPASS_FLAG=false
    tempVar=$(ReadText $PARAM_PGUSERPASS_FLAG "$TEXT_INPUT_PGUSERPASS_TITLE" "$(TEXT_FUNC_INPUT_PGUSERPASS_INPUTBOX "$PARAM_HOST")" "$PARAM_PGUSERPASS" "") || { echo "$tempVar" & exit 1; }
    PARAM_PGUSERPASS=$tempVar
    # todo добавить проверку валидности пароля (что пароль подходит к учетке)
  else
    echo "$(TEXT_FUNC_PGUSERCREATED "$PARAM_HOST" "$PARAM_PGUSERPASS")"
  fi
}

createDb() {
  su - postgres -c "PGPASSWORD=$PARAM_PGPASS psql -c \"CREATE DATABASE \\\"$PARAM_HOST\\\" OWNER \\\"$PARAM_HOST\\\";\""
  echo "$(TEXT_FUNC_PGDBCREATED "$PARAM_HOST")"
}

# проверяем существование БД
checkDbCreated() {
  if (su - postgres -c "PGPASSWORD=$PARAM_PGPASS psql -l -At" | grep "^$PARAM_HOST|" > /dev/null)
  then
    if ($SILENT || whiptail --title "PostgreSQL" --yesno "$TEXT_DBDETECTED" 11 60)
    then
      su - postgres -c "PGPASSWORD=$PARAM_PGPASS dropdb \"$PARAM_HOST\""
      createDb
    else
      echo "$TEXT_DBALREADYEXISTS"
      exit 0;
    fi
  else
    createDb
  fi
}

# проверяем существования пользователя
createUser() {
  grep "$PARAM_USER:" /etc/passwd >/dev/null
  if [ $? != 0 ]
  then
    useradd $PARAM_USER --home-dir "$PARAM_DIRECTORY" --create-home
    echo "$(TEXT_FUNC_USERCREATED "$PARAM_USER")"
  fi
}

# качаем файлы SunEngine
loadFiles() {
  DIRGIT=$(echo "$PARAM_DIRECTORY/SunEngine.Build" | tr -s '/')
  DIR=$(echo "$PARAM_DIRECTORY" | tr -s '/')
  
  # если директория уже существует то удаляем если юзер розрешил
  if [ -d "$PARAM_DIRECTORY" ];
  then
    #если включен сайлент режим то ошибка
    if ( ! $SILENT && whiptail --yesno "$TEXT_GITDETECTED" 11 60)
    then
      rm -r "$PARAM_DIRECTORY"
    else
      echo "$TEXT_GITALREADYEXISTS"
      exit 0;
    fi
  fi
  
  git clone "https://github.com/sunengine/Build" "$DIRGIT" > /dev/null
  exitstatus=$?
  if [ $exitstatus != 0 ]
  then
    echo "$TEXT_DOWNLOADERROR"
    exit 0;
  fi
  
  mv $DIRGIT/* $DIR
}

initConfig() {
  # DataBaseConnection.json
  sed -i "s/<DataBaseName>/$PARAM_HOST/g" "${DIR}Config.server.template/DataBaseConnection.json"
  sed -i "s/<DataBaseUser>/$PARAM_HOST/g" "${DIR}Config.server.template/DataBaseConnection.json"
  sed -i "s/<DataBasePassword>/$PARAM_PGUSERPASS/g" "${DIR}Config.server.template/DataBaseConnection.json"
  
  # SunEngine.json
  sed -i "s/<domain>/$PARAM_HOST/g" "${DIR}Config.server.template/SunEngine.json"
  sed -i "s/<port>/$PARAM_PORT/g" "${DIR}Config.server.template/SunEngine.json"
  sed -i "s!auto!$DIR!g" "${DIR}Config.server.template/SunEngine.json"
  
  # запрашиваем имя пользователя
  ADMINUSERNAME="admin"
  tempVar=$(ReadText true "$TEXT_CREATEADMINACCOUNTTITLE" "$TEXT_CREATEADMINACCOUNTUSERNAME" "admin" "") || { echo "$tempVar" & exit 1; }
  ADMINUSERNAME=$tempVar
  
  # запрашиваем пароль
  ADMINPASSWORD="nimda"
  tempVar=$(ReadText true "$TEXT_CREATEADMINACCOUNTTITLE" "$TEXT_CREATEADMINACCOUNTPASS" "nimda" "") || { echo "$tempVar" & exit 1; }
  ADMINPASSWORD=$tempVar
  
  # запрашиваем почту
  ADMINEMAIL="admin@email."
  tempVar=$(ReadText true "$TEXT_CREATEADMINACCOUNTTITLE" "$TEXT_CREATEADMINACCOUNTEMAIL" "admin@email." "") || { echo "$tempVar" & exit 1; }
  ADMINEMAIL=$tempVar
  
  # DataBaseConnection.json
  sed -i "s/<admin-email>/$ADMINEMAIL/g" "${DIR}Config.server.template/Init/Users.json"
  sed -i "s/<admin-user-name>/$ADMINUSERNAME/g" "${DIR}Config.server.template/Init/Users.json"
  sed -i "s/<admin-password>/$ADMINPASSWORD/g" "${DIR}Config.server.template/Init/Users.json"
  
  echo -e "$(TEXT_FUNC_USERADMINCREATED "$ADMINUSERNAME" "$ADMINPASSWORD" "$ADMINEMAIL")"
  
  # index-page.json
  sed -i "s/<admin-user-name>/$ADMINUSERNAME/g" "${DIR}Config.server.template/Init/Materials/index-page.json"
  
  # копируем настройки с темплейта
  cp -r "${DIR}Config.server.template" "${DIR}Config"
  
  # меняем владельца папки
  chown ${PARAM_USER}:${PARAM_USER} -R *
}

# Заполняем БД данными
initDb() {
  dotnet "${DIR}Server/SunEngine.dll" config:"${DIR}Config" init migrate
}

# systemd
initDemon() {
  echo "$(TEXT_FUNC_SYSTEMDCONFIG "$PARAM_HOST")"
  sed -i "s/<host>/$PARAM_HOST/g" "${DIR}Resources/systemd.template"
  sed -i "s!<dir>!$DIR!g" "${DIR}Resources/systemd.template"
  sed -i "s/<user>/$PARAM_USER/g" "${DIR}Resources/systemd.template"
  cp "${DIR}Resources/systemd.template" "/etc/systemd/system/$PARAM_HOST.service"
  
  # добавляем сервис в автозагрузку
  systemctl enable $PARAM_HOST
  # запускаем сервис
  systemctl start $PARAM_HOST
}

# настраиваем nginx в том числе ssl сертификат
initNGINX() {
  # nginx стартовый конфиг
  sed -i "s/<host>/$PARAM_HOST/g" "${DIR}Resources/nginx.template"
  sed -i "s!<wwwroot>!${DIR}wwwroot!g" "${DIR}Resources/nginx.template"
  cp "${DIR}Resources/nginx.template" "/etc/nginx/sites-available/$PARAM_HOST.conf"
  ln -s "/etc/nginx/sites-available/$PARAM_HOST.conf" "/etc/nginx/sites-enabled/$PARAM_HOST.conf"
  nginx -s reload
  
  echo "$TEXT_HTTPSCONFIG"
  certbot certonly --webroot -w "${DIR}wwwroot" -d $PARAM_HOST -n
  
  # nginx рабочий конфиг
  sed -i "s/<host>/$PARAM_HOST/g" "${DIR}Resources/nginxssl.template"
  sed -i "s!<wwwroot>!${DIR}wwwroot!g" "${DIR}Resources/nginxssl.template"
  sed -i "s/<port>/$PARAM_PORT/g" "${DIR}Resources/nginxssl.template"
  cp "${DIR}Resources/nginxssl.template" "/etc/nginx/sites-available/$PARAM_HOST.conf"
}

###############################################################
#─────────────────────────────────────────────────────────────#
#────────────╔═══╗─╔═══╗─╔╗──╔╗─╔╗──╔╗─╔═══╗─╔═══╗────────────#
#────────────║╔═╗║─║╔═╗║─║╚╗╔╝║─║╚╗╔╝║─║╔══╝─║╔═╗║────────────#
#────────────║╚═╝║─║║─║║─╚╗║║╔╝─╚╗║║╔╝─║╚══╗─║╚═╝║────────────#
#────────────║╔══╝─║╚═╝║──║╚╝║───║╚╝║──║╔══╝─║╔╗╔╝────────────#
#────────────║║────║╔═╗║──╚╗╔╝───╚╗╔╝──║╚══╗─║║║╚╗────────────#
#────────────╚╝────╚╝─╚╝───╚╝─────╚╝───╚═══╝─╚╝╚═╝────────────#
#─────────────────────────────────────────────────────────────#
#──────────────────── http://t.me/pavver ─────────────────────#
#─────────────────────────────────────────────────────────────#
###############################################################

# ставим зависимости скрипта
installDependencies

# репы мелкософта добавлены?
if [ ! -f "/etc/apt/sources.list.d/microsoft-prod.list" ]
then
  addDotnetRepo
fi

#проверяем установлен дотнет или нет, если нет то ставим
checkDotnetVersion


# репы постгреса добавлены?
if [ ! -f "/etc/apt/sources.list.d/pgdg.list" ]
then
  addPgSQLRepo
fi

# проверяем установлен постгрес или нет
checkPostgreSQLVersion

# создаем pg пользователя или запрашиваем пароль если пользователь уже есть
checkPgUser

# проверяем существование БД, если нужно создаем или заменяем
checkDbCreated

# создаем пользователя которому будет принадлежать сайт
createUser

# качаем файлы движка
loadFiles

# задаем конфиги движка
initConfig

# Заполняем БД данными
initDb

# systemd
initDemon

# настраиваем nginx в том числе ssl сертификат
initNGINX

nginx -s reload
