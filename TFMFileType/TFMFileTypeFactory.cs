using System;

using PaintDotNet;

namespace TFMFileType
{
    public class TFMFileTypeFactory : IFileTypeFactory
    {
        public FileType[] GetFileTypeInstances() => new FileType[] { new TFMFileType() };
    }
}
