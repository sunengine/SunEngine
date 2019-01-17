GREEN='\033[0;32m'
NC='\033[0m' # No Color

parent_path=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

cd "$parent_path"


echo -e "$GREEN Clearing Build $NC"
cd Build
rm -rf "*" 

cd "$parent_path"
#echo -e "$GREEN Publishing SunEngine $NC"
#dotnet publish SunEngine -o "$parent_path/Build"


#echo -e "$GREEN Building Client $NC"
#cd Client
#quasar build

#echo  -e "$GREEN Copying Client $NC"
#cp -rf "$parent_path/Client/dist/spa-mat/." "$parent_path/Build/wwwroot"