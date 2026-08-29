using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Assets;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Skyrim.Assets;

namespace WaterForENBPatcherCS;

internal static class SelfTest
{
    public static int Run()
    {
        try
        {
            CellCopyPreservesUnrelatedFlagsAndCanClearHasWater();
            CellCopyCanSetHasWater();
            WorldspaceCopyForwardsEveryOwnedField();
            Console.WriteLine("PASS: water-field merge self-test");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"FAIL: {exception.Message}");
            return 1;
        }
    }

    private static void CellCopyPreservesUnrelatedFlagsAndCanClearHasWater()
    {
        var source = NewCell(0x801);
        var target = NewCell(0x802);
        var water = NewFormKey("WaterSource.esp", 0x900);

        source.Water.SetTo(water);
        source.WaterEnvironmentMap = "textures\\water\\source.dds";
        source.WaterHeight = 123.5f;
        source.Flags = 0;
        target.Flags = Cell.Flag.IsInteriorCell | Cell.Flag.HasWater;

        Program.CopyCellWaterFields(target, source);

        Require(target.Water.FormKey == water, "CELL Water was not forwarded");
        Require(target.WaterEnvironmentMap == source.WaterEnvironmentMap, "CELL WaterEnvironmentMap was not forwarded");
        Require(target.WaterHeight == source.WaterHeight, "CELL WaterHeight was not forwarded");
        Require(target.Flags.HasFlag(Cell.Flag.IsInteriorCell), "CELL unrelated flag was changed");
        Require(!target.Flags.HasFlag(Cell.Flag.HasWater), "CELL HasWater was not cleared");
    }

    private static void CellCopyCanSetHasWater()
    {
        var source = NewCell(0x803);
        var target = NewCell(0x804);
        source.Flags = Cell.Flag.HasWater;
        target.Flags = 0;

        Program.CopyCellWaterFields(target, source);

        Require(target.Flags.HasFlag(Cell.Flag.HasWater), "CELL HasWater was not set");
    }

    private static void WorldspaceCopyForwardsEveryOwnedField()
    {
        var source = NewWorldspace(0x805);
        var target = NewWorldspace(0x806);
        var water = NewFormKey("WaterSource.esp", 0x901);
        var lodWater = NewFormKey("WaterSource.esp", 0x902);

        source.Water.SetTo(water);
        source.LodWater.SetTo(lodWater);
        source.LodWaterHeight = 42.25f;
        source.WaterEnvironmentMap = new AssetLink<SkyrimTextureAssetType>("textures\\water\\world.dds");

        Program.CopyWorldspaceWaterFields(target, source);

        Require(target.Water.FormKey == water, "WRLD Water was not forwarded");
        Require(target.LodWater.FormKey == lodWater, "WRLD LodWater was not forwarded");
        Require(target.LodWaterHeight == source.LodWaterHeight, "WRLD LodWaterHeight was not forwarded");
        Require(
            target.WaterEnvironmentMap?.GivenPath == source.WaterEnvironmentMap?.GivenPath,
            "WRLD WaterEnvironmentMap was not forwarded");
    }

    private static Cell NewCell(uint id) =>
        new(NewFormKey("SelfTest.esp", id), SkyrimRelease.SkyrimSE);

    private static Worldspace NewWorldspace(uint id) =>
        new(NewFormKey("SelfTest.esp", id), SkyrimRelease.SkyrimSE);

    private static FormKey NewFormKey(string plugin, uint id) =>
        new(ModKey.FromNameAndExtension(plugin), id);

    private static void Require(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
