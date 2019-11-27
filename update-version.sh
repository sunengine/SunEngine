#!/usr/bin/env bash

prev=$1
new=$2

sed -i "s/<Version>$prev</<Version>$new</g" ./Server/SunEngine.Cli/SunEngine.Cli.csproj
sed -i "s/<PackageVersion>$prev</<PackageVersion>$new</g" ./Server/SunEngine.Cli/SunEngine.Cli.csproj

sed -i "s/<Version>$prev</<Version>$new</g" ./Server/SunEngine.Core/SunEngine.Core.csproj
sed -i "s/<PackageVersion>$prev</<PackageVersion>$new</g" ./Server/SunEngine.Core/SunEngine.Core.csproj

sed -i "s/\"version\": \"$prev\"/\"version\": \"$new\"/g" ./Client/package.json

sed -i "s/Version: $prev/Version: $new/g" ./README.md

sed -i "s/Версия: $prev/Версия: $new/g" ./README.RU.md




 
