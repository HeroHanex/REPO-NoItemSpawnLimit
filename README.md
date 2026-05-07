# NoItemSpawnLimit

[![GitHub](https://img.shields.io/badge/GitHub-NoItemSpawnLimit-brightgreen?style=for-the-badge&logo=GitHub)](https://github.com/HeroHanex/REPO-NoItemSpawnLimit/)
[![Thunderstore Version](https://img.shields.io/thunderstore/v/HeroHanex/NoItemSpawnLimit?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/repo/p/HeroHanex/NoItemSpawnLimit)
[![Thunderstore Downloads](https://img.shields.io/thunderstore/dt/HeroHanex/NoItemSpawnLimit?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/repo/p/HeroHanex/NoItemSpawnLimit)

Using the [MoreShopItems Updated](https://thunderstore.io/c/repo/p/Jettcodey/MoreShopItems_Updated) mod or other mods that add lots of items, but frustrated by the game's truck item spawn limits?  
**NoItemSpawnLimit** is a host only mod for REPO that removes or customizes truck item spawn limits for all items—including those added by other mods.

## What this mod changes

This mod modifies the truck item spawning system in REPO.

By default, the game limits how many of each item type can spawn in the truck.

This mod overrides that behavior by:
- Increasing or removing per-item spawn limits
- Allowing excess purchased items to spawn at fallback spawn points
- Preserving vanilla item placement logic when possible

This is a **host-only** mod. Clients do not need to install it.

## Spawn Limit Modes

### Global Mode (`RemoveSpawnLimit = true`)
- Ignores all per-item limits
- All items use a shared maximum spawn limit
- Default value: `10000`

### Per-Item Mode (`RemoveSpawnLimit = false`)
- Each item uses its own configurable limit
- Editable via config file or [REPOConfig](https://thunderstore.io/c/repo/p/nickklmao/REPOConfig/) UI

##### **Warning: Buying massive amounts of items can freeze the game.**

## Installation

1. Ensure you have BepInEx installed:
    - Download [BepInEx 5](https://github.com/BepInEx/BepInEx/releases)
    - Extract the zip file inside of your game's root folder
2. Download the mod and extract the contents of the zip into your game’s `BepInEx/plugins` folder. The `NoItemSpawnLimit.dll` should be placed inside `BepInEx/plugins/NoItemSpawnLimit`
3. Launch the game

## Configuration

###### If you have the [REPOConfig](https://thunderstore.io/c/repo/p/nickklmao/REPOConfig/) mod. You can easily edit the configuration inside of the "MODS" in the main menu and skip the steps below.
- Launch the game at least once to generate the config file
- Edit the generated config file at `BepInEx/config/HeroHanex.NoItemSpawnLimit.cfg`
- Each item has its own configurable max spawn amount
- Set `RemoveSpawnLimit` to `true` to remove all limits, or `false` to use per-item settings

## Compatibility

- Works with both singleplayer and multiplayer
- Compatible with most other mods, including those that add new items
- Host-only mod (clients do not need it installed)

## Known Limitations

- Extremely high item counts may cause performance issues or freezes
- Some modded items may not behave correctly if they override spawn logic
- Spawn behavior depends on host in multiplayer

## Support

- [GitHub Issues](https://github.com/HeroHanex/REPO-NoItemSpawnLimit/issues) for bug reports and suggestions

## Developer Contact

Need help or want to discuss the mod?  
- Join the [R.E.P.O. Modding Discord](https://discord.com/invite/vPJtKhYAFe)  
- Post in the [`#released-mods`](https://discord.com/channels/1344557689979670578/1381300604365963419) forum thread for [NoItemSpawnLimit](https://discord.com/channels/1344557689979670578/1381300604365963419)

---

Enjoy buying as many items as you want!

<a href='https://ko-fi.com/B0B41G63RL' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://storage.ko-fi.com/cdn/kofi6.png?v=6' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>
