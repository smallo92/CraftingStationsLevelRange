# Crafting Stations Level Based Range
This is a simple mod that increases the range of your crafting stations build ability depending on their levels. You can also set the default value for a level 1 crafting station (vanilla is 20). This also makes the secondary benches (Stonecutter or Artisans Table) to inherit the range of the parent stations (Workbench or Forge)

## Manual Installation
To install this mod, you need to have BepInEx. After installing BepInEx, extract CraftingStationLevelRange.dll into games install **"\Valheim\BepInEx\plugins"**

## Config
Before the config file is generated, you must run the game with the mod installed. The config file is located at **"\Valheim\BepInEx\config\smallo.mods.craftingstationlevelrange.cfg"**

#### There are serveral config options available;
* EnableMod enables or disable the mod  
* DefaultRange is the range you can set for a level 1 crafting station (vanilla value is 20 and so is the default config option)  
* IncreaseAmount is the range at which you want the range to increase with each level (default is 10)  
* SearchRange is the range to search for crafting stations while holding a hammer. Warning: larger numbers may potentially cause lag, only increase the number if your stations will ever get above this build range.
* ParentInheritance is to allow secondary Crafting Stations (Stonecutter or Artisans Table) to inherit the range of their parent stations (Workbench or Forge) if they are within range
* InheritanceAmount is the amount to multiply the inheritance value by. You may want the secondary station to have a lesser value. (Example: 0.5 would be 50% the amount of the IncreaseAmount variable)

If you have any suggestions, feel free to let me know!

## Changelog

#### v1.5.0;
* Changed the event that's used to change the station size, this event doesn't require any specific tools to be equipped.

#### v1.4.1;
* Fixed an issue with getting the closest bench, was basing it on the player position instead of the benches position, this would cause child benches (Stonecutter or Artisans Table) range to break sometimes

#### v1.4.0;
* Fixed an error when you had 2 parent benches (Workbench or Forge) in range it would only upgrade 1 of the 2
* Made it change the size of the net version of the staiton which means when 1 player upgrades the bench size it will do it for all players (MP friendly obviously)

#### v1.3.0;
* Fixed an error if you deleted the work bench while it was trying to apply the new distance
* Made it so child benches (Stonecutter or Artisans Table) go back to the default range when the parent benches (Workbench or Forge) are destroyed
* Code optimisations

#### v1.2.1;
* Only Readme updates, my bad.

#### v1.2;
* Made it so secondary stations (Stonercutter and Artisans Table) inherit the parent stations (Workbench and Forge) range due to these benches only having 1 level
* Added config options for the inheritance

## Images

![Default Range](https://fivem.fail/gta5/Audio/PrepareSynchronizedAudioEventForScene/Sb2r7Gui2W.png)  
![Level 2 Range](https://fivem.fail/gta5/Money/N_0x226c284c830d0ca8/alMAdiz7Gd.png)

[Discord Support](https://discord.gg/pTGSu8R7DW)

## Affiliate Link
[![ZAP-Hosting Gameserver and Webhosting](https://zap-hosting.com/interface/download/images.php?type=affiliate&id=99496)](https://zap-hosting.com/a/f386564816225e9bcd3445ae47b34c8823f72489)