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
#   2. Build client part and copies to wwwroot folder in build folder



#   ******* VARIABLES **************************


# Project name. For info only.
PROJECT_NAME="Sun Engine"    # replace with you project name

# Path to SunEngine solution directory
SOLUTION_PATH=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )    # Now set to current path

# Path to build folder
BUILD_PATH="$SOLUTION_PATH/build"

# Path to client folder
CLIENT_PATH="$SOLUTION_PATH/Client"



#   ************************************************





# Define console colors
GREEN='\033[0;32m'
NC='\033[0m' # No Color


echo -e "\n${GREEN}Publishing to directory ${PROJECT_NAME} project$ ${NC}\n"


cd "$SOLUTION_PATH"


echo -e "\n${GREEN}Clearing build ${NC}\n"
rm -r "$BUILD_PATH"
mkdir "$BUILD_PATH"
 

echo -e "\n${GREEN}Publish build ${PROJECT_NAME} ${NC}\n"
dotnet publish -c Release "$SOLUTION_PATH/SunEngine.Cli" -o "$BUILD_PATH" -v m


echo -e "\n${GREEN}Building Client ${NC}\n"
cd "$CLIENT_PATH"
quasar build


echo  -e "\n${GREEN}Copying Client to wwwroot directory ${NC}\n"
cp -r "$CLIENT_PATH/dist/spa/." "$BUILD_PATH/wwwroot"
