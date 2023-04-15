using ImageCropResize.WinCore.Localization;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCropResize.WinCore.Core;
public class ImageCore
{
    public static async Task<Image> ResizeFinalImageAsync(Image image, int width, int height)
    {
        Rectangle finalRectangle = new (0, 0, width, height);
        Bitmap ResizedImage = new (width, height);

        ResizedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (Graphics graphics = Graphics.FromImage(ResizedImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using ImageAttributes wrapMode = new();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, finalRectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        }
        await Task.Delay(1000);
        return ResizedImage;
    }

    public static void CheckIfReadyToConvert_Message(Label MessageLabel, bool AreAllButtonDisabled = false)
    {
        if (AreAllButtonDisabled)
        {
            MessageLabel.Text = Language.SaveNewPresets_Message;
        }
        else
        {
            if (!Clipboard.ContainsImage())
            {
                MessageLabel.Text = Language.WaitingForClipboardImage_Message;
                MessageLabel.ForeColor = Color.Red;
            }
            else
            {
                MessageLabel.Text = Language.ReadyToConvert_Message;
                MessageLabel.ForeColor = Color.Green;
            }
        }
    }

    //TODO: Use this piece of code to capture desktop image then possible use this for https://www.codeproject.com/Articles/30524/An-Easy-to-Use-Image-Resizing-and-Cropping-Control
    //which Elysia can use to include for the future plans. 
    public static (Graphics? captureGraphic, Bitmap? Image) CaptureMyScreen(Rectangle rectangle)
    {
        try
        {
            //Creating a new Bitmap object
            Bitmap captureBitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb);

            
            //Creating a Rectangle object which will capture our Current Screen
            Rectangle captureRectangle = Screen.AllScreens[0].Bounds;

            //Creating a New Graphics Object
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);

            //Copying Image from The Screen
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
            return (captureGraphics, captureBitmap);
        }
        catch (Exception ex)
        {
            return (null, null);
        }
    }

}
