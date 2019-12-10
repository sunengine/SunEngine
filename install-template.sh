#!/bin/bash

#
#
#
#
#

if [apt --help > /dev/null]
then
    if [dotnet > /dev/null]
    then

    else
    {
        installDotnet()
    }
    fi
else
echo "This OS not unaivalablek"
fi

installDotnet(){
    wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg
    sudo mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
    wget -q https://packages.microsoft.com/config/debian/10/prod.list
    sudo mv prod.list /etc/apt/sources.list.d/microsoft-prod.list
    sudo chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg
    sudo chown root:root /etc/apt/sources.list.d/microsoft-prod.list
    sudo apt-get update
    sudo apt-get install -y apt-transport-https
    sudo apt-get update
    sudo apt-get install -y aspnetcore-runtime-3.0

}

installNpm(){
    sudo apt install -y npm nginx
}

installQuasar(){
    sudo npm install -g @quasar/cli
}

installMariadb{
    sudo apt install mariadb-server
}
