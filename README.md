# ONITwitch Mod adaptation for Lavriko

Contains donation alerts support

# Building

I used .NET 8 SDK with .NET Framework 4.7.1 SDK installed.

You can check if `C://Windows/Microsoft.NET/Framework64/v4.0.30319` exists - then you're good to go.

Project opens and builds in Rider 2023.1.2

There is a `custom_build.sh` script, that contains all logic about building this repository.

No damn MSBuild tasks and fancy build variables.

All you have to do is set a `LibFolder` in `Directory.Build.props` to a folder with your game.

Do the same for `custom_build.sh`