#!/bin/bash

#   *****************************************************
#   *                                                   *
#   *   Publish to Ubuntu Server script for SunEngine   *
#   *              Script version: 2.1                  *
#   *                                                   *
#   *****************************************************


# Fonts
RED='\e[0;31m'
GREEN='\e[0;32m'
BLUE='\e[0;34m'
NC='\e[0m'
BOLD='\e[1m'


#Include variables
if [ -z "$1" ]; then
    source PUBLISH
else
    echo -e "\n${GREEN}Variables source \"$1\" ${NC}"
    source $1
fi

# Set folders paths
unameOut="$(uname -s)"
case "${unameOut}" in
    Linux*)     machine=Linux;;
    Darwin*)    machine=Mac;;
    CYGWIN*)    machine=Cygwin;;
    MINGW*)     machine=MinGw;;
    *)          machine="UNKNOWN:${unameOut}"
esac

if [  $machine == "Cygwin" ]; then
   LOCAL_BUILD_PATH="$(cygpath -u $LOCAL_BUILD_PATH)"
fi


echo -e "${GREEN}Using variables ${NC}\n"
echo -e "${GREEN}LOCAL_BUILD_PATH = ${LOCAL_BUILD_PATH} ${NC}"
echo -e "${GREEN}REMOTE_USER = ${REMOTE_USER} ${NC}"
echo -e "${GREEN}REMOTE_HOST = ${REMOTE_HOST} ${NC}"
echo -e "${GREEN}REMOTE_DIRECTORY = ${REMOTE_DIRECTORY} ${NC}"
echo -e "${GREEN}REMOTE_DIRECTORY_OWNER = ${REMOTE_DIRECTORY_OWNER} ${NC}"
echo -e "${GREEN}REMOTE_DIRECTORY_GROUP = ${REMOTE_DIRECTORY_GROUP} ${NC}"
echo -e "${GREEN}REMOTE_SYSTEMD_SERVICE_NAME = ${REMOTE_SYSTEMD_SERVICE_NAME} ${NC}"


echo  -e "\n${GREEN}Syncing build ${NC}\n"

rsync -arvzhe ssh --progress --stats  --exclude 'Config'  --chown=$REMOTE_DIRECTORY_OWNER:$REMOTE_DIRECTORY_GROUP  $LOCAL_BUILD_PATH/. -a $REMOTE_USER@$REMOTE_HOST:$REMOTE_DIRECTORY

echo  -e "\n${GREEN}Syncing Config ignore-existing ${NC}\n"

rsync -arvzhe ssh --progress --stats --ignore-existing   --chown=$REMOTE_DIRECTORY_OWNER:$REMOTE_DIRECTORY_GROUP  $LOCAL_BUILD_PATH/Config/. -a $REMOTE_USER@$REMOTE_HOST:$REMOTE_DIRECTORY/Config

echo  -e "\n${GREEN}Restarting systemd service and reload nginx ${NC}\n"

ssh ${REMOTE_USER}@${REMOTE_HOST} << EOF

 cd ${REMOTE_DIRECTORY}/Server
 if  dotnet SunEngine.dll test-db-con; then
 {
    echo  -e "\n${GREEN}Migrating to new version ${NC}\n"
    dotnet SunEngine.dll migrate
 }
 fi


 echo  -e "\n${GREEN}Reloading nginx ${NC}\n"
 systemctl --output=verbose reload nginx

 echo  -e "\n${GREEN}Restarting ${REMOTE_SYSTEMD_SERVICE_NAME} ${NC}\n"
 systemctl --output=verbose restart ${REMOTE_SYSTEMD_SERVICE_NAME}

 echo  -e "\n${GREEN}Sleeping 8 seconds ${NC}\n"
 sleep 8

 echo  -e "\n${GREEN}nginx journal: ${NC}\n"
 journalctl -u nginx --lines=10 --no-pager

 echo  -e "\n${GREEN}${REMOTE_SYSTEMD_SERVICE_NAME} journal: ${NC}\n"
 journalctl -u ${REMOTE_SYSTEMD_SERVICE_NAME} --lines=20 --no-pager
EOF


echo  -e "\n${GREEN}All done! ${NC}\n"
