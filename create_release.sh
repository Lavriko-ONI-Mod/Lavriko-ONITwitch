#!/bin/bash
if [ $# -lt 1 ]
then
    echo "Enter the version of the release"
    exit 1
fi

VERSION="$1"
TARGET_DIR="releases/lavriko_twitch$VERSION"
MODS_DIR="C:\Users\Admin\Documents\Klei\OxygenNotIncluded\mods\dev"

#if [ -d $TARGET_DIR ]
#then
#    echo "Directory $TARGET_DIR already exists"
#    exit 1
#fi

mkdir -p $TARGET_DIR

cp -r build/ONITwitch/* $TARGET_DIR
cp -r build/ONITwitchLib/* $TARGET_DIR

echo "Release $VERSION prepared"

cp -r $TARGET_DIR $MODS_DIR
