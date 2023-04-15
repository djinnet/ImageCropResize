using ImageCropResize.WinCore.Core;
using ImageCropResize.WinCore.DataContainers;
using ImageCropResize.WinCore.Handlers.SettingStoreHandler;
using ImageCropResize.WinCore.Localization;
using ImageCropResize.WinCore.Models;

namespace ImageCropResize;

public partial class SaveSettings : Form, ISaveSettings
{
    private bool SelectByMouse { get; set; } = false;
    private IStoreSettingHandler StoreSettingHandler { get; set; }


    public SaveSettings(IStoreSettingHandler StoreSettingHandler)
    {
        this.StoreSettingHandler = StoreSettingHandler;
        FormStore.SaveSettings = this;
        InitializeComponent();
        LoadPresetLabels();

        SettingHowTo_Label.Text = Language.SettingHowTo;
    }

    private void LoadPresetLabels()
    {
        SettingDataStore.PresetList.Preset_1.SetUIPreset(Preset1_NameLabel, Preset1_XYLabel, Preset1_WidthHeightLabel, Preset1_ResizeWHLabel);
        SettingDataStore.PresetList.Preset_2.SetUIPreset(Preset2_NameLabel, Preset2_XYLabel, Preset2_WidthHeightLabel, Preset2_ResizeWHLabel);
        SettingDataStore.PresetList.Preset_3.SetUIPreset(Preset3_NameLabel, Preset3_XYLabel, Preset3_WidthHeightLabel, Preset3_ResizeWHLabel);
    }

    // Highlights box content on mouse or tab
    private void SettingBox_HighlightEnter(object sender, EventArgs e)
    {
        NumericUpDown? focusedBox = sender as NumericUpDown;
        focusedBox?.Select();
        focusedBox?.Select(0, focusedBox.Text.Length);
        if (MouseButtons == MouseButtons.Left)
        {
            SelectByMouse = true;
        }
    }

    private void SettingBox_HighlightMouseDown(object sender, MouseEventArgs e)
    {
        NumericUpDown? focusedBox = sender as NumericUpDown;
        if (SelectByMouse)
        {
            focusedBox?.Select(0, focusedBox.Text.Length);
            SelectByMouse = false;
        }
    }

    // Enables Resize button if Width & Height is more than 0
    private void SettingWHBox_EnableResizeIfValid(object sender, EventArgs e)
    {
        if (SettingWidthBox.Value != 0 && SettingHeightBox.Value != 0)
        {
            EnableResizeCheckBox.Enabled = true;
        }
    }

    // Allows use of Resize Width & Height boxes
    private void ResizeCheckBox_EnableResize(object sender, MouseEventArgs e)
    {
        if (EnableResizeCheckBox.Checked == true)
        {
            SettingResizeWidthBox.ReadOnly = false;
            SettingResizeHeightBox.ReadOnly = false;
        }
        else
        {
            SettingResizeWidthBox.ReadOnly = true;
            SettingResizeHeightBox.ReadOnly = true;
        }
    }

    // Changes display of Resize depending on Resize Button value 
    private void ResizeCheckBox_Click(object sender, EventArgs e)
    {
        if (EnableResizeCheckBox.Text == "Enable Resize")
        {
            EnableResizeCheckBox.Text = "Disable Resize";
            ResizeWidthLabel.ForeColor = Color.Black;
            ResizeHeightLabel.ForeColor = Color.Black;
            SettingResizeWidthBox.Enabled = true;
            SettingResizeWidthBox.ReadOnly = false;
            SettingResizeHeightBox.Enabled = true;
            SettingResizeHeightBox.ReadOnly = false;
        }
        else
        {
            EnableResizeCheckBox.Text = "Enable Resize";
            ResizeWidthLabel.ForeColor = default;
            ResizeHeightLabel.ForeColor = default;
            SettingResizeWidthBox.Enabled = false;
            SettingResizeWidthBox.Value = 0;
            SettingResizeWidthBox.ReadOnly = true;
            SettingResizeHeightBox.Enabled = false;
            SettingResizeHeightBox.Value = 0;
            SettingResizeHeightBox.ReadOnly = true;
        }

    }

    // Aspect ratio for Resize Height & Width boxes when values changed, respectively
    private void SettingResizeHeightBox_AspectRatio(object sender, EventArgs e)
    {
        if (SettingWidthBox.Value == 0 || SettingHeightBox.Value == 0)
        {
            return;
        }

        if (SettingHeightBox.Value > SettingResizeHeightBox.Value)
        {
            Decimal imageWidth = SettingWidthBox.Value;
            Decimal imageHeight = SettingHeightBox.Value;
            Decimal quotient = (imageHeight / imageWidth);
            Decimal resizeWidthBoxValue = SettingResizeWidthBox.Value;
            SettingResizeHeightBox.Value = Decimal.Round((quotient * resizeWidthBoxValue));
        }
    }

    private void SettingResizeWidthBox_AspectRatio(object sender, EventArgs e)
    {
        if (SettingWidthBox.Value == 0 || SettingHeightBox.Value == 0)
        {
            return;
        }
        if (SettingWidthBox.Value > SettingWidthBox.Value)
        {
            Decimal imageWidth = SettingWidthBox.Value;
            Decimal imageHeight = SettingHeightBox.Value;
            Decimal quotient = (imageHeight / imageWidth);
            Decimal resizeWidthBoxValue = SettingResizeWidthBox.Value;
            SettingResizeHeightBox.Value = Decimal.Round((quotient * resizeWidthBoxValue));
        }
    }

