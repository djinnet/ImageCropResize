using ImageCropResize.WinCore.Core;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ImageCropResize.WinCore.Models;

[Serializable]
public class Preset
{
    public Preset(string name)
    {
        Name = name;
    }
    public string Name { get; set; } = string.Empty;

    public int XValue { get; set; } = 0;

    public int YValue { get; set; } = 0;

    public int Width { get; set; } = 0;

    public int Height { get; set; } = 0;

    public int ResizeHeight { get; set; } = 0;

    public int ResizeWidth { get; set;} = 0;

    public bool ResizeEnabled { get; set; } = false;

    public void SetResizeHeightAndWidth(bool EnableResizeCheckBox, int w_value, int h_value)
    {
        if (EnableResizeCheckBox)
        {
            ResizeWidth = w_value;
            ResizeHeight = h_value;
            ResizeEnabled = true;
        }
        else
        {
            ResizeWidth = 0;
            ResizeHeight = 0;
            ResizeEnabled = false;
        }
    }

    public void Reset(int number)
    {
        Name = "Save Preset " + number;
        XValue = 0;
        YValue = 0;
        Width = 0;
        Height = 0;
        ResizeWidth = 0;
        ResizeHeight = 0;
        ResizeEnabled = false;
    }

    public bool PresetValidated()
    {
        if (ResizeEnabled)
        {
            return (Width > 0 && Height > 0) || (XValue > 0 && YValue > 0) || (ResizeHeight > 0 && ResizeWidth > 0);
        }
        else
        {
            return (Width > 0 && Height > 0) || (XValue > 0 && YValue > 0);
        }
    }
}
