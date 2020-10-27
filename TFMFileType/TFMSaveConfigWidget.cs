using System;
using System.Windows.Forms;

using PaintDotNet;

namespace TFMFileType
{
    public sealed partial class TFMSaveConfigWidget : SaveConfigWidget
    {
        public TFMSaveConfigWidget()
        {
            InitializeComponent();
        }

        protected override void InitFileType()
        {
            this.fileType = new TFMFileType();
        }

        protected override void InitWidgetFromToken(SaveConfigToken token)
        {
            TFMSaveConfigToken t = token as TFMSaveConfigToken;

            PixelFormat.SelectedIndex = (int)t.PixelFormat;
            GenerateMipmaps.Checked = t.GenerateMipmaps;
        }

        protected override void InitTokenFromWidget()
        {
            TFMSaveConfigToken t = this.token as TFMSaveConfigToken;

            t.PixelFormat = (TFMPixelFormat)PixelFormat.SelectedIndex;
            t.GenerateMipmaps = GenerateMipmaps.Checked;
        }

        private void PixelFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PixelFormat.SelectedIndex == 0) PixelFormat.SelectedIndex++;

            UpdateToken();
        }

        private void GenerateMipmaps_CheckedChanged(object sender, EventArgs e)
        {
            UpdateToken();
        }
    }
}
