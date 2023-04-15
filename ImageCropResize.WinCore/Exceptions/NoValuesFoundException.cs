using ImageCropResize.WinCore.Localization;

namespace ImageCropResize.WinCore.Exceptions;
public class NoValuesFoundException : Exception
{
    static readonly string ErrorMessage = Language.NoValuesSet_Message;
    

    public NoValuesFoundException() : base(ErrorMessage)
    {


    }
}
