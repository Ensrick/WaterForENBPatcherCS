namespace WaterForENBPatcherCS.Settings;

internal sealed class Settings
{
    public string WaterForEnbModName { get; set; } = "Water for ENB (Shades of Skyrim).esp";

    public List<string> WaterFamilyPlugins { get; set; } =
    [
        "Water for ENB - Patch - Beyond Reach.esp",
        "Water for ENB - Patch - Beyond Skyrim.esp",
        "Water for ENB - Patch - Wyrmstooth.esp",
        "WENB Shades USSEP.esp",
    ];

    public List<string> ModsToSkip { get; set; } =
    [
        "DynDOLOD.esm",
        "Occlusion.esp",
        "Synthesis.esp",
        "Requiem for the Indifferent.esp",
        "Bashed Patch, 0.esp",
    ];
}
