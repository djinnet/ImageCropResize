using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ImageCropResize.WinCore.Models;
[Serializable]
public class PresetList
{
    public List<Preset> Presets { get; set; } = new List<Preset>();


    public PresetList()
    {
    }

    public static PresetList CreateEmptyPresetList()
    {
        var emptyData = new PresetList();
        emptyData.Presets.Clear(); //Just to ensure we dont save anything from previous memory
        emptyData.Presets.Add(new Preset("Preset 1"));
        emptyData.Presets.Add(new Preset("Preset 2"));
        emptyData.Presets.Add(new Preset("Preset 3"));
        return emptyData;
    }

    [XmlIgnore]
    [JsonIgnore]
    public Preset Preset_1
    {
        get
        {
            return Presets[0];
        }
        set
        {
            Presets[0] = value;
        }
    }

    [XmlIgnore]
    [JsonIgnore]
    public Preset Preset_2
    {
        get
        {
            return Presets[1];
        }
        set 
        {
            Presets[1] = value; 
        }
    }

    [XmlIgnore]
    [JsonIgnore]
    public Preset Preset_3
    {
        get
        {
            return Presets[2];
        }
        set
        {
            Presets[2] = value;
        }
    }
}
