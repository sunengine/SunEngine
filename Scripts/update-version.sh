#!/usr/bin/env bash

prev=$1
new=$2

sed -i "s/<Version>$prev</<Version>$new</g" ../Server/SunEngine.Cli/SunEngine.Cli.csproj

sed -i "s/<Version>$prev</<Version>$new</g" ../Server/SunEngine.Core/SunEngine.Core.csproj

sed -i "s/\"version\": \"$prev\"/\"version\": \"$new\"/g" ../Client/package.json

sed -i "s/label=Version\&message=v$prev\&/label=Version\&message=v$new\&/g" ../README.md

sed -i "s/label=%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F\&message=v$prev\&/label=%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F\&message=v$new\&/g" ../README.RU.md

sed -i "s/>v$prev</>v$new</g" ../Config/Init/Materials/index-page.html

sed -i "s/>v$prev</>v$new</g" ../Config.template/Init/Materials/index-page.html







