using ImageCropResize.WinCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCropResize.WinCore.Core;
public static class PresetUIExtension
{
    public static void SetUIPreset(this Preset data, Control NameLabel, Label XYLabel, Label WidthHeightLabel, Label ResizeWHLabel)
    {
        NameLabel.Text = data.Name;
        XYLabel.Text = $"{data.XValue}x, {data.YValue}y";
        WidthHeightLabel.Text = $"Cropped: {data.Width}w x {data.Height}h";
        ResizeWHLabel.Text = $"Resized: {data.ResizeWidth}w x {data.ResizeHeight}h";

    }

    public static void SetButtonUIEnabled(this Preset preset, Button button)
    {
        button.Enabled = preset.PresetValidated();
    }
}
