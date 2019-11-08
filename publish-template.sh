#!/bin/bash

#   *****************************************************
#   *                                                   *
#   *   Publish to Ubuntu Server script for SunEngine   *
#   *               Script version: 3                   *
#   *                                                   *
#   *****************************************************


#   Note:  Build does not contain any config files
#   No server Config directory
#   No Client/config.js
#   You need to upload it manually to the server
#   This can be done one time on first deploy
#   You need to edit them only if settings changes


#   How it works:
#   1. Copy build folder to remote Ubuntu server using ssh scp
#   2. Publish server to build folder
#   3. Make remote folder owner to "www-data"
#   4. Restart systemctl site service



#   ******* VARIABLES ***********************************


# Project name. For info only.
PROJECT_NAME="Sun Engine"    # replace with you project name

# Path to build directory
LOCAL_BUILD_PATH="$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )/build"   # Now set to "current_path/build"

# IP or domain name of remote host
HOST="111.111.111.111"

# Remote directory path
REMOTE_DIRECTORY_PATH="/sites/my-site/"

# Remote user to connect and copy
REMOTE_USER="root"

# Remote user owns REMOTE_DIRECTORY_PATH, usually "www-data"
REMOTE_DIRECTORY_OWNER="www-data"

# systemctl service name to restart after upload
SYSTEMCTL_SERVICE_NAME="mysite_systemctl_service_name"



#   ******************************************************





# Definition of console colors
GREEN='\033[0;32m'
NC='\033[0m' # No Color

# Open ssh store session.  
# This need to enter ssh password or key phrase once.
ssh-agent
ssh-add

echo -e "\n${GREEN}Publishing ${PROJECT_NAME} to ${HOST} server${NC}\n"

# Copy build folder to server
scp -r -C  "${LOCAL_BUILD_PATH}"/* "${REMOTE_USER}@${HOST}:${REMOTE_DIRECTORY_PATH}"


echo -e "\n${GREEN}Resetting owner of ${REMOTE_DIRECTORY_PATH} to www-data and restarting ${PROJECT_NAME} ${NC}\n" 


# Make remote folder owner to "www-data".
# Restart systemctl site service
ssh ${REMOTE_USER}@${HOST} << EOF
  chown ${REMOTE_DIRECTORY_OWNER}:${REMOTE_DIRECTORY_OWNER} -R ${REMOTE_DIRECTORY_PATH}
  systemctl restart ${SYSTEMCTL_SERVICE_NAME}
EOF

# Close ssh password store session
ssh-add -D
