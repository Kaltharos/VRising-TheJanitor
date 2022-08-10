# Template
### Server Only Mod
Server only mod for automatically removing all dropped items.
Relics, and death containers are excluded.

## Installation
Copy & paste the `Mods.dll` to `\Server\BepInEx\plugins\` folder.

## Removal
Delete the `TheJanitor.dll` from your plugins folder.

## No Wetstone Version
1. Comment `<!--<DefineConstants>WETSTONE</DefineConstants>-->` in `TheJanitor.csproj`
2. Rebuild the dll.

## Config
<details>
<summary>Config</summary>

- `Enable Chat Listen` [default `true`]\
Enable hooking into chat to listen to chat messages.
- `Chat Command` [default `~cleanallnow`]\
Clean all dropped items on the server.
- `Enable Auto Cleaner` [default `true`]\
Enable the auto cleaner.\
Does not included an already existing dropped items.\
Relics & death bags are also excluded.
- `Auto Clean Timer` [default `600`]\
Timer in seconds to wait before the dropped item is deleted automatically.

</details>

## Commands
All commands are admin only!

<details>
<summary>cleanallnow</summary>

`~cleanallnow`\
Clean all dropped items on the server.

</details>

## More Information
<details>
<summary>Changelog</summary>

`1.0.0`
- Initial Release

</details>

<details>
<summary>Known Issues</summary>

### General
- No known issue.

</details>