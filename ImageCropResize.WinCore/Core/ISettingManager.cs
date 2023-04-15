namespace ImageCropResize.WinCore.Core;

public interface ISettingManager
{
    string FileName { get; }
    string OutputDirectory { get; }

    T? CreateAsJson<T>(string path, string filename, T value) where T : class;
    T? ReadAsJson<T>(string path) where T : class;
    bool UpdateAsJson<T>(T value) where T : class;
}