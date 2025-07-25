![alt text](TheEscapists.png)
![alt text](Reincarcerated.png)

# The Escapists Reincarcerated Repository

This is the repository for The Escapists Reincarcerated.

This is only officially supported for Windows systems. Linux work-arounds can be found in the Discord (linked below).

Requirements for CTFAK to work properly: 
.NET 6.0 Desktop Runtime, .NET 6.0 Runtime, and ASP.NET Core Runtime 6.0 : https://dotnet.microsoft.com/en-us/download/dotnet/6.0

The CTFAK 2.0 GitHub page can be found here: https://github.com/CTFAK/CTFAK2.0 (you do NOT have to download this)

Discord: https://discord.gg/PEzv5ECNgR

For fully built and "stable" releases, go to the "Releases" section.

TER uses Unity 6000.0.41f1 (Note: Older versions of Unity were used in much older versions of the source. The current version has been used since May 6, 2025 and is not expected to change.)

The source code presented in the ```main``` branch is stuff pushed daily, and will not be the same as what is seen in the released versions most of the time.

## Changing your The Escapists folder directory

The game will not get past the loading screen if you do not have a valid path for The Escapists.

In the root of the The Escapists Reincarcerated directory, go to this folder: "The Escapists Reincarcerated_Data/StreamingAssets/CTFAK".

In this folder, go into the "config.ini" file to change the directory of your The Escapists folder path at "GameFolderPath" under "Settings" in that file. 

YOU MUST USE FORWARD SLASHES (/) INSTEAD OF BACK SLASHES (\\), AND YOU ALSO MUST PUT QUOTES AROUND THE PATH.

Example path: "C:/Program Files (x86)/Steam/steamapps/common/The Escapists"
