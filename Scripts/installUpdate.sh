#!/bin/bash

#   ***************************************
#   *                                     *
#   *    install and update SunEngine     *
#   *        Script version: 0.5          *
#   *                                     *
#   ***************************************

DIRECTORY=""
PGPASS="postgre"
PGPORT="5432"
PGUSERPASS=$(head /dev/random | tr -dc A-Za-z0-9 | head -c 16)
USER=""
PGUSERPASSFLAG=true
HOST="localhost"
SILENT=false
PORT="5050"

#region разбор параметров

while true; do
    case "$1" in
        -h | --help )
            echo "    -h, --help        Информация о командах";
            echo "    -d, --directory   Полный путь к папке установки, по умолчанию /var/www/(--host)";
            echo "    -H, --host        Домен либо ip (заодно имя БД и имя пользователя БД)";
            echo "        --port        Внутренний порт используемый сервером Kestrel до проксирования через Nginx (стоит задавать только если стандартный порт 5050 занят)";
            echo "    -u  --user        Пользователь от имено которого будет работать сервис и которому будут принадлежать все файлы сайта";
            echo "    -p, --pgpass      Пароль PostgreSQL от юзера postgres";
            echo "                      (какой сейчас установлен или какой поставить при установке PostgreSQL)";
            echo "    -P, --pguserpass  Пароль PostgreSQL от пользователя от оимени которого создается БД (значение ключа --host)";
            echo "                      (значение по умолчанию 16 случайных сииволов, задавать вручную только когда пользователь уже создан)";
            echo "    -s, --silent      Установка без участия пользователя (на все вапросы отвечать да)";
            exit 0;
        ;;
        -d | --directory )
            DIRECTORY="$2"
            shift 2
        ;;
        -p | --pgpass )
            PGPASS="$2"
            shift 2
        ;;
        -u | --user )
            USER="$2"
            shift 2
        ;;
        -P | --pguserpass )
            PGUSERPASS="$2"
            PGUSERPASSFLAG=false
            shift 2
        ;;
        -H | --host )
            HOST="$2"
            shift 2
        ;;
        --port )
            PORT="$2"
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

# Если пользователь не задан то формируем его имя с хоста
if [ -z "$USER" ]
then
    USER=$(echo "$HOST" | tr -dc '[:alnum:]\n\r')
fi

# если директория не задана задаем директорию по умолчанию
if [ -z "$DIRECTORY" ]
then
    DIRECTORY="/var/www/$USER/"
fi

SILENTINSTALL="debconf-apt-progress -- "
if $SILENT
then
    SILENTINSTALL=""
fi

#endregion

if (( $EUID != 0 )); then
    echo "Поскольку скрипт использует apt то нужны права администратора" 1>&2
    exit 100
fi

