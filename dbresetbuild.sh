GREEN='\033[0;32m'
NC='\033[0m' # No Color

parent_path=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

cd "$parent_path"


echo -e "$GREEN Clearing BuildDbReset $NC"
rm -rf "$parent_path/BuildDbReset"
mkdir "$parent_path/BuildDbReset"
mkdir "$parent_path/BuildDbReset/Migrations"
mkdir "$parent_path/BuildDbReset/DataSeedDev"

echo -e "\n$GREEN Publishing Migrations $NC"
dotnet publish -c Release Migrations -o "$parent_path/BuildDbReset/Migrations" -v m


echo -e "\n$GREEN Publishing DataSeedDev $NC"
dotnet publish -c Release DataSeedDev -o "$parent_path/BuildDbReset/DataSeedDev" -v m
