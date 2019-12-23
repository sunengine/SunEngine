#!/usr/bin/env bash

prev=$1
new=$2

sed -i "s/<Version>$prev</<Version>$new</g" ../Server/SunEngine.Cli/SunEngine.Cli.csproj

sed -i "s/<Version>$prev</<Version>$new</g" ../Server/SunEngine.Core/SunEngine.Core.csproj

sed -i "s/\"version\": \"$prev\"/\"version\": \"$new\"/g" ../Client/package.json

sed -i "s/label=Version\&message=$prev\&/label=Version\&message=$new\&/g" ../README.md

sed -i "s/label=Version\&message=$prev\&/label=Version\&message=$new\&/g" ../README.RU.md





