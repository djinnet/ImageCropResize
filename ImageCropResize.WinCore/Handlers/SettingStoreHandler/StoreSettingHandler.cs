using ImageCropResize.WinCore.Core;
using ImageCropResize.WinCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCropResize.WinCore.Handlers.SettingStoreHandler;
public class StoreSettingHandler : IStoreSettingHandler
{
    ISettingManager Manager { get; set; }
    ISettingDataStore DataStore { get; set; }
    public StoreSettingHandler(ISettingManager manager, ISettingDataStore dataStore)
    {
        Manager = manager;
        DataStore = dataStore;
    }

    public void CreateOrReadSettingJson()
    {
        PresetList? emptyData = PresetList.CreateEmptyPresetList();

        try
        {
            if (!Directory.Exists(Manager.OutputDirectory))
            {
                //create data
                emptyData = Manager.CreateAsJson(Manager.OutputDirectory, $@"\{Manager.FileName}.json", emptyData);
            }
            else
            {
                //read the json to data
                emptyData = Manager.ReadAsJson<PresetList>(Manager.OutputDirectory + $@"\{Manager.FileName}.json");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            //always set in store
            if (emptyData != null)
            {
                DataStore.SetList(emptyData);
                DataStore.LoadAllPresets(emptyData);
            }
        }
    }

    public void SaveJson()
    {
        if (SettingDataStore.PresetList != null)
        {
            Manager.UpdateAsJson(SettingDataStore.PresetList);
        }
    }
}
