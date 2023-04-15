using ImageCropResize.WinCore.Enums;
using ImageCropResize.WinCore.Handlers.FileHandler;
using ImageCropResize.WinCore.Models;
using Newtonsoft.Json.Linq;
using System.Configuration;


namespace ImageCropResize.WinCore.Core;
public class SettingManager : ISettingManager
{
    public string OutputDirectory => Path.Combine(Directory.GetCurrentDirectory(), "Settings");
    public string FileName => "PresetsSettings";

    public bool UpdateAsJson<T>(T value) where T : class
    {
        try
        {
            var fileHandler = new FileHandlerGeneric<T>(OutputDirectory + $@"\{FileName}.json", EFile_Type.Json);
            //update files with new values
            fileHandler.Update(value);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to update json with the value: {value}, exception: {e.Message}");
            return false;
        }
    }

    public T? CreateAsJson<T>(string path, string filename, T value) where T : class
    {
        try
        {
            var fileHandler = new FileHandlerGeneric<T>(path, EFile_Type.Json);
            return fileHandler.Create(value, filename);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to create json with the value: {value}, exception: {e.Message}, on path: {path}, filename: {filename}");
            return null;
        }
    }

    public T? ReadAsJson<T>(string path) where T : class
    {
        try
        {
            var fileHandler = new FileHandlerGeneric<T>(path, EFile_Type.Json);
            return fileHandler.Read();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to read json with the path: {path}, exception: {e.Message}");
            return null;
        }
    }
}
