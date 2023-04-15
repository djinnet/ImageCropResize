using ImageCropResize.WinCore.Localization;

namespace ImageCropResize.WinCore.Exceptions;
public class UnableToConvertImageException : Exception
{
    static readonly string ErrorMessage = Language.UnableToConvertTheImage_Message;

    public UnableToConvertImageException() : base(ErrorMessage)
    {
        

    }

    public UnableToConvertImageException(Exception inner) : base(ErrorMessage, inner)
    {


    }
}
