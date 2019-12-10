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

#Include global variables
source SUNENGINE

# Path to SunEngine solution directory
SOLUTION_PATH=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )    # Now set to current path

# Path to client folder
CLIENT_PATH="$SOLUTION_PATH/Client"

#   ************************************************


echo -e "\n${GREEN}Publishing to directory ${PROJECT_NAME} project$ ${NC}\n"


cd "$SOLUTION_PATH"

echo -e "Building server" 
if [ dotnet > /dev/null ]; then
    echo -e "\n${GREEN}Publish build ${PROJECT_NAME} ${NC}\n"
    dotnet publish --configuration Release "$SOLUTION_PATH/SunEngine.Cli" --output "$OUTPUT/backend"
else
    echo -e "\n${RED} .NET Core not install."
    exit 1
fi


echo -e "\n${GREEN}Building Client ${NC}\n"
cd "$CLIENT_PATH"

#check on available quasar
if [ ! quasar > /dev/null ]; then
    echo -e "\n${RED} Quasar not install please install by command\n${BLUE} npm install -g @quasar/cli "
    exit 1
fi

npm install
cp -r $PWD/src/site.template $PWD/src/site
quasar build






# echo  -e "\n${GREEN}Copying Client to wwwroot directory ${NC}\n"
# cp -r "$CLIENT_PATH/dist/spa/." "$BUILD_PATH/wwwroot"


# echo -e "\n${GREEN}Install node_modules ${NC}\n if it no there ${NC}\n"

# if [ ! -d SunEngine/Client/node_modules ]; then
# cd SunEngine/Client && npm install &
# fi


# echo -e "\n${GREEN}Copy "site-template" to "site" ${NC}\n"

# cp -r SunEngine/Client/src/site-template SunEngine/Client/src/site
