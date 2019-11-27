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
#   2. Publish server to build folder
#   3. Publish client to build folder in wwwroot
#   4. Publish data (Resources, Skins)



#   ******* VARIABLES **************************


# Project name. For info only. Replace with you project name.
PROJECT_NAME="Demo SunEngine"

# Path to SunEngine root directory.
PROJECT_ROOT="/path/to/project/root"


#   ************************************************







# Path to build folder
BUILD_PATH="$PROJECT_ROOT/build"

# Path to server folder
SERVER_PATH="$PROJECT_ROOT/Server"

# Path to client folder
CLIENT_PATH="$PROJECT_ROOT/Client"


# Define console colors
GREEN='\033[0;32m'
NC='\033[0m' # No Color


echo -e "\n${GREEN}Publishing \"${PROJECT_NAME}\" project. ${NC}\n"


cd "$PROJECT_ROOT"


echo -e "${GREEN}Clearing build ${NC}\n"
rm -r "$BUILD_PATH"
mkdir "$BUILD_PATH"


echo -e "${GREEN}Publish build ${PROJECT_NAME} ${NC}\n"
dotnet publish -c Release "$SERVER_PATH/SunEngine.Cli" -o "$BUILD_PATH/Server" -v m


echo -e "${GREEN}Building Client ${NC}\n"
cd "$CLIENT_PATH"
quasar build


echo  -e "${GREEN}Copying Client to wwwroot directory ${NC}\n"
cp -r "$CLIENT_PATH/dist/spa/." "$BUILD_PATH/wwwroot"

echo  -e "${GREEN}Clearing dist directory ${NC}\n"
rm -rf "$CLIENT_PATH/dist"

echo  -e "${GREEN}Copying Resources to build directory ${NC}\n"
cp -r "$PROJECT_ROOT/Resources/." "$BUILD_PATH/Resources"

echo  -e "${GREEN}Copying Skins to build directory ${NC}\n"
cp -r "$PROJECT_ROOT/Skins/." "$BUILD_PATH/Skins"

echo  -e "${GREEN}Creating UploadImages and CurrentSkin directory ${NC}\n"
mkdir "$BUILD_PATH/wwwroot/UploadImages"
mkdir "$BUILD_PATH/wwwroot/Skin"
