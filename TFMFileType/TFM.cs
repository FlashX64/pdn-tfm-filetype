using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using PaintDotNet.IO;

namespace TFMFileType
{
    public enum TFMPixelFormat : UInt16
    {
        TFM8Bit = 0,
        TFM565 = 1,
        TFM4444 = 2,
        TFM1555 = 3
    }

    public static unsafe class TFM
    {
        public const UInt32 Signature = 0x006D6674;
        public const UInt16 CurrentVersion = 1;

        public static Bitmap Load(Stream input)
        {
            Bitmap bmp = null;

            using (var br = new BinaryReader(input))
            {
                // Reading TFM header
                if (br.ReadUInt32() != Signature) throw new FormatException("Invalid TFM file.");

                TFMPixelFormat pxFormat;
                UInt16 version, sizeX, mipmaps;

                version = br.ReadUInt16();
                if (version != CurrentVersion) throw new FormatException($"Unsupported TFM version '{version}'.");

                pxFormat = (TFMPixelFormat)input.ReadUInt16();

                if (!Enum.IsDefined(typeof(TFMPixelFormat), pxFormat)) throw new FormatException($"Unsupported TFM pixel format '{(uint)pxFormat}'.");

                sizeX = br.ReadUInt16();
                int size = sizeX * sizeX;

                mipmaps = br.ReadUInt16();
                byte[] rgb = new byte[size * 4];

                switch (pxFormat)
                {
                    case TFMPixelFormat.TFM8Bit:
                        {
                            byte[] colorTable = br.ReadBytes(256 * 3);

                            byte[] buf = br.ReadBytes(size);

                            int tableIndex;
                            for (int j = 0, rgbIndex = 0; j < size; j++)
                            {
                                tableIndex = buf[j];
                                rgb[rgbIndex++] = colorTable[3 * tableIndex + 2];
                                rgb[rgbIndex++] = colorTable[3 * tableIndex + 1];
                                rgb[rgbIndex++] = colorTable[3 * tableIndex];
                                rgb[rgbIndex++] = 255;
                            }

                            break;
                        }
                    case TFMPixelFormat.TFM565:
                        {
                            UInt16[] buf = new UInt16[size];

                            for (int j = 0; j < size; j++) buf[j] = br.ReadUInt16();

                            UInt16 pix;
                            for (int j = 0, rgb_index = 0; j < size; j++)
                            {
                                pix = buf[j];
                                rgb[rgb_index++] = (byte)((pix & 31) << 3);
                                rgb[rgb_index++] = (byte)(((pix >> 5) & 63) << 2);
                                rgb[rgb_index++] = (byte)((pix >> 11) << 3);
                                rgb[rgb_index++] = 255;
                            }

                            break;
                        }
                    case TFMPixelFormat.TFM4444:
                        {
                            UInt16[] buf = new UInt16[size];

                            for (int j = 0; j < size; j++) buf[j] = br.ReadUInt16();

                            UInt16 pix;
                            for (int j = 0, rgb_index = 0; j < size; j++)
                            {
                                pix = buf[j];

                                rgb[rgb_index++] = (byte)((pix & 15) << 4);
                                rgb[rgb_index++] = (byte)(((pix >> 4) & 15) << 4);
                                rgb[rgb_index++] = (byte)(((pix >> 8) & 15) << 4);
                                rgb[rgb_index++] = (byte)((pix >> 12) << 4);
                            }

                            break;
                        }
                    case TFMPixelFormat.TFM1555:
                        {
                            UInt16[] buf = new UInt16[size];

                            for (int j = 0; j < size; j++) buf[j] = br.ReadUInt16();

                            UInt16 pix;
                            for (int j = 0, rgb_index = 0; j < size; j++)
                            {
                                pix = buf[j];

                                rgb[rgb_index++] = (byte)((pix & 31) << 3);
                                rgb[rgb_index++] = (byte)(((pix >> 5) & 31) << 3);
                                rgb[rgb_index++] = (byte)(((pix >> 10) & 31) << 3);
                                rgb[rgb_index++] = (byte)((pix >> 15) << 7);
                            }

                            break;
                        }
                }


                fixed (byte* ptr = rgb)
                {
                    bmp = new Bitmap(sizeX, sizeX, sizeX * 4, PixelFormat.Format32bppArgb, (IntPtr)ptr);
                }
            }

            return bmp;
        }

