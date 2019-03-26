#!/bin/bash
#  BuildProject Script for SunEngine

GREEN='\033[0;32m'
NC='\033[0m' # No Color

parent_path=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

cd "$parent_path"


echo -e "\n${GREEN} Clearing Build${NC}\n"
rm -r "$parent_path/build"
mkdir "$parent_path/build"
 

echo -e "\n${GREEN}Publishing SunEngine${NC}\n"
cd "../SunEngine/SunEngine"
dotnet publish -c Release SunEngine -o "$parent_path/build" -v m


echo -e "\n${GREEN}Clearing Images${NC}\n"
rm -r "$parent_path/build/wwwroot/UploadImages/"*/


echo -e "\n${GREEN}Building Client${NC}\n"
cd "$parent_path"
quasar build


#echo -e "\n${GREEN}Removing /Client/dist/spa/config.js${NC}\n"
#rm -r "$parent_path/Client/dist/spa/config.js"


echo  -e "\n${GREEN}Copying Client${NC}\n"
cp -r "$parent_path/Client/dist/spa/." "$parent_path/build/wwwroot"
