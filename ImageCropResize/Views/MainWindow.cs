using ImageCropResize.WinCore.Core;
using ImageCropResize.WinCore.DataContainers;
using ImageCropResize.WinCore.Exceptions;
using ImageCropResize.WinCore.Exceptions.Colors;
using ImageCropResize.WinCore.Handlers.SettingStoreHandler;
using ImageCropResize.WinCore.Localization;
using ImageCropResize.WinCore.Models;

namespace ImageCropResize;

// TODO: Refactor ALL THE CODE

public partial class MainWindow : Form, IMainWindow
{
    #region Properties
    private ImagesData ImagesData { get; set; } = new ImagesData();

    private WindowData WindowData { get; set; } = new WindowData();

    private bool CheckIfAllButtonsAreDisabled => Preset1Button.Enabled == false && Preset2Button.Enabled == false && Preset3Button.Enabled == false;

    private IStoreSettingHandler StoreSettingHandler { get; set; }

    #endregion


    public MainWindow(IStoreSettingHandler storeSettingHandler)
    {
        this.StoreSettingHandler = storeSettingHandler;
        FormStore.Window = this;
        InitializeComponent();

        //subscribe to the event
        OnEnabled_SaveNewSettingsButton += MainWindow_OnEnabled_SaveNewSettingsButton;

        LoadPresetLabels();
        EnableOrDisableButtons();

        FormClosing += AppFormClosingEventHandler;
    }

    #region Events handlers + related methods
    private event EventHandler<bool>? Event_SaveNewSettingsButton;

    private event EventHandler<bool> OnEnabled_SaveNewSettingsButton
    {
        add
        {
            Event_SaveNewSettingsButton += value;
        }
        remove
        {
            Event_SaveNewSettingsButton -= value;
        }
    }

    private void MainWindow_OnEnabled_SaveNewSettingsButton(object? sender, bool value)
    {
        SaveNewSettingsButton.Enabled = value;
    }

    public void EnabledSaveButton(bool value)
    {
        Event_SaveNewSettingsButton?.Invoke(this, value);
    }

    #endregion

    private void AppFormClosingEventHandler(object? sender, FormClosingEventArgs e)
    {
        //Always unsubscribed when the application is closing
        OnEnabled_SaveNewSettingsButton -= MainWindow_OnEnabled_SaveNewSettingsButton;
        StoreSettingHandler.SaveJson();
    }

    private void MainWindow_Activated(object sender, EventArgs e)
    {
        ImageCore.CheckIfReadyToConvert_Message(MessageLabel, CheckIfAllButtonsAreDisabled);
    }


    public void LoadPresetLabels()
    {
        SettingDataStore.PresetList.Preset_1.SetUIPreset(Preset1Button, Preset1_XYLabel, Preset1_WidthHeightLabel, Preset1_ResizeWHLabel);
        SettingDataStore.PresetList.Preset_2.SetUIPreset(Preset2Button, Preset2_XYLabel, Preset2_WidthHeightLabel, Preset2_ResizeWHLabel);
        SettingDataStore.PresetList.Preset_3.SetUIPreset(Preset3Button, Preset3_XYLabel, Preset3_WidthHeightLabel, Preset3_ResizeWHLabel);
        
        ImageCore.CheckIfReadyToConvert_Message(MessageLabel, CheckIfAllButtonsAreDisabled);
    }

    public void EnableOrDisableButtons()
    {
        SettingDataStore.PresetList.Preset_1.SetButtonUIEnabled(Preset1Button);
        SettingDataStore.PresetList.Preset_2.SetButtonUIEnabled(Preset2Button);
        SettingDataStore.PresetList.Preset_3.SetButtonUIEnabled(Preset3Button);
    }


    private void SaveNewSettingsButton_Click(object sender, EventArgs e)
    {
        SaveNewSettingsButton.Enabled = false;
        var settings = new SaveSettings(StoreSettingHandler);
        settings.Show();
    }

    private void UndoButton_Click(object sender, EventArgs e)
    {
        ConvertedImageLabel.Text = Language.ConvertedImageLabelText;
        MessageLabel.Text = Language.Undo_Message;
        if (ImagesData.OriginalImage != null)
        {
            Clipboard.SetImage(ImagesData.OriginalImage);
        }

        CroppedImageBox.Image = null;
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
        StoreSettingHandler.SaveJson();
        Application.Exit();
    }

    private async void Preset1Button_Click(object sender, EventArgs e)
    {
        try
        {
            Preset1Button.Enabled = false;

            if (SettingDataStore.PresetList.Preset_1.Width == 0)
            {
                throw new NoValuesFoundException();
            }

            await WindowData.SetPresetValuesToWindowAsync(SettingDataStore.PresetList.Preset_1);

            await GrabImageFromClipboard();

            await Task.Delay(3000);
            Preset1Button.Enabled = true;
        }
        catch (NoValuesFoundException ex)
        {
            MessageLabel.Text = ex.Message;
            MessageLabel.ForeColor = ExceptionColors.ErrorColor;
        }
    }

