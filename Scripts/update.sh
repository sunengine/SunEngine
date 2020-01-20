#!/bin/bash

#   ***************************************
#   *                                     *
#   *    Update Script for SunEngine      *
#   *        Script version: 2.0          *
#   *                                     *
#   ***************************************


# Fonts
RED='\e[0;31m'
GREEN='\e[0;32m'
BLUE='\e[0;34m'
NC='\e[0m'
BOLD='\e[1m'


#Include variables
if [ -z "$1" ]; then
    source UPDATE 
else
    echo -e "\n${GREEN}Variables source \"$1\" ${NC}"
    source $1
fi


# Set folders paths
unameOut="$(uname -s)"
case "${unameOut}" in
    Linux*)     machine=Linux;;
    Darwin*)    machine=Mac;;
    CYGWIN*)    machine=Cygwin;;
    MINGW*)     machine=MinGw;;
    *)          machine="UNKNOWN:${unameOut}"
esac

CURRENT_PATH=$PWD

if [ $PROJECT_ROOT == "auto" ]; then
  {
  
  checkRoot()
  {
    ! [ -f $PWD/.SunEngineRoot ]
  }


  if checkRoot; then
    cd ..
  fi 

  if checkRoot; then
    echo -e "\n${RED} Project root directory not found (detecting .SunEngineRoot file)."
    exit 1
  fi

  if [ $machine == "Cygwin" ]; then
    PROJECT_ROOT="$(cygpath -w $PWD)"
    CURRENT_PATH="$(cygpath -w $CURRENT_PATH)"
  else
    PROJECT_ROOT="${PWD}"
  fi
  }
fi

echo -e "\n${GREEN}Updating \"${PROJECT_NAME}\" project. ${NC}\n"


cd ${PROJECT_ROOT}

if  ! git pull origin master; then
    exit 1
fi

cd $CURRENT_PATH

if  ! bash ${BUILD_SCRIPT_PATH}; then
    exit 1
fi

if  ! bash ${PUBLISH_SCRIPT_PATH}; then
    exit 1
fi