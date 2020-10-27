using System;
using System.Reflection;

using PaintDotNet;

namespace TFMFileType
{
    public sealed class TFMPluginSupportInfo : IPluginSupportInfo
    {
        public string DisplayName => "TFM FileType";

        public string Author => "FlashX";

        public string Copyright
        {
            get
            {
                object[] attributes = typeof(TFMPluginSupportInfo).Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public Version Version => typeof(TFMPluginSupportInfo).Assembly.GetName().Version;

        public Uri WebsiteUri => new Uri(@"https://github.com/FlashX64/pdn-tfm-filetype");
    }
}
