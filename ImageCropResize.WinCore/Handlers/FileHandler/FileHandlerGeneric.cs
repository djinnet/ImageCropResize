using ImageCropResize.WinCore.Enums;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ImageCropResize.WinCore.Handlers.FileHandler;
public class FileHandlerGeneric<T> where T : class
{
    private readonly string _basePath;
    private readonly EFile_Type _fileType = EFile_Type.None;

    public FileHandlerGeneric(string basePath, EFile_Type file_Type)
    {
        _basePath = basePath;
        _fileType = file_Type;
    }

    public bool CreateFolder()
    {
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
            return true;
        }
        return false;
    }

    public T Create(T data, string filename)
    {
        try
        {
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }

            switch (_fileType)
            {
                case EFile_Type.Xml:
                    var serializer = new XmlSerializer(typeof(T));
                    using (var stream = new StreamWriter(_basePath))
                    {
                        serializer.Serialize(stream, data);
                    }
                    break;
                case EFile_Type.Json:
                    string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                    File.WriteAllText(_basePath + filename, jsonData);
                    break;
                default:
                    throw new NotSupportedException($"File type {_fileType} is not supported.");
            }
        }
        catch (Exception es)
        {
            Console.WriteLine(es.Message);
        }

        return data;
    }

    public T? Read()
    {
        switch (_fileType)
        {
            case EFile_Type.Xml:
                var serializer = new XmlSerializer(typeof(T));
                using (var stream = new StreamReader(_basePath))
                {
                    return (T?)serializer.Deserialize(stream);
                }
            case EFile_Type.Json:
                var json = File.ReadAllText(_basePath);
                return JsonConvert.DeserializeObject<T>(json);
            default:
                throw new NotSupportedException($"File type {_fileType} is not supported.");
        }
    }

    public void Update(T data)
    {
        switch (_fileType)
        {
            case EFile_Type.Xml:
                var serializer = new XmlSerializer(typeof(T));
                using (var stream = new StreamWriter(_basePath))
                {
                    serializer.Serialize(stream, data);
                }
                break;
            case EFile_Type.Json:
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(_basePath, json);
                break;
            default:
                throw new NotSupportedException($"File type {_fileType} is not supported.");
        }
    }

    public void Delete()
    {
        File.Delete(_basePath);
    }
}