    private async void Preset2Button_Click(object sender, EventArgs e)
    {
        try
        {
            Preset2Button.Enabled = false;

            if (SettingDataStore.PresetList.Preset_2.Width == 0)
            {
                throw new NoValuesFoundException();
            }

            await WindowData.SetPresetValuesToWindowAsync(SettingDataStore.PresetList.Preset_2);

            await GrabImageFromClipboard();

            await Task.Delay(3000);
            Preset2Button.Enabled = true;
        }
        catch (NoValuesFoundException ex)
        {
            MessageLabel.Text = ex.Message;
            MessageLabel.ForeColor = ExceptionColors.ErrorColor;
        }
    }

    private async void Preset3Button_Click(object sender, EventArgs e)
    {
        try
        {
            Preset3Button.Enabled = false;

            if (SettingDataStore.PresetList.Preset_3.Width == 0)
            {
                throw new NoValuesFoundException();
            }

            await WindowData.SetPresetValuesToWindowAsync(SettingDataStore.PresetList.Preset_3);

            await GrabImageFromClipboard();

            await Task.Delay(3000);
            Preset3Button.Enabled = true;
        }
        catch (NoValuesFoundException ex)
        {
            MessageLabel.Text = ex.Message;
            MessageLabel.ForeColor = ExceptionColors.ErrorColor;
        }
    }

    private async Task GrabImageFromClipboard()
    {
        try
        {
            if (!Clipboard.ContainsImage())
            {
                throw new NoImageInClipboardException();
            }

            //just checking if the image arent an null because the datatype could be nullable
            Image? ClipboardImage = Clipboard.GetImage() ?? throw new NoImageInClipboardException();
            ImagesData.OriginalImage = ClipboardImage;
            ImagesData.OriginalImageDimensions = ClipboardImage.PhysicalDimension;
            ImagesData.ImageConvertedToBitmap = new Bitmap(ImagesData.OriginalImage);

            OriginalImageBox.Image = ImagesData.ImageConvertedToBitmap;
            OriginalImageBox.SizeMode = PictureBoxSizeMode.StretchImage;

            OriginalImageLabel.Text = $"Original Image: {ImagesData.OriginalImageDimensions.Width}W x {ImagesData.OriginalImageDimensions.Height}H";

            await CropOriginalImage(ImagesData.ImageConvertedToBitmap);
        }
        catch (NoImageInClipboardException ex)
        {
            MessageLabel.Text = ex.Message;
            MessageLabel.ForeColor = ExceptionColors.ErrorColor;
        }
    }

    private async Task CropOriginalImage(Bitmap image)
    {
        try
        {
            //TODO: Check this if width/height switched, breaks at Clone
            if ((image.Width < Width) || (image.Height < Height))
            {
                throw new UnableToConvertImageException();
            }

            try
            {
                Rectangle cropArea = new (WindowData.XValue, WindowData.YValue, WindowData.Width, WindowData.Height);
                ImagesData.CroppedImage = image.Clone(cropArea, image.PixelFormat);

                CroppedImageBox.Image = ImagesData.CroppedImage;
                CroppedImageBox.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            catch (Exception ex)
            {
                throw new UnableToConvertImageException(ex);
            }

            if (WindowData.ResizeEnabled == false)
            {
                ImagesData.ConvertedImageDimensions = ImagesData.CroppedImage.PhysicalDimension;
                ConvertedImageLabel.Text = $"Converted Image: {ImagesData.ConvertedImageDimensions.Width}W x {ImagesData.ConvertedImageDimensions.Height}H";
                Clipboard.SetImage(ImagesData.CroppedImage);
            }

            var ResizedImage = await ImageCore.ResizeFinalImageAsync(ImagesData.CroppedImage, WindowData.ResizeWidth, WindowData.ResizeHeight);

            ImagesData.ConvertedImageDimensions = ResizedImage.PhysicalDimension;
            ConvertedImageLabel.Text = $"Converted Image: {ImagesData.ConvertedImageDimensions.Width}W x {ImagesData.ConvertedImageDimensions.Height}H";
            Clipboard.SetImage(ResizedImage);

            MessageLabel.Text = Language.Success_Message;
            MessageLabel.ForeColor = Color.Green;
            UndoButton.Enabled = true;
        }
        catch (UnableToConvertImageException customException)
        {
            MessageLabel.Text = customException.Message;
            MessageLabel.ForeColor = ExceptionColors.ErrorColor;
            CroppedImageBox.Image = null;
            ConvertedImageLabel.Text = Language.ConvertedImageLabelText;
        }
    }
}