    // Cancel button closes window
    private void CloseWindowButton_Click(object sender, EventArgs e)
    {
        StoreSettingHandler.SaveJson();
        FormStore.Window.EnabledSaveButton(true);
        FormStore.Window.LoadPresetLabels();
        FormStore.Window.EnableOrDisableButtons();
        Close();
    }

    // Settings save to or override preset buttons 

    private void Preset1Button_Click(object sender, EventArgs e)
    {
        if (SettingResizeHeightBox.Value > SettingHeightBox.Value || SettingResizeWidthBox.Value > SettingResizeWidthBox.Value)
        {
            MessageBox.Show("Resize values must be smaller than crop values");
            return;
        }

        if (SettingDataStore.PresetList.Preset_1.XValue > 0)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to override your preset settings?", "Caption", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        SettingDataStore.PresetList.Preset_1.Name = SettingNameTextBox.Text;
        SettingDataStore.PresetList.Preset_1.XValue = ((int)SettingXBox.Value);
        SettingDataStore.PresetList.Preset_1.YValue = ((int)SettingYBox.Value);
        SettingDataStore.PresetList.Preset_1.Width = ((int)SettingWidthBox.Value);
        SettingDataStore.PresetList.Preset_1.Height = ((int)SettingHeightBox.Value);

        SettingDataStore.PresetList.Preset_1.SetResizeHeightAndWidth(EnableResizeCheckBox.Checked, (int)SettingResizeWidthBox.Value, (int)SettingResizeHeightBox.Value);

        StoreSettingHandler.SaveJson();
        LoadPresetLabels();
    }

    private void Preset2Button_Click(object sender, EventArgs e)
    {
        if (SettingResizeHeightBox.Value > SettingHeightBox.Value || SettingResizeWidthBox.Value > SettingResizeWidthBox.Value)
        {
            MessageBox.Show("Resize values must be smaller than crop values");
            return;
        }

        if (SettingDataStore.PresetList.Preset_2.XValue > 0)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to override your preset settings?", "Caption", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        SettingDataStore.PresetList.Preset_2.Name = SettingNameTextBox.Text;
        SettingDataStore.PresetList.Preset_2.XValue = ((int)SettingXBox.Value);
        SettingDataStore.PresetList.Preset_2.YValue = ((int)SettingYBox.Value);
        SettingDataStore.PresetList.Preset_2.Width = ((int)SettingWidthBox.Value);
        SettingDataStore.PresetList.Preset_2.Height = ((int)SettingHeightBox.Value);

        SettingDataStore.PresetList.Preset_2.SetResizeHeightAndWidth(EnableResizeCheckBox.Checked, (int)SettingResizeWidthBox.Value, (int)SettingResizeHeightBox.Value);

        StoreSettingHandler.SaveJson();
        LoadPresetLabels();
    }

    private void Preset3Button_Click(object sender, EventArgs e)
    {
        if (SettingResizeHeightBox.Value > SettingHeightBox.Value || SettingResizeWidthBox.Value > SettingResizeWidthBox.Value)
        {
            MessageBox.Show("Resize values must be smaller than crop values");
            return;
        }

        if (SettingDataStore.PresetList.Preset_3.XValue > 0)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to override your preset settings?", "Caption", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        SettingDataStore.PresetList.Preset_3.Name = SettingNameTextBox.Text;
        SettingDataStore.PresetList.Preset_3.XValue = ((int)SettingXBox.Value);
        SettingDataStore.PresetList.Preset_3.YValue = ((int)SettingYBox.Value);
        SettingDataStore.PresetList.Preset_3.Width = ((int)SettingWidthBox.Value);
        SettingDataStore.PresetList.Preset_3.Height = ((int)SettingHeightBox.Value);


        SettingDataStore.PresetList.Preset_3.SetResizeHeightAndWidth(EnableResizeCheckBox.Checked, (int)SettingResizeWidthBox.Value, (int)SettingResizeHeightBox.Value);


        StoreSettingHandler.SaveJson();
        LoadPresetLabels();
    }

    private void Checkbox_CheckedChanged(object sender, EventArgs e)
    {
        if (Preset1Checkbox.Checked == true || Preset2Checkbox.Checked == true || Preset3Checkbox.Checked == true)
        {
            ResetPresetsButton.Enabled = true;
        }
        else
        {
            ResetPresetsButton.Enabled = false;
        }
    }

    private void ResetPresetsButton_Click(object sender, EventArgs e)
    {
        if (Preset1Checkbox.Checked == true)
        {
            SettingDataStore.PresetList.Preset_1.Reset(1);
            Preset1Checkbox.Checked = false;
        }

        if (Preset2Checkbox.Checked == true)
        {
            SettingDataStore.PresetList.Preset_2.Reset(2);
            Preset2Checkbox.Checked = false;
        }

        if (Preset3Checkbox.Checked == true)
        {
            SettingDataStore.PresetList.Preset_3.Reset(3);
            Preset3Checkbox.Checked = false;
        }

        ResetPresetsButton.Enabled = false;

        LoadPresetLabels();
    }
}