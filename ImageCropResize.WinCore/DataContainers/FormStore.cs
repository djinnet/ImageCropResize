using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCropResize.WinCore.DataContainers;
public static class FormStore
{
    public static ISaveSettings SaveSettings { get; set; }

    public static IMainWindow Window { get; set; }
}
