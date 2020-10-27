using System;
using System.IO;

using PaintDotNet;

namespace TFMFileType
{
    [PluginSupportInfo(typeof(TFMPluginSupportInfo))]
    public class TFMFileType : FileType
    {
        public TFMFileType() : base("TFM", new FileTypeOptions()
        {
            LoadExtensions = new string[] { ".tfm" },
            SaveExtensions = new string[] { ".tfm" }
        })
        {

        }

        public override SaveConfigWidget CreateSaveConfigWidget() => new TFMSaveConfigWidget();

        protected override SaveConfigToken OnCreateDefaultSaveConfigToken() => new TFMSaveConfigToken(TFMPixelFormat.TFM8Bit, true);

        protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler callback)
        {
            using (RenderArgs args = new RenderArgs(new Surface(input.Size)))
            {
                input.Render(args, true);
                TFM.Save(args.Bitmap, output, token as TFMSaveConfigToken);
            }
        }

        protected override Document OnLoad(Stream input)
        {
            var bmp = TFM.Load(input);

            return Document.FromImage(bmp);
        }
    }
}
