namespace ImageCropResize.WinCore.Models;
public class WindowData
{
    public string PresetName { get; set; } = string.Empty;
    public int XValue { get; set; }
    public int YValue { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int ResizeWidth { get; set; }
    public int ResizeHeight { get; set; }
    public bool ResizeEnabled { get; set; } = false;

    public void SetPresetValuesToWindow(Preset preset)
    {
        PresetName = preset.Name;
        XValue = preset.XValue;
        YValue = preset.YValue;
        Width = preset.Width;
        Height = preset.Height;
        ResizeWidth = preset.ResizeWidth;
        ResizeHeight = preset.ResizeHeight;
        ResizeEnabled = preset.ResizeEnabled;
    }

    //This isnt a 'true' async, but is just for showcasing of how to async properly. Use rather the sync SetPresetValuesToWindow for better purpose
    public async Task SetPresetValuesToWindowAsync(Preset preset)
    {
        PresetName = preset.Name;
        XValue = preset.XValue;
        YValue = preset.YValue;
        Width = preset.Width;
        Height = preset.Height;
        ResizeWidth = preset.ResizeWidth;
        ResizeHeight = preset.ResizeHeight;
        ResizeEnabled = preset.ResizeEnabled;
        await Task.Delay(1000);
    }
}
