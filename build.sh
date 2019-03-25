GREEN='\033[0;32m'
NC='\033[0m' # No Color

parent_path=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

cd "$parent_path"


echo -e "$GREEN Clearing Build $NC"
rm -r "$parent_path/Build"
mkdir "$parent_path/Build"
 

echo -e "\n$GREEN Publishing SunEngine $NC"
dotnet publish -c Release SunEngine -o "$parent_path/Build" -v m


echo -e "\n$GREEN Clearing Images $NC"
rm -r "$parent_path/Build/wwwroot/UploadImages/*/*"


echo -e "$GREEN Building Client $NC"
cd "$parent_path/Client"
quasar build


echo  -e "$GREEN Copying Client $NC"
cp -r "$parent_path/Client/dist/spa/." "$parent_path/Build/wwwroot"
