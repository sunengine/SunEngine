#!/bin/bash

# Example script of publishing SunEngine


GREEN='\033[0;32m'
NC='\033[0m' # No Color

ssh-agent
ssh-add

echo -e "${GREEN}Publishing MySite${NC}\n"

scp -r -C  ~/PathToLocalBuildDirectory/SunEngine/build/* remoteUserName@host:remoteDirToMySite
#Example scp -r -C  ~/Projects/SunEngine/build/* user1@123.123.123.123:/site/MySite/

echo -e "${GREEN}Resetting owner of /site/MySite to www-data and restarting MySite${NC}\n" 

ssh remoteUserName@host << EOF
  chown www-data:www-data -R /site/MySite
  systemctl restart MySite
EOF

#                                ---- How it works ----
# ssh remoteUserName@host << EOF                   Connection by ssh to remote server
#  chown www-data:www-data -R /site/MySite         Change owner to www-data
#  systemctl restart MySite                        Restart MySite systemctl service
# EOF

ssh-add -D