# Узнаем данные о системе на которой нас запустили
# имя дисрибутива
distr=$(grep ^ID /etc/*-release | cut -f2 -d'=')
# версия дистрибутива
version=$(grep ^VERSION_ID /etc/*-release | cut -f2 -d'=' | sed -e 's/^"//' -e 's/"$//')

# ставим "зависимости" скрипта
$SILENTINSTALL apt-get update
$SILENTINSTALL apt-get -y install wget apt-transport-https dpkg git

# Окно ошибки
Error() {
    if [[ $SILENT ]]
    then
        echo "$1 $2"
    else
        whiptail --title "$1" --msgbox  "$2" 10 60
    fi
    exit 0
}

#region dotnet

dotnetPackageName="aspnetcore-runtime-3.1"
dotnetVersionName="Microsoft.AspNetCore.App 3.1"

# Добавление репозиториев Microsoft для dotnet
addDotnetRepo() {
    case "$distr" in
        "debian" )
            if [ "$version" != "10" ] && [ "$version" != "9" ]
            then
                noSupportError "dotnet" "dotnet не поддерживает $distr $version а значит SunEngin запустить не получится"
            fi
            # добавляем репозиторий
            if ($SILENT || whiptail --title "dotnet" --yesno "Для установки dotnet нужны репозитории от Microsoft.\n\nДобавить репозитории?" 11 60) then
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
            if [ "$version" != "16.04" ] && [ "$version" != "18.04" ] && [ "$version" != "19.04" ] && [ "$version" != "19.10" ]
            then
                noSupportError "dotnet" "dotnet не поддерживает $distr $version а значит SunEngin запустить не получится"
            fi
            if ($SILENT || whiptail --title "dotnet" --yesno "Для установки dotnet нужны репозитории от Microsoft.\n\nДобавить репозитории?" 11 60) then
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
            noSupportError "dotnet" "dotnet не поддерживает $distr $version а значит SunEngin запустить не получится"
        ;;
    esac
    echo "добавлены репозитории Microsoft в /etc/apt/sources.list.d/microsoft-prod.list"
    $SILENTINSTALL apt-get update
}

# Установка dotnet нужной версии если таковая еще не установлена
checkDotnetVersion() {
    if ((! echo $(whereis dotnet) | grep "/usr/bin/dotnet" > /dev/null)
    && (! echo $(dotnet --list-runtimes) | grep "$dotnetVersionName" > /dev/null))
    then
        if ($SILENT || whiptail --title "dotnet" --yesno "$dotnetPackageName не установлен, установить?" 11 60) then
            $SILENTINSTALL apt-get -y install $dotnetPackageName
        else
            exit 0
        fi
    fi
    echo "$dotnetPackageName установлен"
}

# репы мелкософта добавлены?
if [ ! -f "/etc/apt/sources.list.d/microsoft-prod.list" ]
then
    addDotnetRepo
fi

#проверяем установлен дотнет или нет
checkDotnetVersion

#endregion

checkPostgreSQLVersion() {
    if ((! echo $(whereis psql) | grep "/usr/bin/psql" > /dev/null)
    && (! echo $(psql --version) | grep "(PostgreSQL) 11" > /dev/null))
    then
        
        if ($SILENT || whiptail --title "PostgreSQL" --yesno "postgresql-11 не установлен, установить?" 11 60)
        then
            $SILENTINSTALL apt-get -y install postgresql-11
        fi
    fi
    echo "postgresql-11 установлен"
}

checkPostgreSQLVersion

# Проверяем пользователя от бд, ведь для безопасности для всего должны быть свои пользователи верно?
ddd=$(su - postgres -c "psql -c \"CREATE USER \\\"$HOST\\\" WITH PASSWORD '$PGUSERPASS';\"" 2>&1)
if [ "$ddd" != "CREATE ROLE" ]
then
    if $PGUSERPASSFLAG
    then
        if $SILENT
        then
            echo "PostgreSQL пользователь $HOST уже существует, неизвестен пароль от этого пользователя"
            exit 0
        fi
        
        PGUSERPASS=$(whiptail --title "Пароль $HOST" --inputbox "Введите пароль от PostgreSQL пользователя \"$HOST\"" 10 60 3>&1 1>&2 2>&3)
        # Обработка кнопки "Отмена"
        exitstatus=$?
        if [[ $exitstatus != 0 ]]
        then
            exit 0;
        fi
    fi
else
    echo "Создан PostgreSQL пользователь \"$HOST\" с паролем \"$PGUSERPASS\""
fi

createDb()
{
    su - postgres -c "psql -c \"CREATE DATABASE \\\"$HOST\\\" OWNER \\\"$HOST\\\";\""
    echo "БД $HOST создана"
}

# проверяем существование БД
if (su - postgres -c "psql -l -At" | grep "^$HOST|" > /dev/null)
then
    if ($SILENT || whiptail --title "PostgreSQL" --yesno "Обнаружена БД скорее всего она осталась от предыдущей установки, удалить?" 11 60)
    then
        su - postgres -c "dropdb \"$HOST\""
        createDb
    else
        echo "БД уже есть, возможно вы хотели запустить обновление а не установку?"
        exit 0;
    fi
else
    createDb
fi

if ! $SILENT
then
    PORT=$(whiptail --title "Kestrel port" --inputbox "Внутренний порт используемый сервером Kestrel до проксирования через Nginx, стоит задавать только если стандартный порт 5050 занят или вы по каким либо причинам не хотите использовать этот порт" 10 60 $PORT 3>&1 1>&2 2>&3)
    # Обработка кнопки "Отмена"
    exitstatus=$?
    if [[ $exitstatus != 0 ]]
    then
        exit 0;
    fi
fi

grep "$USER:" /etc/passwd >/dev/null
if [ $? != 0 ]
then
    useradd $USER --home-dir "$DIRECTORY" --create-home
    echo "Пользователь $USER создан"
fi

DIR=$(echo "$DIRECTORY/SunEngine.Build" | tr -s '/')

# качаем файлы SunEngine
su - $USER -c "git clone \"https://github.com/sunengine/SunEngine.Build\" \"$DIR\" > /dev/null"
exitstatus=$?
if [ $exitstatus != 0 ]
then
    echo "Не удалось скачать файлы движка"
    exit 0;
fi

# DataBaseConnection.json
su - $USER -c "sed -i \"s/<DataBaseName>/$HOST/g\" \"$DIR/Config.server.template/DataBaseConnection.json\""
su - $USER -c "sed -i \"s/<DataBaseUser>/$HOST/g\" \"$DIR/Config.server.template/DataBaseConnection.json\""
su - $USER -c "sed -i \"s/<DataBasePassword>/$PGUSERPASS/g\" \"$DIR/Config.server.template/DataBaseConnection.json\""

# SunEngine.json
su - $USER -c "sed -i \"s/<domain>/$HOST/g\" \"$DIR/Config.server.template/SunEngine.json\""
su - $USER -c "sed -i \"s/<port>/$PORT/g\" \"$DIR/Config.server.template/SunEngine.json\""
su - $USER -c "sed -i \"s!auto!$DIR!g\" \"$DIR/Config.server.template/SunEngine.json\""

ADMINUSERNAME="admin"
ADMINPASSWORD="nimda"
ADMINEMAIL="admin@email"

# DataBaseConnection.json
su - $USER -c "sed -i \"s/<admin-email>/$ADMINEMAIL/g\" \"$DIR/Config.server.template/Init/Users.json\""
su - $USER -c "sed -i \"s/<admin-user-name>/$ADMINUSERNAME/g\" \"$DIR/Config.server.template/Init/Users.json\""
su - $USER -c "sed -i \"s/<admin-password>/$ADMINPASSWORD/g\" \"$DIR/Config.server.template/Init/Users.json\""

echo -e "Создан пользователь администратор\nимя пользователя: $ADMINUSERNAME\nпароль: $ADMINPASSWORD\nemail: $ADMINEMAIL"

# index-page.json
su - $USER -c "sed -i \"s/<admin-user-name>/$ADMINUSERNAME/g\" \"$DIR/Config.server.template/Init/Materials/index-page.json\""

# копируем настройки с темплейта
su - $USER -c "cp -r \"$DIR/Config.server.template\" \"$DIR/Config\""

# systemd
echo "настраиваю systemd демон $HOST.service"
su - $USER -c "sed -i \"s/<host>/$HOST/g\" \"$DIR/Resources/systemd.template\""
su - $USER -c "sed -i \"s/<dir>/$DIR/g\" \"$DIR/Resources/systemd.template\""
su - $USER -c "sed -i \"s/<user>/$USER/g\" \"$DIR/Resources/systemd.template\""
cp "/etc/systemd/system/$HOST.service" "$DIR/Resources/systemd.template"

# добавляем сервис в автозагрузку
systemctl enable $HOST
# запускаем сервис 
systemctl start $HOST

echo "ставим вебсервер nginx"
$SILENTINSTALL apt-get -y install nginx

# nginx
su - $USER -c "sed -i \"s/<host>/$HOST/g\" \"$DIR/Resources/nginx.template\""
su - $USER -c "sed -i \"s!<wwwroot>!$DIR/wwwroot!g\" \"$DIR/Resources/nginx.template\""
su - $USER -c "sed -i \"s/<port>/$PORT/g\" \"$DIR/Resources/nginx.template\""
# настраиваем проксирование сайта через nginx
cp "/etc/nginx/sites-available/$HOST" "$DIR/Resources/nginx.template"
# включаем сайт?
ln -s "/etc/nginx/sites-enabled/$HOST" "/etc/nginx/sites-available/$HOST"

# Заполняем БД данными
su - $USER -c "dotnet \"$DIR/Server/SunEngine.dll\" config:\"$DIR/Config\" init migrate"

echo "настраиваем сертификат для https"
$SILENTINSTALL apt-get -y install certbot

certbot certonly --webroot -w "$DIR" -d $HOST

nginx -s reload
