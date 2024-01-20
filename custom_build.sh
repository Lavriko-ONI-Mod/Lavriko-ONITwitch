if [ -f "ONIMods/build/PLib.dll" ]; then
    echo "PLib is already built. Skipping build."
    echo ""
else
    echo 'PLib is not built. Building now.'
    echo ""
    cd "ONIMods"
    
    # we need echo 1 so that close confirmation is automatically accepted
    echo 1 | bash "custom_build.sh"
    
    # shellcheck disable=SC2181
    if [ $? -ne 0 ]; then
        echo ""
        echo "!Building PLib failed!"
        echo ""
        echo "Press any key to exit"
        read -n 1 -s
        exit 1
    fi
    cd ../
    echo ""
    echo 'PLib is now built. Proceeding...'
    echo ""
fi

echo "Building Lavriko Twitch"

dotnet restore

# shellcheck disable=SC2181
if [ $? -ne 0 ]; then
    echo ""
    echo "!dotnet restore failed!"
    echo ""
    echo "Press any key to exit"
    read -n 1 -s
    exit 1
fi

dotnet build --configuration Release

# shellcheck disable=SC2181
if [ $? -ne 0 ]; then
    echo ""
    echo "!dotnet build finished with error!"
    echo ""
    echo "Press any key to exit"
    read -n 1 -s
    exit 1
fi

if [ ! -d "build" ]; then
  mkdir "build"
else
  echo "build dir is already present"
  echo "Clearing build directory"
  rm -R build/*
  echo "build directory is empty"
fi


echo "copying mod contents"
cp ONITwitchCore/bin/Release/net471/* build/
cp ../ONIMods/build/* build/

echo "copying mod.yaml and mod_info.yaml"
cp ONITwitchCore/mod.yaml build/
cp ONITwitchCore/mod_info.yaml build/

echo "copying anim, assets, elements, templates, translations"
cp -r ONITwitchCore/anim build/
cp -r ONITwitchCore/assets build/
cp -r ONITwitchCore/elements build/
cp -r ONITwitchCore/templates build/
cp -r ONITwitchCore/translations build/

echo "Build Done!"

echo "ILRepacking now"

IL_REPACK_VERSION='2.0.27'

IL_REPACK_PATH="$USERPROFILE\.nuget\packages\ilrepack\\$IL_REPACK_VERSION\tools\ILRepack.exe"

# For ILRepack
# WHY: to get a single dll with all required references and not spill our mod with extra files, and not to cause any conflicts with other mods 
# /out is destination file
# /lib is referenced assemblies (add game dlls here, so our mod can build)
# all other parameters are dlls that we actually merge 

"$IL_REPACK_PATH" \
  /out:build/ONITwitch.dll \
  /lib:"C:\Users\Admin\Downloads\ONI 17.11.23\OxygenNotIncluded_Data\Managed" \
  build/ONITwitch.dll build/ONITwitchLib.dll build/PLib.dll

# shellcheck disable=SC2181
if [ $? -ne 0 ]; then
    echo ""
    echo "!ILRepack finished with error!"
    echo ""
    echo "Press any key to exit"
    read -n 1 -s
    exit 1
else
    echo ""
    echo "ILRepack SUCCEEDED"
    echo ""
fi

echo "Removing source assemblies"

find build -type f -name '*.dll' ! -name 'ONITwitch.dll' -exec rm {} \;
find build -type f -name '*.pdb' ! -name 'ONITwitch.pdb' -exec rm {} \;
find build -type f -name '*.xml' ! -name 'ONITwitch.xml' -exec rm {} \;

echo ""
echo "Build SUCCEEDED!"
echo ""

MODS_DIR="$USERPROFILE\Documents\Klei\OxygenNotIncluded\mods\dev"

echo "Copying mod to mods/dev"

if [ ! -d "$MODS_DIR/lavriko_twitch" ]; then
  mkdir "$MODS_DIR/lavriko_twitch"
else
  echo "mods/dev/lavriko_twitch dir is already present"
  echo "Clearing mods/dev/lavriko_twitch directory"
  rm -rf "$MODS_DIR/lavriko_twitch"
  mkdir "$MODS_DIR/lavriko_twitch"
fi

cp -r build/* $MODS_DIR/lavriko_twitch

echo "Copying done!"

echo "Press any key to exit"
read -n 1 -s