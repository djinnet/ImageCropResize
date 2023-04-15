using ImageCropResize.WinCore.Localization;

namespace ImageCropResize.WinCore.Exceptions;
public class NoImageInClipboardException : Exception
{
    static readonly string ErrorMessage = Language.NotAnImage_Message;

    public NoImageInClipboardException() : base(ErrorMessage)
    {


    }

    public NoImageInClipboardException(Exception inner) : base(ErrorMessage, inner)
    {


    }
}
