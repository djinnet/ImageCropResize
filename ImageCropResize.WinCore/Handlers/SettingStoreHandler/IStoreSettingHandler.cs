namespace ImageCropResize.WinCore.Handlers.SettingStoreHandler;

public interface IStoreSettingHandler
{
    void CreateOrReadSettingJson();
    void SaveJson();
}