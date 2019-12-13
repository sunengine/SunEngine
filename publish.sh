#!/bin/bash

#   *****************************************************
#   *                                                   *
#   *   Publish to Ubuntu Server script for SunEngine   *
#   *               Script version: 4                   *
#   *                                                   *
#   *****************************************************



#Include variables
source PUBLISH


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

echo rsync -arvzhe ssh --progress --stats  --exclude 'Config'  --chown=$REMOTE_DIRECTORY_OWNER:$REMOTE_DIRECTORY_GROUP  $LOCAL_BUILD_PATH/. -a $REMOTE_USER@$REMOTE_HOST:$REMOTE_DIRECTORY

rsync -arvzhe ssh --progress --stats  --exclude 'Config'  --chown=$REMOTE_DIRECTORY_OWNER:$REMOTE_DIRECTORY_GROUP  $LOCAL_BUILD_PATH/. -a $REMOTE_USER@$REMOTE_HOST:$REMOTE_DIRECTORY

echo  -e "\n${GREEN}Syncing Config ignore-existing ${NC}\n"

rsync -arvzhe ssh --progress --stats --ignore-existing   --chown=$REMOTE_DIRECTORY_OWNER:$REMOTE_DIRECTORY_GROUP  $LOCAL_BUILD_PATH/Config/. -a $REMOTE_USER@$REMOTE_HOST:$REMOTE_DIRECTORY/Config


echo  -e "\n${GREEN}Restarting systemd service and reload nginx ${NC}\n"

ssh ${REMOTE_USER}@${REMOTE_HOST} << EOF
 echo  -e "\n${GREEN}Reloading nginx ${NC}\n"
 systemctl --output=verbose reload nginx

 echo  -e "\n${GREEN}Restarting ${REMOTE_SYSTEMD_SERVICE_NAME} ${NC}\n"
 systemctl --output=verbose restart ${REMOTE_SYSTEMD_SERVICE_NAME}

 sleep 5;

 echo  -e "\n${GREEN}nginx journal: ${NC}\n"
 journalctl -u nginx --lines=10 --no-pager

 echo  -e "\n${GREEN}${REMOTE_SYSTEMD_SERVICE_NAME} journal: ${NC}\n"
 journalctl -u ${REMOTE_SYSTEMD_SERVICE_NAME} --lines=15 --no-pager
EOF


echo  -e "\n${GREEN}All done! ${NC}\n"
