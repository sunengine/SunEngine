#!/bin/bash

#   ***************************************
#   *                                     *
#   *  BuildProject Script for SunEngine  *
#   *         Script version: 3           *
#   *                                     *
#   ***************************************


#   Note:  Build does not contain any config files
#   No server Config directory
#   No Client/config.js
#   You need to upload it manually to the server
#   This can be done one time on first deploy
#   You need to edit them only if settings changes


#   How it works:
#   1. Clear build folder
#   1. Publish server to build folder
#   2. Publish client to build folder



#   ******* VARIABLES **************************


# Project name. For info only. Replace with you project name.
PROJECT_NAME="SunEngine"   

# Path to SunEngine root directory. Now set to current path.
SOLUTION_PATH=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )


#   ************************************************







# Path to build folder
BUILD_PATH="$SOLUTION_PATH/build"

# Path to server folder
SERVER_PATH="$SOLUTION_PATH/Server"

# Path to client folder
CLIENT_PATH="$SOLUTION_PATH/Client"


# Define console colors
GREEN='\033[0;32m'
NC='\033[0m' # No Color


echo -e "\n${GREEN}Publishing to directory ${PROJECT_NAME} project$ ${NC}\n"


cd "$SOLUTION_PATH"


echo -e "\n${GREEN}Clearing build ${NC}\n"
rm -r "$BUILD_PATH"
mkdir "$BUILD_PATH"
 

echo -e "\n${GREEN}Publish build ${PROJECT_NAME} ${NC}\n"
dotnet publish -c Release "$SERVER_PATH/SunEngine.Cli" -o "$BUILD_PATH/Server" -v m


echo -e "\n${GREEN}Building Client ${NC}\n"
cd "$CLIENT_PATH"
quasar build


echo  -e "\n${GREEN}Copying Client to wwwroot directory ${NC}\n"
cp -r "$CLIENT_PATH/dist/spa/." "$BUILD_PATH/Client"

echo  -e "\n${GREEN}Clearing dist directory ${NC}\n"
rm -rf "$CLIENT_PATH/dist"
