#!/bin/bash
#  BuildProject Script for SunEngine

GREEN='\033[0;32m'
NC='\033[0m' # No Color

parent_path=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

cd "$parent_path"


echo -e "\n${GREEN} Clearing Build${NC}\n"
rm -r "$parent_path/Build"
mkdir "$parent_path/Build"
 

echo -e "\n${GREEN}Publishing SunEngine${NC}\n"
dotnet publish -c Release SunEngine -o "$parent_path/Build" -v m


echo -e "\n${GREEN}Clearing Images${NC}\n"
rm -r "$parent_path/Build/wwwroot/UploadImages/"*/

echo -e "\n${GREEN}Building Client${NC}\n"
cd "$parent_path/Client"
quasar build

echo -e "\n${GREEN}Removing /Client/dist/spa/config.js ${NC}\n"
rm -r "$parent_path/Client/dist/spa/config.js"

echo  -e "\n${GREEN}Copying Client${NC}\n"
cp -r "$parent_path/Client/dist/spa/." "$parent_path/Build/wwwroot"