        public static void Save(Bitmap bmp, Stream output, TFMSaveConfigToken token)
        {
            bool IsPowerOfTwo(int n) => (int)(Math.Ceiling((Math.Log(n) / Math.Log(2)))) == (int)(Math.Floor(((Math.Log(n) / Math.Log(2)))));
            BitmapData GetBmpData(Bitmap b, PixelFormat pxFormat) => b.LockBits(new Rectangle(Point.Empty, b.Size), ImageLockMode.ReadOnly, pxFormat);

            if (bmp.Width != bmp.Height) throw new FormatException("The image width must be equal to the image height.");
            if (bmp.Width >= UInt16.MaxValue) throw new FormatException("Maximum available image resolution exceeded.");
            if (!IsPowerOfTwo(bmp.Width)) throw new FormatException("The image resolution must be a power of two.");

            UInt16 mipmapCount = 1;
            UInt16 sizeX = (UInt16)bmp.Width;

            #region Mipmaps
            // Get available mipmaps count
            if (token.GenerateMipmaps)
            {
                UInt16 mipSize = sizeX;

                while (mipSize >= 2)
                {
                    mipSize /= 2;
                    mipmapCount++;
                }
            }

            // Create mipmaps
            Bitmap[] mips = new Bitmap[mipmapCount];
            mips[0] = bmp;

            if (token.GenerateMipmaps)
            {
                UInt16 mipSize = sizeX;

                for (int i = 1; i < mipmapCount; i++)
                {
                    mipSize /= 2;
                    mips[i] = CreateMipmap(bmp, mipSize);
                }
            }
            #endregion

            var bw = new BinaryWriter(output);
            bw.Write(Signature);
            bw.Write(CurrentVersion);
            bw.Write((UInt16)token.PixelFormat);
            bw.Write(sizeX);
            bw.Write(mipmapCount);

            switch (token.PixelFormat)
            {
                case TFMPixelFormat.TFM565:
                    {
                        UInt16 pix;

                        foreach (Bitmap mip in mips)
                        {
                            BitmapData bmpData = GetBmpData(mip, PixelFormat.Format24bppRgb);
                            byte* dataPtr = (byte*)bmpData.Scan0;

                            for (int i = 0; i < mip.Width * mip.Width; i++)
                            {
                                pix = (UInt16)(((*dataPtr++ >> 3) | ((*dataPtr++ >> 2) << 5) | (*dataPtr++ >> 3) << 11));
                                bw.Write(pix);
                            }

                            mip.UnlockBits(bmpData);
                        }

                        break;
                    }
                case TFMPixelFormat.TFM4444:
                    {
                        UInt16 pix;

                        foreach (Bitmap mip in mips)
                        {
                            BitmapData bmpData = GetBmpData(mip, PixelFormat.Format32bppArgb);
                            byte* dataPtr = (byte*)bmpData.Scan0;

                            for (int i = 0; i < mip.Width * mip.Width; i++)
                            {
                                pix = (UInt16)(((*dataPtr++ >> 4) | ((*dataPtr++ >> 4) << 4) | ((*dataPtr++ >> 4) << 8) | (*dataPtr++ >> 4) << 12));
                                bw.Write(pix);
                            }

                            mip.UnlockBits(bmpData);
                        }

                        break;
                    }
                case TFMPixelFormat.TFM1555:
                    {
                        UInt16 pix;

                        foreach (Bitmap mip in mips)
                        {
                            BitmapData bmpData = GetBmpData(mip, PixelFormat.Format32bppArgb);
                            byte* dataPtr = (byte*)bmpData.Scan0;

                            for (int i = 0; i < mip.Width * mip.Width; i++)
                            {
                                pix = (UInt16)(((*dataPtr++ >> 3) | ((*dataPtr++ >> 3) << 5) | ((*dataPtr++ >> 3) << 10) | (*dataPtr++ >> 7) << 15));
                                bw.Write(pix);
                            }

                            mip.UnlockBits(bmpData);
                        }

                        break;
                    }
            }
        }


        private static Bitmap CreateMipmap(Bitmap bmp, int sizeX)
        {
            var mipmap = new Bitmap(sizeX, sizeX);
            var g = Graphics.FromImage(mipmap);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.DrawImage(bmp, Point.Empty.X, Point.Empty.Y, sizeX, sizeX);

            return mipmap;
        }
    }
}
