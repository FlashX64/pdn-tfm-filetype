using System;

using PaintDotNet;

namespace TFMFileType
{
    [Serializable]
    public sealed class TFMSaveConfigToken : SaveConfigToken
    {
        public TFMSaveConfigToken(TFMPixelFormat pxFormat, bool generateMipmaps)
        {
            PixelFormat = pxFormat;
            GenerateMipmaps = generateMipmaps;
        }

        private TFMSaveConfigToken(TFMSaveConfigToken instance)
        {
            PixelFormat = instance.PixelFormat;
            GenerateMipmaps = instance.GenerateMipmaps;
        }

        public TFMPixelFormat PixelFormat { get; set; }
        public bool GenerateMipmaps { get; set; }

        public override object Clone() => new TFMSaveConfigToken(this);
    }
}
