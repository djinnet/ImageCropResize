namespace ImageCropResize.WinCore.Models;

public interface ISettingDataStore
{
    void LoadAllPresets(PresetList presets);
    void SetList(PresetList presetList);
}