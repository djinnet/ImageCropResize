namespace ImageCropResize.WinCore.Models;
public class ImagesData
{
    public Image? OriginalImage { get; set; } = null;
    public SizeF OriginalImageDimensions { get; set; } = new SizeF(0, 0);
    public SizeF ConvertedImageDimensions { get; set; } = new SizeF(0, 0);
    public Bitmap? ImageConvertedToBitmap { get; set; } = null;
    public Bitmap? CroppedImage { get; set; } = null;
    public Bitmap? ResizedImage { get; set; } = null;
}   
