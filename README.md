# Water for ENB Community Shaders conflict patcher

This fork generates an order-aware, ESL-flagged compatibility patch for Water for ENB when it is used with Community Shaders and mods such as Lux.

The original patcher forwarded only part of the water state and began from the winning record. That could either discard Lux lighting when Water for ENB won, or discard Water for ENB fields when Lux won. This fork reconstructs each affected record instead:

- the latest non-Water record supplies the complete CELL or WRLD record;
- Water for ENB and its transitive compatibility-patch family supply only water-owned fields;
- the generated override is uncompressed and the ESP is ESL-flagged;
- records that only involve Water for ENB and the originating master are omitted.

## Field policy

For `CELL`, the patcher forwards `Water`, `WaterEnvironmentMap`, `WaterHeight`, and only the `HasWater` flag bit. For `WRLD`, it forwards `Water`, `LodWater`, `LodWaterHeight`, and `WaterEnvironmentMap`. Every other field remains from the latest non-Water context.

This intentionally prevents a broad "one plugin wins" resolution. Lighting, image spaces, encounter data, ownership, locations, and unrelated flags remain with Lux or the latest relevant non-Water patch.

## Build

The supported build uses .NET 9 and the stable Mutagen/Synthesis packages pinned in the project file.

```powershell
dotnet build .\WaterForENBPatcherCS.sln -c Release
& .\WaterForENBPatcherCS\bin\Release\net9.0\WaterForENBPatcherCS.exe --self-test
```

## Headless run

The executable uses Synthesis Typical Open and accepts its normal command-line inputs:

```text
run-patcher --GameRelease SkyrimSE --DataFolderPath <Data> --LoadOrderFilePath <plugins.txt> --OutputPath "<output>\Ensrick Lux Water CS Patch.esp" --ExtraDataFolder <repository-root>
```

`settings.json` may override the Water for ENB plugin filename, the explicit Water-family list, and the skip list. The explicit family list covers official worldspace patches that intentionally do not master the main Water plugin; transitive master relationships are then discovered automatically. The output filename is fixed to `Ensrick Lux Water CS Patch.esp` so configuration, exclusions, and the generated artifact cannot silently diverge.

## License and ancestry

GPL-3.0-only, matching the upstream project. Based on `mindflvx/WaterForENBPatcherCS`, which credits the original work by Panthuncia.
