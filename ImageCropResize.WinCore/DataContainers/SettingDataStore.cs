using ImageCropResize.WinCore.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCropResize.WinCore.Models;

//Store setting data
public class SettingDataStore : ISettingDataStore
{
    public SettingDataStore()
    {
        
    }

    public static PresetList PresetList { get; private set; }

    public void SetList(PresetList presetList)
    {
        if (presetList == null) return;

        PresetList = presetList;
    }

    public void LoadAllPresets(PresetList presets)
    {
        PresetList.Preset_1 = presets.Preset_1;
        PresetList.Preset_2 = presets.Preset_2;
        PresetList.Preset_3 = presets.Preset_3;
    }
}
